using UnityEngine;

public class Shuriken : Projectile
{
    private Shuriken()
    {
        projectileSpeed = 60f;
    }

    protected override void Awake()
    {
        base.Awake();

        gameObject.name = "Shuriken";
    }

    protected override void Update()
    {
        base.Update();
        transform.Rotate(new Vector3(0, 0, -startDirection * 10)); 
    }
}
