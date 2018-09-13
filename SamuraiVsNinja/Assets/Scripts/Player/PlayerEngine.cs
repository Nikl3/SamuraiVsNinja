using System.Collections;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    #region VARIABLES

    private Player player;

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
    private float maxWallSlideSpeed = 3f;
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

    #region RANGE_ATTACK

    [Header("RANGE ATTACK")]

    [SerializeField]
    private Transform ProjectileSpawnPoint;

    private bool isRangeAttacking = false;
    [SerializeField]
    private readonly float rangeAttackCooldown = 2f;

    #endregion RANGE_ATTACK

    private readonly float respawnCooldown = 2f;

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

    public float RangeAttackCooldown
    {
        get
        {
            return rangeAttackCooldown;
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
        player = GetComponent<Player>();
    }

    private void Start()
    {
        startSpeed = moveSpeed;
        gravity = dashGravity = -(2 * MaxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2* Mathf.Abs(gravity) * MinJumpHeight);
    }

    private void HandleWallSliding()
    {
        wallDirectionX = (player.Controller2D.Collisions.Left) ? -1 : 1;

        wallSliding = false;
        player.AnimatorController.AnimatorSetBool("IsWallsliding", false);

        if ((player.Controller2D.Collisions.Left || player.Controller2D.Collisions.Right) && !player.Controller2D.Collisions.Below && velocity.y < 0)
        {
            if (directionalInput.x == 0)
            {
                return;
            }

            wallSliding = true;
            player.AnimatorController.AnimatorSetBool("IsWallsliding", true);

            if (velocity.y < -maxWallSlideSpeed)
            {
                velocity.y = -maxWallSlideSpeed;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (player.Controller2D.Collisions.Below) ? AccelerationTimeGrounded : accelerationTimeAirbourne);
        velocity.y += gravity * Time.deltaTime;
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

        player.Controller2D.Move(velocity * Time.deltaTime, directionalInput);

        player.AnimatorController.AnimatorSetBool("IsRunning", Mathf.Abs(directionalInput.x) > 0 ? true : false);
     
        if (player.Controller2D.Collisions.Above || player.Controller2D.Collisions.Below)
        {
            velocity.y = 0;
            player.AnimatorController.AnimatorSetBool("IsJumping", false);
            player.AnimatorController.AnimatorSetBool("IsDropping", false);
        }
        else
        {
            player.AnimatorController.AnimatorSetBool("IsJumping", velocity.y > 0 ? true : false);
            player.AnimatorController.AnimatorSetBool("IsDropping", velocity.y < 0 ? true : false);
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
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
        }

        if (player.Controller2D.Collisions.Below)
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

    public void OnMeleeAttack()
    {
        player.AnimatorController.AnimatorSetTrigger("Attack");
    }

    public void OnRangedAttack()
    {
        if (!isRangeAttacking) 
        { 
            StartCoroutine(IRangeAttack());
        }
    }

    public void OnDash()
    {
        if(!isDashing)
        {
            StartCoroutine(IDash());
        }
    }

    public void OnKnockback(Vector2 knockdownDirection, Vector2 knockbackForce)
    {
        StartCoroutine(IKnockback(knockdownDirection, knockbackForce));
    }

    public void StartInvincibility(float invincibilityDuartion, float flashSpeed)
    {
        StartCoroutine(IInvincibility(invincibilityDuartion, flashSpeed));
    }

    public void Respawn(Vector2 spawnPoint)
    {
        StartCoroutine(IRespawn(spawnPoint));
    }

    #region COROUTINES

    private IEnumerator IRangeAttack()
    {
        isRangeAttacking = true;
        player.AnimatorController.AnimatorSetTrigger("Throw");
        player.PlayerInfo.StartRangeCooldown(isRangeAttacking, RangeAttackCooldown);

        var projectile = Instantiate(ResourceManager.Instance.GetPrefabByIndex(3, 0), ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Kunai>().ProjectileInitialize(player.Controller2D.Collisions.FaceDirection);

        yield return new WaitForSeconds(RangeAttackCooldown);
        isRangeAttacking = false;
    }

    private IEnumerator IDash()
    {
        isDashing = true;
        player.AnimatorController.AnimatorSetBool("IsDashing", true);
        player.PlayerInfo.StartDashCooldown(DashCooldown);

        gravity = 0;
        moveSpeed = isDashing ? DashSpeed + moveSpeed : moveSpeed;

        yield return new WaitForSeconds(dashTime);

        gravity = dashGravity;
        moveSpeed = startSpeed;
        player.AnimatorController.AnimatorSetBool("IsDashing", false);
       
        yield return new WaitForSeconds(DashCooldown);
        isDashing = false;
    }

    private IEnumerator IKnockback(Vector2 knockdownDirection, Vector2 knockbackForce)
    {
        //print("Knockdown direction: " + knockdownDirection);
        //print("Knockback force: " + knockbackForce);

        velocity.x = (velocity.x) > 0 ?
            knockdownDirection.x * -(velocity.x + knockbackForce.x) :
            knockdownDirection.x * (velocity.x - knockbackForce.x);

        velocity.y = knockdownDirection.y + knockbackForce.y;

        yield return null;
    }

    private IEnumerator IInvincibility(float invincibilityDuration, float flashSpeed)
    {
        float currentInvincibilityDuration = 0;
        StartCoroutine(IFlashSpriteRenderer(flashSpeed));

        while(currentInvincibilityDuration <= invincibilityDuration)
        {
            currentInvincibilityDuration += Time.deltaTime;
            yield return null;
        }

        player.ChangePlayerState(PlayerState.NORMAL);
    }

    private IEnumerator IFlashSpriteRenderer(float flashSpeed)
    {
        while (player.CurrentState.Equals(PlayerState.INVINCIBILITY))
        {
            player.SpriteRenderer.enabled = !player.SpriteRenderer.enabled;

            yield return new WaitForSeconds(flashSpeed);
        }

        player.SpriteRenderer.enabled = true;
    }

    private IEnumerator IRespawn(Vector2 spawnPoint)
    {
        player.PlayerInfo.StartRespawnCooldown(RespawnCooldown);

        player.AnimatorController.AnimatorSetBool("HasDied", true);
        var bc = player.GetComponent<BoxCollider2D>();
        bc.enabled = false;

        float startTime = Time.time;
        float totalDistanceToSpawnPoint = Vector2.Distance(transform.position, spawnPoint);

        while ((Vector2)transform.position != spawnPoint)
        {
            float currentDuration = Time.time - startTime;
            float journeyFraction = currentDuration / totalDistanceToSpawnPoint;
            transform.position = Vector2.Lerp(transform.position, spawnPoint, journeyFraction);
            yield return null;
        }
        bc.enabled = true;
        player.AnimatorController.AnimatorSetBool("HasDied", false);
        player.ChangePlayerState(PlayerState.INVINCIBILITY);
    }

    #endregion COROUTINES
}