using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))	
		{
			Application.LoadLevel ("Demo_Santo_Cave");
		}
	}	
}
