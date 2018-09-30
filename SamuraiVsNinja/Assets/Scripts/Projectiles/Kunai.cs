public class Kunai : Projectile
{ 
	private Kunai()
	{
		projectileSpeed = 60f;
	}

	protected override void Awake()
	{
		base.Awake();

		gameObject.name = "Kunai";
	}
}
