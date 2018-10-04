using Fabric;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {
	public GameObject go;
	public string audioComp;
	public string randomComp;
	
	public string iloPlay;
	public string iloStop;

	public string seqPlay;
	public string seqStop;
	public string seqAdvance;
	
	public string swiPlay;
	public string swiAdvance1;
	public string swiAdvance2;
	public string swiStop;

	void Update () {

		if(Input.GetKeyDown(KeyCode.I))
		{
			EventManager.Instance.PostEvent(swiPlay);
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
			EventManager.Instance.PostEvent(swiAdvance1);
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			EventManager.Instance.PostEvent(swiAdvance2);
		}
		if(Input.GetKeyDown(KeyCode.L))
		{
			EventManager.Instance.PostEvent(swiStop);
		}

		if(Input.GetKeyDown(KeyCode.T))
		{
			EventManager.Instance.PostEvent(seqPlay);
		}
		if(Input.GetKeyDown(KeyCode.Y))
		{
			EventManager.Instance.PostEvent(seqStop);
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			EventManager.Instance.PostEvent(seqAdvance);
		}







		if(Input.GetKeyDown(KeyCode.E))
		{
			EventManager.Instance.PostEvent(iloPlay);
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			EventManager.Instance.PostEvent(iloStop);
		}

		if(Input.GetKeyDown(KeyCode.Q))
		{
			EventManager.Instance.PostEvent(audioComp, go);
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
			EventManager.Instance.PostEvent(randomComp);
		}
	}
}
