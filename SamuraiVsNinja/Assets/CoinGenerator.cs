using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour {

    public Transform[] Spawnpoints;
    public GameObject coinPrefab;
    public float SpawnIntervall = 0;

	void Start () {
    
	}
	
	void Update () {
        SpawnIntervall += Time.deltaTime;

        if (SpawnIntervall > 4f) {

            Debug.Log("!!!!");
            int randomPosition = Random.Range(0, Spawnpoints.Length);
            Vector2 foo = Spawnpoints[randomPosition].position;

            GameObject go = Instantiate(coinPrefab, foo, Quaternion.identity);
            SpawnIntervall = 0;
        }


	}
}

//        if (Time.time > resourceSpawn + timeSinceLastResource && resourceSpawnOnOff) {
//            GameObject go = Instantiate(resourcePrefab, transform.position +
//            new Vector3(Random.Range(-3f, 3f), Random.Range(2f, -2f), 0), transform.rotation);
//go.transform.parent = spawnFolder;
//            timeSinceLastResource = Time.time;
//        }