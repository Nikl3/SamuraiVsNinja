using UnityEngine;

public class Sushi : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sushiSprites;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = sushiSprites[Random.Range(0, sushiSprites.Length)];
    }
}
