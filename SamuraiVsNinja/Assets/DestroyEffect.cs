using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour {

    public float destroyTime;
	
	void Update () {
        Destroy(gameObject, destroyTime);
	}
}
