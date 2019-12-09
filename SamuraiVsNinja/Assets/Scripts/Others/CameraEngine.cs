using System.Collections.Generic;
using UnityEngine;

public class CameraEngine : Singelton<CameraEngine>
{
    #region VARIABLES

    [Space]
    [Header("ZOOM VARIABLES")]
    public float SmoothTime = 0.5f;
    public float MaxZoom = 20f;
    public float MinZoom = 80f;
    private readonly float zoomLimiter = 50f;
    public Vector2 Offset = Vector2.zero;
    public Vector2 CameraVelocity;

    [Space]
    [Header("SHAKE VARIABLES")]
    public float power = 0.8f;
    public float duration = 1f;
    public float slowDownAmount = 1f;

    [Space]
    [Header("DEBUG VARIABLES")]
    public bool ShowDebugGizmos;

    private List<Transform> targets = new List<Transform>();
    private GameObject levelBackgroundImageGameObject;
    private Vector2 startPosition = Vector2.zero;
    private float initialDuration;

    #endregion VARIABLES

    public bool IsShaking
    {
        get;
        set;
    } = false;
    public float OrthographicSize
    {
        get
        {
            return MainCamera.orthographicSize;
        }
    }
    public Camera MainCamera
    {
        get;
        private set;
    }

    public Vector2 ScreenToWorldPoint(Vector2 position)
    {
        return MainCamera.ScreenToWorldPoint(position);
    }

    private void Awake()
    {
        MainCamera = GetComponent<Camera>();
        var temp = transform.Find("LevelBackgroundImageGameObject");

        if(temp)
        {
            levelBackgroundImageGameObject = temp.gameObject;
        }
    }

    private void Start()
    {
        startPosition = transform.localPosition;
        initialDuration = duration;    
    }

    private void Update()
    {
        if (IsShaking)
        {
            if (duration > 0)
            {
                transform.localPosition = (Vector2)transform.position + Random.insideUnitCircle * power;
                duration -= Time.unscaledDeltaTime * slowDownAmount;
            }
            else
            {
                IsShaking = false;
                duration = initialDuration;
            }
        }
    }

    private void LateUpdate()
    {
        if (targets == null || targets.Count == 0)
            return;

        CameraMovement();
        CameraZoom();
    }

    private void OnDrawGizmos()
    {
        if(ShowDebugGizmos == false)
        {
            return;
        }

        Gizmos.DrawCube(debugBounds.center, debugBounds.extents * 2);
    }

    private Vector2 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        return EncapsulateTargetBounds().center;
    }

    private Bounds debugBounds;

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
        var foo = EncapsulateTargetBounds();

        if(ShowDebugGizmos)
        {
            debugBounds = foo;
        }

        return foo.size.x;
    }
    private void CameraMovement()
    {
        Vector2 centerPoint = GetCenterPoint();
        Vector2 newPosition = centerPoint + Offset;
        transform.position = Vector2.SmoothDamp(transform.position, newPosition, ref CameraVelocity, SmoothTime);
    }
    private void CameraZoom()
    {
        var newZoom = Mathf.Lerp(MaxZoom, MinZoom, GetGreatestTargetDistance() / zoomLimiter);
        MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, newZoom, Time.deltaTime);
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
    public void ManageLevelBackground(bool setActive)
    {
        if(levelBackgroundImageGameObject == null)
        {
            Debug.LogError("No level background object...");
            return;
        }

        levelBackgroundImageGameObject.SetActive(setActive);
    }
}
