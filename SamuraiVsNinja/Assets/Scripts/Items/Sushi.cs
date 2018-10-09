using UnityEngine;

public class Sushi : Item
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sushiSprites;
    private readonly float dropSpeed = 20f;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sushiSprites[Random.Range(0, sushiSprites.Length)];
    }

    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector2.down * dropSpeed * Time.unscaledDeltaTime);
        if(transform.position.y <= -70)
        {
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 6), transform.position);
            ObjectPoolManager.Instance.DespawnObject(gameObject);
        }
    }
}
