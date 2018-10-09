using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    void SceneSwitch() {
        SceneManager.LoadScene(1);
    }

	void Start () {
        Invoke("SceneSwitch", 37f);
	}
	
}
