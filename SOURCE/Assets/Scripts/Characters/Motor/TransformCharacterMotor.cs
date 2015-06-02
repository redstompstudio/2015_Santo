#define DEBUG_CC2D_RAYS

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TransformCharacterMotor : BaseCharacterMotor
{
#region COMPONENTS
	[Header("COMPONENTS SETTINGS")]
	[HideInInspector][NonSerialized]
	public CharacterCollisionState2D collisionState = new CharacterCollisionState2D();

	/// <summary>
	/// holder for our raycast origin corners (TR, TL, BR, BL)
	/// </summary>
	private CharacterRaycastOrigins _raycastOrigins;

#endregion

#region INTERNAL TYPES
	private struct CharacterRaycastOrigins
	{
		public Vector3 topLeft;
		public Vector3 bottomRight;
		public Vector3 bottomLeft;
	}

	public class CharacterCollisionState2D
	{
		public bool right;
		public bool left;
		public bool above;
		public bool below;
		public bool becameGroundedThisFrame;
		public bool wasGroundedLastFrame;
		public bool movingDownSlope;
		public float slopeAngle;


		public bool HasCollision()
		{
			return below || right || left || above;
		}


		public void Reset()
		{
			right = left = above = below = becameGroundedThisFrame = movingDownSlope = false;
			slopeAngle = 0f;
		}


		public override string ToString()
		{
			return string.Format( "[CharacterCollisionState2D] r: {0}, l: {1}, a: {2}, b: {3}, movingDownSlope: {4}, angle: {5}, wasGroundedLastFrame: {6}, becameGroundedThisFrame: {7}",
				right, left, above, below, movingDownSlope, slopeAngle, wasGroundedLastFrame, becameGroundedThisFrame );
		}
	}
#endregion

	public event Action<RaycastHit> onControllerCollidedEvent;

	[SerializeField]
	[Range( 0.001f, 0.3f )]
	private float skinWidth = 0.02f;
	private const float kSkinWidthFloatFudgeFactor = 0.001f;

	[HideInInspector][NonSerialized]
	public Vector3 velocity;

	/// <summary>
	/// the threshold in the change in vertical movement between frames that constitutes jumping
	/// </summary>
	/// <value>The jumping threshold.</value>
	public float jumpingThreshold = 0.07f;

	/// <summary>
	/// stores our raycast hit during movement
	/// </summary>
	private RaycastHit _raycastHit;

	/// <summary>
	/// stores any raycast hits that occur this frame. we have to store them in case we get a hit moving
	/// horizontally and vertically so that we can send the events after all collision state is set
	/// </summary>
	private List<RaycastHit> _raycastHitsThisFrame = new List<RaycastHit>( 2 );


#region MASKS
	[Header("MASKS SETTINGS")]
	/// <summary>
	/// mask with all layers that the player should interact with
	/// </summary>
	public LayerMask platformMask = 0;
	/// <summary>
	/// mask with all layers that trigger events should fire when intersected
	/// </summary>
	public LayerMask triggerMask = 0;
	/// <summary>
	/// mask with all layers that should act as one-way platforms. Note that one-way platforms should always be EdgeCollider2Ds. This is private because it does not support being
	/// updated anytime outside of the inspector for now.
	/// </summary>
	[SerializeField]
	private LayerMask oneWayPlatformMask = 0;
#endregion

#region SLOPE SETTINGS
	[Header("SLOPE SETTINGS")]
	/// <summary>
	/// the max slope angle that the CC2D can climb
	/// </summary>
	/// <value>The slope limit.</value>
	[Range( 0, 90f )]
	public float slopeLimit = 30.0f;

	/// <summary>
	/// this is used to calculate the downward ray that is cast to check for slopes. We use the somewhat arbitrary value 75 degrees
	/// to calculate the length of the ray that checks for slopes.
	/// </summary>
	private float _slopeLimitTangent = Mathf.Tan( 75.0f * Mathf.Deg2Rad );

	/// <summary>
	/// curve for multiplying speed based on slope (negative = down slope and positive = up slope)
	/// </summary>
	public AnimationCurve slopeSpeedMultiplier = new AnimationCurve( new Keyframe( -90, 1.5f ), new Keyframe( 0, 1 ), new Keyframe( 90, 0 ) );

	// we use this flag to mark the case where we are travelling up a slope and we modified our delta.y to allow the climb to occur.
	// the reason is so that if we reach the end of the slope we can make an adjustment to stay grounded
	private bool _isGoingUpSlope = false;

#endregion

#region COLLISION SETTINGS
	[Header("COLLISION SETTINGS")]
	[Range( 2, 20 )]
	public int totalHorizontalRays = 8;
	[Range( 2, 20 )]
	public int totalVerticalRays = 4;

	// horizontal/vertical movement data
	private float _verticalDistanceBetweenRays;
	private float _horizontalDistanceBetweenRays;

#endregion

#region PROPERTIES
	public override bool IsGrounded { 
		get { 
			return collisionState.below; 
		} 
	}

	public override bool IsKinematic {
		get {
			return base.IsKinematic;
		}
		set {
			base.IsKinematic = value;
		}
	}

	public override bool UseGravity {
		get {
			return base.UseGravity;
		}
		set {
			base.UseGravity = value;
		}
	}

	public float SkinWidth
	{
		get { return skinWidth; }
		set
		{
			skinWidth = value;
			RecalculateDistanceBetweenRays();
		}
	}

	public override Vector3 Velocity {
		get {
			return velocity;
		}
	}
#endregion

#region DEBUG METHODS
	[System.Diagnostics.Conditional( "DEBUG_CC2D_RAYS" )]
	private void DrawRay( Vector3 start, Vector3 dir, Color color )
	{
#if UNITY_EDITOR
		Debug.DrawRay( start, dir, color );
#endif
	}
#endregion

#region OVERRIDE
	protected override void Awake ()
	{
		base.Awake ();

		platformMask |= oneWayPlatformMask;

		SkinWidth = skinWidth;

		// We want to set our CC2D to ignore all collision layers except what is in our triggerMask
		for( var i = 0; i < 32; i++ )
		{
			// See if our triggerMask contains this layer and if not ignore it
			if( ( triggerMask.value & 1 << i ) == 0 )
				Physics2D.IgnoreLayerCollision( CachedGameObject.layer, i );
		}
	}

	protected override void Update ()
	{
		base.Update ();

//		if (IsGrounded)
//			velocity.y = 0.0f;

		var smoothedMovementFactor = IsGrounded ? 20.0f : 5.0f; // how fast do we change direction?
		velocity.x = Mathf.Lerp( velocity.x, 0.0f, Time.deltaTime * smoothedMovementFactor );

		if(UseGravity)
			velocity.y += Physics.gravity.y * Time.deltaTime;

		Move( velocity, Time.deltaTime);
	}

	public override void RotateToVelocityDirection (float pSpeed, bool pIgnoreY = true)
	{
		Vector3 direction = velocity;

		if (pIgnoreY)
			direction.y = 0.0f;

		if(direction != Vector3.zero)
			CachedTransform.rotation = Quaternion.Lerp (CachedTransform.rotation,
				Quaternion.LookRotation (direction), Time.deltaTime * pSpeed);
	}

	public override void AddVelocity (Vector3 pVelocity)
	{
		velocity += pVelocity;
	}

	public override void SetVelocity (Vector3 pVelocity)
	{
		velocity = pVelocity;
	}
#endregion

	/// <summary>
	/// this should be called anytime you have to modify the BoxCollider2D at runtime. It will recalculate the distance between the rays used for collision detection.
	/// It is also used in the SkinWidth setter in case it is changed at runtime.
	/// </summary>
	public void RecalculateDistanceBetweenRays()
	{
		// figure out the distance between our rays in both directions
		// horizontal
		var colliderUseableHeight = CachedCollider.size.y * Mathf.Abs( transform.localScale.y ) - ( 2f * skinWidth );
		_verticalDistanceBetweenRays = colliderUseableHeight / ( totalHorizontalRays - 1 );

		// vertical
		var colliderUseableWidth = CachedCollider.size.x * Mathf.Abs( transform.localScale.x ) - ( 2f * skinWidth );
		_horizontalDistanceBetweenRays = colliderUseableWidth / ( totalVerticalRays - 1 );
	}

	/// <summary>
	/// Attempts to move the character to position + deltaMovement. Any colliders in the way will cause the movement to
	/// stop when run into.
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	public override void Move (Vector3 pVelocity, float pSpeed)
	{
		Vector3 deltaMovement = pVelocity * pSpeed;

		// Save off our current grounded state which we will use for wasGroundedLastFrame and becameGroundedThisFrame
		collisionState.wasGroundedLastFrame = collisionState.below;

		collisionState.Reset();
		_raycastHitsThisFrame.Clear();
		_isGoingUpSlope = false;

		var desiredPosition = CachedTransform.position + deltaMovement;
		PrimeRaycastOrigins( desiredPosition, deltaMovement );

		// First, we check for a slope below us before moving.
		// Only check slopes if we are going down and grounded
		if (deltaMovement.y < 0 && collisionState.wasGroundedLastFrame) 
			HandleVerticalSlope (ref deltaMovement);

		// now we check movement in the horizontal dir
		if( Math.Abs(deltaMovement.x) > float.Epsilon )
			MoveHorizontally( ref deltaMovement );

		// next, check movement in the vertical dir
		if( Math.Abs(deltaMovement.y) > float.Epsilon )
			MoveVertically( ref deltaMovement );

		CachedTransform.Translate( deltaMovement, Space.World );

			// only calculate velocity if we have a non-zero deltaTime
		if( Time.deltaTime > 0 )
			velocity = deltaMovement / Time.deltaTime;

		// set our becameGrounded state based on the previous and current collision state
		if( !collisionState.wasGroundedLastFrame && collisionState.below )
			collisionState.becameGroundedThisFrame = true;

		// if we are going up a slope we artificially set a y velocity so we need to zero it out here
		if( _isGoingUpSlope )
			velocity.y = 0;

		// send off the collision events if we have a listener
		if( onControllerCollidedEvent != null )
		{
			for( var i = 0; i < _raycastHitsThisFrame.Count; i++ )
				onControllerCollidedEvent( _raycastHitsThisFrame[i] );
		}
	}
		
	/// <summary>
	/// Resets the raycastOrigins to the current extents of the box collider inset by the SkinWidth. It is inset
	/// to avoid casting a ray from a position directly touching another collider which results in wonky normal data.
	/// </summary>
	/// <param name="futurePosition">Future position.</param>
	/// <param name="deltaMovement">Delta movement.</param>
	private void PrimeRaycastOrigins( Vector3 futurePosition, Vector3 deltaMovement )
	{
		// our raycasts need to be fired from the bounds inset by the SkinWidth
		var modifiedBounds = CachedCollider.bounds;
		modifiedBounds.Expand( -2.0f * skinWidth );

		//Z
		_raycastOrigins.topLeft = new Vector3( modifiedBounds.min.x, modifiedBounds.max.y, CachedTransform.position.z );
		_raycastOrigins.bottomRight = new Vector3( modifiedBounds.max.x, modifiedBounds.min.y, CachedTransform.position.z );
		_raycastOrigins.bottomLeft = modifiedBounds.min;
	}

	/// <summary>
	/// Checks the center point under the BoxCollider2D for a slope. If it finds one then the deltaMovement is adjusted so that
	/// the player stays grounded and the slopeSpeedModifier is taken into account to speed up movement.
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	private void HandleVerticalSlope( ref Vector3 deltaMovement )
	{
		// slope check from the center of our collider
		var centerOfCollider = ( _raycastOrigins.bottomLeft.x + _raycastOrigins.bottomRight.x ) * 0.5f;
		var rayDirection = -Vector3.up;

		// the ray distance is based on our slopeLimit
		var slopeCheckRayDistance = _slopeLimitTangent * ( _raycastOrigins.bottomRight.x - centerOfCollider );

		//Z
		var slopeRay = new Vector3( centerOfCollider, _raycastOrigins.bottomLeft.y, CachedTransform.position.z );

		DrawRay( slopeRay, rayDirection * slopeCheckRayDistance, Color.yellow );

		//_raycastHit = Physics2D.Raycast( slopeRay, rayDirection, slopeCheckRayDistance, platformMask );

		if(Physics.Raycast(slopeRay, rayDirection, out _raycastHit, slopeCheckRayDistance, platformMask))
		{
			// Bail out if we have no slope
			var angle = Vector3.Angle( _raycastHit.normal, Vector3.up );

			if( angle == 0.0f)
				return;

			// We are moving down the slope if our normal and movement direction are in the same x direction
			bool isMovingDownSlope = Mathf.Sign( _raycastHit.normal.x ) == Mathf.Sign( deltaMovement.x );

			if( isMovingDownSlope )
			{
				// Going down we want to speed up in most cases so the slopeSpeedMultiplier curve should be > 1 for negative angles
				var slopeModifier = slopeSpeedMultiplier.Evaluate( -angle );
				deltaMovement.y = _raycastHit.point.y - slopeRay.y - SkinWidth;
				deltaMovement.x *= slopeModifier;
				collisionState.movingDownSlope = true;
				collisionState.slopeAngle = angle;
			}
		}
	}

	/// <summary>
	/// handles adjusting deltaMovement if we are going up a slope.
	/// </summary>
	/// <returns><c>true</c>, if horizontal slope was handled, <c>false</c> otherwise.</returns>
	/// <param name="deltaMovement">Delta movement.</param>
	/// <param name="angle">Angle.</param>
	private bool HandleHorizontalSlope( ref Vector3 deltaMovement, float angle )
	{
		// disregard 90 degree angles (walls)
		if( Mathf.RoundToInt( angle ) == 90 )
			return false;

		// if we can walk on slopes and our angle is small enough we need to move up
		if( angle < slopeLimit )
		{
			// we only need to adjust the deltaMovement if we are not jumping
			// TODO: this uses a magic number which isn't ideal!
			if( deltaMovement.y < jumpingThreshold )
			{
				// apply the slopeModifier to slow our movement up the slope
				var slopeModifier = slopeSpeedMultiplier.Evaluate( angle );
				deltaMovement.x *= slopeModifier;

				// we dont set collisions on the sides for this since a slope is not technically a side collision

				// smooth y movement when we climb. we make the y movement equivalent to the actual y location that corresponds
				// to our new x location using our good friend Pythagoras
				deltaMovement.y = Mathf.Abs( Mathf.Tan( angle * Mathf.Deg2Rad ) * deltaMovement.x );
				_isGoingUpSlope = true;

				collisionState.below = true;
			}
		}
		else // too steep. get out of here
		{
			deltaMovement.x = 0;
		}

		return true;
	}

	/// <summary>
	/// Ee have to use a bit of trickery in this one. The rays must be cast from a small distance inside of our
	/// collider (SkinWidth) to avoid zero distance rays which will get the wrong normal. Because of this small offset
	/// we have to increase the ray distance SkinWidth then remember to remove SkinWidth from deltaMovement before
	/// actually moving the player
	/// </summary>
	private void MoveHorizontally( ref Vector3 deltaMovement )
	{
		bool isGoingRight = deltaMovement.x > 0.0f;
		float rayDistance = Mathf.Abs( deltaMovement.x ) + skinWidth;
		Vector3 rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
		Vector3 initialRayOrigin = isGoingRight ? _raycastOrigins.bottomRight : _raycastOrigins.bottomLeft;

		for( var i = 0; i < totalHorizontalRays; i++ )
		{
			Vector3 ray = new Vector3( initialRayOrigin.x, initialRayOrigin.y + i * _verticalDistanceBetweenRays, CachedTransform.position.z );

			DrawRay( ray, rayDirection * rayDistance, Color.red );

			// If we are grounded we will include oneWayPlatforms only on the first ray (the bottom one). this will allow us to
			// walk up sloped oneWayPlatforms
			bool hitGround = false;

			if (i == 0 && collisionState.wasGroundedLastFrame)
				hitGround = Physics.Raycast (ray, rayDirection, out _raycastHit, rayDistance, platformMask);
			else
				hitGround = Physics.Raycast (ray, rayDirection, out _raycastHit, rayDistance, platformMask & ~oneWayPlatformMask);

			if( hitGround )
			{
				// The bottom ray can hit slopes but no other ray can so we have special handling for those cases
				if( i == 0 && HandleHorizontalSlope( ref deltaMovement, Vector2.Angle( _raycastHit.normal, Vector3.up ) ) )
				{
					_raycastHitsThisFrame.Add( _raycastHit );
					break;
				}

				// Set our new deltaMovement and recalculate the rayDistance taking it into account
				deltaMovement.x = _raycastHit.point.x - ray.x;
				rayDistance = Mathf.Abs( deltaMovement.x );

				// Remember to remove the SkinWidth from our deltaMovement
				if( isGoingRight )
				{
					deltaMovement.x -= skinWidth;
					collisionState.right = true;
				}
				else
				{
					deltaMovement.x += skinWidth;
					collisionState.left = true;
				}

				_raycastHitsThisFrame.Add( _raycastHit );

				// We add a small fudge factor for the float operations here. if our rayDistance is smaller
				// than the width + fudge bail out because we have a direct impact
				if( rayDistance < skinWidth + kSkinWidthFloatFudgeFactor )
					break;
			}
		}
	}

	private void MoveVertically( ref Vector3 deltaMovement )
	{
		var isGoingUp = deltaMovement.y > 0.0f;
		var rayDistance = Mathf.Abs( deltaMovement.y ) + skinWidth * 5.0f;
		var rayDirection = isGoingUp ? Vector3.up : -Vector3.up;
		var initialRayOrigin = isGoingUp ? _raycastOrigins.topLeft : _raycastOrigins.bottomLeft;

		// Apply our horizontal deltaMovement here so that we do our raycast from the actual position we would be in if we had moved
		initialRayOrigin.x += deltaMovement.x;

		// If we are moving up, we should ignore the layers in oneWayPlatformMask
		var mask = platformMask;

		if( isGoingUp && !collisionState.wasGroundedLastFrame )
			mask &= ~oneWayPlatformMask;

		for( var i = 0; i < totalVerticalRays; i++ )
		{
			Vector3 ray = new Vector3( initialRayOrigin.x + i * _horizontalDistanceBetweenRays, initialRayOrigin.y, CachedTransform.position.z );

			DrawRay( ray, rayDirection * rayDistance, Color.red );

			if(Physics.Raycast(ray, rayDirection, out _raycastHit, rayDistance, mask))
			{
				// Set our new deltaMovement and recalculate the rayDistance taking it into account
				deltaMovement.y = _raycastHit.point.y - ray.y;
				rayDistance = Mathf.Abs( deltaMovement.y );

				// Remember to remove the SkinWidth from our deltaMovement
				if( isGoingUp )
				{
					deltaMovement.y -= skinWidth;
					collisionState.above = true;
				}
				else
				{
					deltaMovement.y += skinWidth;
					collisionState.below = true;
				}

				_raycastHitsThisFrame.Add( _raycastHit );

				// This is a hack to deal with the top of slopes. if we walk up a slope and reach the apex we can get in a situation
				// where our ray gets a hit that is less then SkinWidth causing us to be ungrounded the next frame due to residual velocity.
				if( !isGoingUp && deltaMovement.y > 0.00001f )
					_isGoingUpSlope = true;

				// We add a small fudge factor for the float operations here. if our rayDistance is smaller
				// than the width + fudge bail out because we have a direct impact
				if( rayDistance < skinWidth + kSkinWidthFloatFudgeFactor )
					return;
			}
		}
	}

	/// <summary>
	/// Moves directly down until grounded
	/// </summary>
	public void WarpToGrounded()
	{
		do
		{
			Move( -Vector3.up, 1.0f);
		} while( !IsGrounded );
	}
}
