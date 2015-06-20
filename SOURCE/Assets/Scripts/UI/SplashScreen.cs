using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour 
{
	public Image redStompLogo;
	public Image gameLogo;

	void Start()
	{
		StartCoroutine (RedStompLogo ());
	}

	private IEnumerator RedStompLogo()
	{
		Color temp = redStompLogo.color;
		float alpha = temp.a;

		while(alpha < 1)
		{
			alpha += Time.deltaTime;
			redStompLogo.color = new Color (temp.r, temp.g, temp.b, alpha);
			yield return null;
		}

		yield return new WaitForSeconds (1.5f);

		while(alpha > 0)
		{
			alpha -= Time.deltaTime;
			redStompLogo.color = new Color (temp.r, temp.g, temp.b, alpha);
			yield return null;
		}

		temp = gameLogo.color;
		alpha = temp.a;

		while(alpha < 1)
		{
			alpha += Time.deltaTime;
			gameLogo.color = new Color (temp.r, temp.g, temp.b, alpha);
			yield return null;
		}

		yield return new WaitForSeconds (1.5f);

		while(alpha > 0)
		{
			alpha -= Time.deltaTime;
			gameLogo.color = new Color (temp.r, temp.g, temp.b, alpha);
			yield return null;
		}

		Application.LoadLevel ("Main Menu");
	}
}
