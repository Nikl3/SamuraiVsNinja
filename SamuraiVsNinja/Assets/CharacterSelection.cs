using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour {

    List<GameObject> models;
    int selectionIndex = 0;

	void Start () {
        models = new List<GameObject>();
        foreach (Transform t in transform) {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
            
        }
        models[selectionIndex].SetActive(true);
    }

    public void Select(int index) {
        if (index == selectionIndex) {
            return;
        }
        if (index < 0 || index >= models.Count) {
            return;
        }
        models[selectionIndex].SetActive(false);
        selectionIndex = index;
        models[selectionIndex].SetActive(true);
    }
    
}
