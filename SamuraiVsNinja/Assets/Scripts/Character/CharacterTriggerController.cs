using UnityEngine;

public class CharacterTriggerController : MonoBehaviour
{
    private Character owner;
    private Vector2 knockbackForce = new Vector2(10, 20);

    private void Awake()
    {
        owner = GetComponentInParent<Character>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner.CurrentState == CHARACTER_STATE.NORMAL)
        {
            var hitDirection = collision.transform.position - transform.position;
            hitDirection = hitDirection.normalized;

            switch (collision.tag)
            {
                case "Onigiri":
                    //EventManager.Instance.PostEvent("Pickup");
                    Debug.LogError("Play Pickup sound here!");
                    owner.AddOnigiri(1);
                    ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 5), collision.transform.position);
                    ObjectPoolManager.Instance.DespawnObject(collision.gameObject);
                    break;

                case "Character":
                    if (owner.CharacterEngine.IsDashing)
                    {
                        var hittedPlayer = collision.gameObject.GetComponentInParent<Character>();
                        hittedPlayer.TakeDamage(owner, hitDirection, new Vector2(-40, 15), 0);
                    }
                    break;

                case "Killzone":
                    owner.TakeDamage(owner,hitDirection, Vector2.zero, 3);
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (owner.CurrentState == CHARACTER_STATE.NORMAL)
        {
            switch (collision.tag)
            {            
                case "Spike":

                    var hitDirection = collision.transform.position - transform.position;
                    hitDirection = hitDirection.normalized;
                    owner.TakeDamage(owner, hitDirection, knockbackForce, 1);

                    break;
            }
        }
    }
}
