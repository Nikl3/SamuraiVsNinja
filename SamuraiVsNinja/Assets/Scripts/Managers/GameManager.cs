using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    
    public int P1CoinCount = 0;
    public TextMeshProUGUI coinsText;
    public GameObject p1;
    public GameObject WinScreen;



	void Start () {
		
	}
	
	void Update () {
		if (P1CoinCount >= 10f) {
            print("p1 wins");
            WinScreen.SetActive(true);
            Time.timeScale = 0f;
        }
	}
}
