using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public Transform player2;
    Vector3 offset;
    public Vector2 cameraMargins;
    Camera cam;

    void UpdateCam() {
        var p1 = player.position;
        var p2 = player2.position;
        var boxMin = Vector2.Min(p1, p2);
        var boxMax = Vector2.Max(p1, p2);
        boxMin -= cameraMargins;
        boxMax += cameraMargins;
        var boxDim = boxMax - boxMin;

        // clamp..?
        boxDim.y = Mathf.Max(boxDim.y, 11f); // min cam height
        cam.orthographicSize = boxDim.y;
        transform.position = (p1 + p2) /2f - Vector3.forward;    

    }
    void Start () {
        //offset = transform.position - player.position;
        cam = FindObjectOfType<Camera>();
	}
	
	void LateUpdate () {
        UpdateCam();
	}
}
