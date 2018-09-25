public class Shuriken : Projectile {

    private Shuriken() {
        projectileSpeed = 60f;
    }

    protected override void Awake() {
        base.Awake();

        gameObject.name = "Shuriken";
    }
}
