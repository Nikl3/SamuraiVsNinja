using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestroy : MonoBehaviour {

    public float destroyTime;
	
	void Update () { 
            Destroy(gameObject, destroyTime);
	}
}
