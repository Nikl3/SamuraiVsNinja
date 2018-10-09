using UnityEngine;

public class Onigiri : Item
{
    public void Foo(Vector2 force)
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }
}
