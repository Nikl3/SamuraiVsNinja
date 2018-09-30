using System.Collections.Generic;
using UnityEngine;

public class CameraEngine : Singelton<CameraEngine>
{
    #region VARIABLES

    [SerializeField]
    private List<Transform> targets;

    [SerializeField]
    private float smoothTime = 0.5f;
    [SerializeField]
    private float maxZoom = 20f;
    [SerializeField]
    private float minZoom = 30f;
    [SerializeField]
    private float zoomLimiter = 100f;

    private Vector2 offset = Vector2.zero;
    private Vector2 cameraVelocity;
    private Camera mainCamera;

    #endregion VARIABLES

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (targets == null || targets.Count == 0)
            return;

        CameraMovement();
        CameraZoom();
    }

    private Vector2 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        return EncapsulateTargetBounds().center;
    }
    private Bounds EncapsulateTargetBounds()
    {
        var bounds = new Bounds(targets[0].position, Vector2.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds;
    }
    private float GetGreatestTargetDistance()
    {
        return EncapsulateTargetBounds().size.x;
    }
    private void CameraMovement()
    {
        Vector2 centerPoint = GetCenterPoint();
        Vector2 newPosition = centerPoint + offset;
        transform.position = Vector2.SmoothDamp(transform.position, newPosition, ref cameraVelocity, smoothTime);
    }
    private void CameraZoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestTargetDistance() / zoomLimiter);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newZoom, Time.deltaTime);
    }

    public void AddTarget(Transform newTarget)
    {
        targets.Add(newTarget);
    }
    public void ClearTargets()
    {
        if (targets != null && targets.Count > 0)
        {
            targets.Clear();
        }
    }
}
