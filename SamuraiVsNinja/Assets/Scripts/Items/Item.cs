using UnityEngine;

public class Item : MonoBehaviour
{
    protected Animator animator;
    protected bool isFloating;
    protected string itemTag;
    protected Transform graphics;

    protected Rigidbody2D rb2d;
    protected readonly float lifeTime = 5f;
    protected float currentLifetime;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        itemTag = tag;
        graphics = transform.Find("Graphics");
    }

    private void OnEnable()
    {
        currentLifetime = lifeTime;
        isFloating = false;
        tag = "Untagged";
    }

    protected virtual void Update()
    {
        if (GameMaster.Instance.IsLoadingScene)
        {
            ObjectPoolManager.Instance.DespawnObject(gameObject);
            return;
        }   
    }

    #region OLD
    /*
    private readonly float degreesPerSecond = 15.0f;
    private readonly float amplitude = 0.5f;
    private readonly float frequency = 1f;

    // Position Storage Variables
    private Vector3 posOffset = new Vector3();
    private Vector3 tempPosition = new Vector3();

    private void Start()
    {
        posOffset = transform.position;
    }

    private void Update()
    {
        FloatItem();
    }

    private void FloatItem()
    {
        // Movement object on Y-Axis.
        transform.Translate(new Vector2(Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down.
        tempPosition = posOffset;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPosition;
    }
    */
    #endregion OLD
}