using System.Collections.Generic;
using UnityEngine;

public class CameraEngine : Singelton<CameraEngine>
{
    #region VARIABLES
    [Header("ZOOM VARIABLES")]
    [SerializeField]
    private float smoothTime = 0.5f;
    [SerializeField]
    private float maxZoom = 20f;
    [SerializeField]
    private float minZoom = 80f;
    [SerializeField]
    private readonly float zoomLimiter = 50f;
    [SerializeField]
    private Vector2 offset = Vector2.zero;
    [SerializeField]
    private Vector2 cameraVelocity;

    [Header("SHAKE VARIABLES")]
    [SerializeField]
    private float power = 0.8f;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private float slowDownAmount = 1f;
    
    [SerializeField]
    private List<Transform> targets;
    private GameObject levelBackgroundImageGameObject;
    private Vector2 startPosition = Vector2.zero;
    private float initialDuration;
    private bool isShaking = false;

    #endregion VARIABLES

    public bool IsShaking
    {
        get
        {
            return isShaking;
        }
        set
        {
            isShaking = value;
        }
    }
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
        levelBackgroundImageGameObject = transform.Find("LevelBackgroundImageGameObject").gameObject;
    }

    private void Start()
    {
        startPosition = transform.localPosition;
        initialDuration = duration;    
    }

    private void Update()
    {
        if (isShaking)
        {
            if (duration > 0)
            {
                transform.localPosition = (Vector2)transform.position + Random.insideUnitCircle * power;
                duration -= Time.unscaledDeltaTime * slowDownAmount;
            }
            else
            {
                isShaking = false;
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
        levelBackgroundImageGameObject.SetActive(setActive);
    }
}
