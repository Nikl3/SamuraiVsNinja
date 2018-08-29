using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAmmo : MonoBehaviour {

    public float ammospeed;
    public float ammoDuration;
    int startDir;

    void SelfDestroy() {
        Destroy(gameObject);
    }

    public void BulletMove(int ammoDir) {
        startDir = ammoDir;
        Invoke("SelfDestroy", 5f);
    }

    void Update () {
        transform.position += new Vector3 (startDir, 0, 0) * ammospeed * Time.deltaTime;
    }
}
