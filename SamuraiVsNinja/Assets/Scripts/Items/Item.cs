using UnityEngine;

public class Item : MonoBehaviour
{
    private Animator animator;
    private bool isFloating;
    private string itemTag;
    private Transform graphics;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        itemTag = tag;
        graphics = transform.Find("Graphics");
    }

    private void OnEnable()
    {
        isFloating = false;
        tag = "Untagged";
    }

    private void OnDisable()
    {       
        tag = itemTag; 
        transform.position = Vector2.one;
        graphics.position = Vector2.one;
    }

    private void Update()
    {
        if (GameMaster.Instance.IsLoadingScene)
        {
            ObjectPoolManager.Instance.DespawnObject(gameObject);
            return;
        }

        if (!isFloating)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Spawning"))
            {
                isFloating = true;
                tag = itemTag;
            }
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