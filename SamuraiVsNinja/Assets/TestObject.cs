using UnityEngine;

public class TestObject : MonoBehaviour
{
    private readonly float lifeTime = 10f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        InvokeRepeating("ChangeColor", 0.5f, 0.1f);
    }

    private void OnEnable()
    {
        Invoke("LifeTime", lifeTime);

        ObjectPoolManager.Instance.TestObjects.Add(gameObject);
    }

    private void OnDisable()
    {
        ObjectPoolManager.Instance.TestObjects.Remove(gameObject);
        ObjectPoolManager.Instance.DespawnObject(gameObject);
    }

    private void LifeTime()
    {
        gameObject.SetActive(false);
    }

    private void ChangeColor()
    {
        spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
