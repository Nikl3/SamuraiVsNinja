using Fabric;
using System.Collections;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    public bool UsingLerpMethod1 = true;

    #region VARIABLES

    private Player owner;

    #region JUMP
    [Header("JUMP")]

    [SerializeField]
    private float minJumpHeight = 1f;
    [SerializeField]
    private float maxJumpHeight = 4f;
    [SerializeField]
    private readonly float timeToJumpApex = 0.5f;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    #endregion JUMP

    #region MOVEMENT

    [Header("MOVEMENT")]
    [SerializeField]
    private float moveSpeed = 6f;
    private float startSpeed;
    private readonly float accelerationTimeAirbourne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;
    private float gravity;
    private float dashGravity;
    private Vector2 velocity;
    private float velocityXSmoothing;
    private Vector2 directionalInput;

    #endregion MOVEMENT

    #region WALL_SLIDE

    [Header("WALL SLIDE")]

    [SerializeField]
    private readonly float maxWallSlideSpeed = 3f;
    [SerializeField]
    private Vector2 wallJumpClimb = new Vector2(2.5f, 16);
    [SerializeField]
    private Vector2 wallJumpOff = new Vector2(8.5f, 7);
    [SerializeField]
    private Vector2 wallLeap = new Vector2(18, 17);
    private bool wallSliding;
    private int wallDirectionX;

    #endregion WALL_SLIDE

    #region DASH

    [Header("DASH")]
    [SerializeField]
    private float dashSpeed = 20;
    private bool isDashing = false;
    private readonly float dashCooldown = 2f;
    private readonly float dashTime = 0.3f;

    #endregion DASH

    #region THROW

    [Header("THROW")]
    [SerializeField]
    private bool isThrowing = false;
    [SerializeField]
    private readonly float throwAttackCooldown = 2f;

    #endregion THROW

    private readonly float respawnCooldown = 2f;

    private Coroutine throwAttackCoroutine;
    private Coroutine dashCoroutine;
    private Coroutine knockbackCoroutine;
    private Coroutine invincibilityCoroutine;
    private Coroutine respawnCoroutine;

    #endregion VARIABLES

    #region PROPERTIES

    public bool IsAttacking
    {
        get;
        set;
    }
    public bool IsDashing
    {
        get
        {
            return isDashing;
        }
    }
    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }

        set
        {
            dashSpeed = value;
        }
    }
    public float DashCooldown
    {
        get
        {
            return dashCooldown;
        }
    }
    public float ThrowAttackCooldown
    {
        get
        {
            return throwAttackCooldown;
        }
    }
    public float RespawnCooldown
    {
        get
        {
            return respawnCooldown;
        }
    }
    public float MinJumpHeight
    {
        get
        {
            return minJumpHeight;
        }

        set
        {
            minJumpHeight = value;
        }
    }
    public float MaxJumpHeight
    {
        get
        {
            return maxJumpHeight;
        }

        set
        {
            maxJumpHeight = value;
        }
    }
    public float AccelerationTimeGrounded
    {
        get
        {
            return accelerationTimeGrounded;
        }

        set
        {
            accelerationTimeGrounded = value;
        }
    }

    #endregion PROPERTIES

    private void Awake()
    {
        owner = GetComponent<Player>();
    }
    private void Start()
    {
        startSpeed = moveSpeed;
        gravity = dashGravity = -(2 * MaxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * MinJumpHeight);       
    }

    private void HandleWallSliding()
    {
        wallDirectionX = (owner.Controller2D.Collisions.Left) ? -1 : 1;

        wallSliding = false;
        owner.AnimatorController.AnimatorSetBool("IsWallsliding", false);

        if ((owner.Controller2D.Collisions.Left || owner.Controller2D.Collisions.Right) && !owner.Controller2D.Collisions.Below && velocity.y < 0)
        {
            owner.AnimatorController.AnimatorSetBool("IsWallsliding", true);
            owner.AnimatorController.AnimatorSetBool("IsAttacking", false);
            wallSliding = true;

            if (!InputManager.Instance.X_ButtonUp(owner.PlayerData.ID))
            {
                return;
            }

            if (velocity.y < -maxWallSlideSpeed)
            {
                velocity.y = -maxWallSlideSpeed;
            }
        }
    }
    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (owner.Controller2D.Collisions.Below) ? AccelerationTimeGrounded : accelerationTimeAirbourne);
        velocity.y += gravity * Time.deltaTime;
    }
    private void CheckifRunningAndLocalScale()
    {
        if (directionalInput.x != 0 && !wallSliding)
        {
            owner.AnimatorController.PlayerGraphics.localScale = new Vector2(owner.Controller2D.Collisions.FaceDirection > 0 ? -1 : 1, 1);
            owner.AnimatorController.AnimatorSetBool("IsRunning", owner.Controller2D.Collisions.Below ? true : false);
        }
        else
        {
            owner.AnimatorController.AnimatorSetBool("IsRunning", false);
        }
    }
    private void CheckTopAndBottomCollision()
    {
        if (owner.Controller2D.Collisions.Above || owner.Controller2D.Collisions.Below)
        {
            velocity.y = 0;
            owner.AnimatorController.AnimatorSetBool("IsJumping", false);
            owner.AnimatorController.AnimatorSetBool("IsDropping", false);
        }
        else
        {
            if (!owner.AnimatorController.GetAnimaionState("Attack"))
            {
                owner.AnimatorController.AnimatorSetBool("IsJumping", velocity.y > 0 ? true : false);
                owner.AnimatorController.AnimatorSetBool("IsDropping", velocity.y < 0 ? true : false);
            }
        }
    }

    public void ResetVariables()
    {
        directionalInput = Vector2.zero;
        velocity = Vector2.zero;
    }
    public void CalculateMovement()
    {
        CalculateVelocity();
        HandleWallSliding();
        owner.Controller2D.Move(velocity * Time.deltaTime, directionalInput);

        CheckifRunningAndLocalScale();
        CheckTopAndBottomCollision();
    }
    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
    public void OnJumpInputDown()
    {
        if (owner.Controller2D.Collisions.Below)
        {
            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 4), transform.position);
        }

        if (wallSliding)
        {
            if (wallDirectionX == directionalInput.x)
            {
                velocity.x = -wallDirectionX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirectionX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirectionX * wallLeap.x;
                velocity.y = wallLeap.y;
            }

            ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 4), transform.position);
        }

        if (owner.Controller2D.Collisions.Below)
        {
            velocity.y = maxJumpVelocity;
        }
    }
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }
    public void HandleMeleeAttacks()
    {
        owner.PlayerData.PlayerInfo.Attacks++;
        owner.AnimatorController.AnimatorSetBool("IsAttacking", true);
    }
    public void OnThrow()
    {
        if (throwAttackCoroutine == null &&
            !isThrowing &&
            !wallSliding &&
            !IsDashing)
        {
            throwAttackCoroutine = StartCoroutine(IThrow());
        }
    }
    public void OnDash()
    {
        if (dashCoroutine == null &&
            !isDashing &&
            !wallSliding)
        {
            dashCoroutine = StartCoroutine(IDash());
        }
    }
    public void OnKnockback(Vector2 knockdownDirection, Vector2 knockbackForce)
    {
        if (knockbackCoroutine == null)
        {
            knockbackCoroutine = StartCoroutine(IKnockback(knockdownDirection, knockbackForce));
        }
    }
    public void StartInvincibility(float invincibilityDuartion, float flashSpeed)
    {
        if (invincibilityCoroutine == null)
        {
            invincibilityCoroutine = StartCoroutine(IInvincibility(invincibilityDuartion, flashSpeed));
        }
    }
    public void Respawn(Vector2 spawnPoint, float respawnDelay = 0f)
    {
        if (respawnCoroutine == null)
        {
            respawnCoroutine = StartCoroutine(IRespawn(spawnPoint, respawnDelay));
        }
    }

    #region COROUTINES

    private IEnumerator IThrow()
    {
        owner.PlayerData.PlayerInfo.Attacks++;
        isThrowing = true;
        owner.AnimatorController.AnimatorSetBool("IsThrowing", true);
        owner.PlayerData.PlayerInfo.StartThrowCooldown(isThrowing, ThrowAttackCooldown);

        yield return new WaitForSeconds(ThrowAttackCooldown);
        isThrowing = false;
        throwAttackCoroutine = null;
    }
    private IEnumerator IDash()
    {
        isDashing = true;
        owner.PlayerData.PlayerInfo.StartDashCooldown(DashCooldown);
        owner.AnimatorController.AnimatorSetBool("IsDashing", true);

        //ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 5), transform.position);

        EventManager.Instance.PostEvent("Dash");

        gravity = 0;
        moveSpeed += DashSpeed;

        yield return new WaitForSeconds(dashTime);

        gravity = dashGravity;
        moveSpeed = startSpeed;
        owner.AnimatorController.AnimatorSetBool("IsDashing", false);

        isDashing = false;
        yield return new WaitForSeconds(DashCooldown);

        dashCoroutine = null;
    }
    private IEnumerator IKnockback(Vector2 knockdownDirection, Vector2 knockbackForce)
    {
        velocity.x = (velocity.x) > 0 ?
            knockdownDirection.x * -(velocity.x + knockbackForce.x) :
            knockdownDirection.x * (velocity.x - knockbackForce.x);

        velocity.y = knockdownDirection.y + knockbackForce.y;

        yield return null;
        knockbackCoroutine = null;
    }
    private IEnumerator IInvincibility(float invincibilityDuration, float flashSpeed)
    {
        float currentInvincibilityDuration = 0;
        StartCoroutine(IFlashSpriteRenderer(flashSpeed));

        while (currentInvincibilityDuration <= invincibilityDuration)
        {
            currentInvincibilityDuration += Time.deltaTime;
            yield return null;
        }

        owner.ChangePlayerState(PlayerState.NORMAL);
        invincibilityCoroutine = null;
    }
    private IEnumerator IFlashSpriteRenderer(float flashSpeed)
    {
        while (owner.CurrentState.Equals(PlayerState.INVINCIBILITY))
        {
            owner.SpriteRenderer.enabled = !owner.SpriteRenderer.enabled;

            yield return new WaitForSeconds(flashSpeed);
        }

        owner.SpriteRenderer.enabled = true;
    }
    private IEnumerator IRespawn(Vector2 spawnPoint, float respawnDelay)
    {
        owner.Controller2D.Collider2D.enabled = false;
        var respawnEffect = ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 3), new Vector2(spawnPoint.x, spawnPoint.y - 4f), Quaternion.Euler(new Vector2(-90, 0))).GetComponent<Effect>();

        //owner.PlayerData.PlayerInfo.StartRespawnCooldown(RespawnCooldown);
        owner.AnimatorController.AnimatorSetBool("HasDied", true);

        Vector2 startPosition = transform.position;
        Vector2 endPosition = spawnPoint;

        if (UsingLerpMethod1)
        {
            #region LERP_METHOD_1

            float startTime = Time.time;
            float totalDistanceToSpawnPoint = Vector2.Distance(transform.position, spawnPoint);

            while ((Vector2)transform.position != spawnPoint)
            {
                float currentDuration = Time.time - startTime;
                float journeyFraction = (currentDuration / totalDistanceToSpawnPoint) * 20;
                transform.position = Vector2.Lerp(startPosition, endPosition, journeyFraction);
                yield return null;
            }

            #endregion LERP_METHOD_1
        }
        else
        {
            #region LERP_METHOD_2

            float lerpTime = 1f;
            float startedLerpTime = Time.time;
            float perC = 0f;

            while (perC <= 1.0f)
            {
                float timeSinceStartedLerping = Time.time - startedLerpTime;
                perC = timeSinceStartedLerping / lerpTime;
                transform.position = Vector2.Lerp(startPosition, endPosition, perC);
                yield return null;
            }

            #endregion LERP_METHOD_2
        }

        owner.Controller2D.Collider2D.enabled = true;
        owner.ResetValues();

        yield return new WaitForSeconds(respawnDelay);

        respawnEffect.ParticleSystem.Stop();

        ObjectPoolManager.Instance.SpawnObject(ResourceManager.Instance.GetPrefabByIndex(5, 1), transform.position);

        owner.AnimatorController.AnimatorSetBool("HasDied", false);
        owner.ChangePlayerState(PlayerState.INVINCIBILITY);
        respawnCoroutine = null;
    }

    #endregion COROUTINES
}