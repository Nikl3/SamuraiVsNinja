using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    
    public int P1CoinCount = 0;
    public TextMeshProUGUI coinsText;
    public GameObject p1;
    public List<GameObject> PlayersCount = new List<GameObject>();
    public GameObject WinScreen;




	void Start () {
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("LocalPlayer")) {
            PlayersCount.Add(player);
        }
	}
	
	void Update () {
		if (P1CoinCount >= 10f) {
            print("p1 wins");
            WinScreen.SetActive(true);
            Time.timeScale = 0f;
        }
	}
}
