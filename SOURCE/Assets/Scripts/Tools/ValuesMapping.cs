using System.Collections;

public class ValuesMapping
{
	// Maps a value from ome arbitrary range to another arbitrary range
	public static float Map( float pValue, float pLeftMin, float pLeftMax, float pRightMin, float pRightMax )
	{
		return pRightMin + ( pValue - pLeftMin ) * ( pRightMax - pRightMin ) / ( pLeftMax - pLeftMin );
	}
}
