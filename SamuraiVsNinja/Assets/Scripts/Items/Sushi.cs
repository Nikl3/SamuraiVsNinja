using UnityEngine;

public class Sushi : Item
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sushiSprites;
    private float startSpeed = 20f;
    private float dropSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sushiSprites[Random.Range(0, sushiSprites.Length)];
        dropSpeed = startSpeed;
    }

    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector2.down * dropSpeed * Time.unscaledDeltaTime);
        dropSpeed += 0.5f;

        var foo = CameraEngine.Instance.MainCamera.WorldToScreenPoint(transform.position);
        if (foo.y >= 0)
        {
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 6), transform.position);
            ObjectPoolManager.Instance.DespawnObject(gameObject);
        }
    }
}
