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

    #region RANGE_ATTACK

    [Header("RANGE ATTACK")]

    private bool isRangeAttacking = false;
    [SerializeField]
    private readonly float rangeAttackCooldown = 2f;

    #endregion RANGE_ATTACK

    private readonly float respawnCooldown = 2f;
    
    private Coroutine rangeAttackCoroutine;
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
            player.AnimatorController.AnimatorSetBool("IsWallsliding", true);
            wallSliding = true;

            if (!InputManager.Instance.X_ButtonUp(player.PlayerData.ID))
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
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (player.Controller2D.Collisions.Below) ? AccelerationTimeGrounded : accelerationTimeAirbourne);
        velocity.y += gravity * Time.deltaTime;
    }

    private void CheckifRunningAndLocalScale()
    {  
        if (directionalInput.x != 0 && !wallSliding)
        {
            player.AnimatorController.PlayerGraphics.localScale = new Vector2(player.Controller2D.Collisions.FaceDirection > 0 ? -1 : 1, 1);

            player.AnimatorController.AnimatorSetBool("IsRunning", true);
        }
        else
        {
            player.AnimatorController.AnimatorSetBool("IsRunning", false);
        }
    }

    private void CheckTopAndBottomCollision()
    {
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

        CheckifRunningAndLocalScale();
        CheckTopAndBottomCollision();
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

    public void HandleMeleeAttacks()
    {
        player.AnimatorController.AnimatorSetTrigger("Attack");

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, Vector2.right, 1f, 10);
        if (hit)
        {
            Debug.LogError(hit.collider.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1f);
    }

    public void OnRangedAttack()
    {
        if (rangeAttackCoroutine == null &&
            !isRangeAttacking && 
            !wallSliding && 
            !IsDashing)
        {
            rangeAttackCoroutine = StartCoroutine(IRangeAttack());         
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
        if(knockbackCoroutine == null)
        {
            knockbackCoroutine = StartCoroutine(IKnockback(knockdownDirection, knockbackForce));
        }     
    }

    public void StartInvincibility(float invincibilityDuartion, float flashSpeed)
    {
        if(invincibilityCoroutine == null)
        {
            invincibilityCoroutine = StartCoroutine(IInvincibility(invincibilityDuartion, flashSpeed));
        }    
    }

    public void Respawn(Vector2 spawnPoint)
    {
        if (respawnCoroutine == null)
        {
            respawnCoroutine = StartCoroutine(IRespawn(spawnPoint));
        }
    }

    #region COROUTINES

    private IEnumerator IRangeAttack()
    {
        isRangeAttacking = true;
        player.AnimatorController.AnimatorSetTrigger("Throw");
        player.PlayerInfo.StartRangeCooldown(isRangeAttacking, RangeAttackCooldown);

        yield return new WaitForSeconds(RangeAttackCooldown);
        isRangeAttacking = false;
        rangeAttackCoroutine = null;
    }

    private IEnumerator IDash()
    {
        isDashing = true;

        player.PlayerInfo.StartDashCooldown(DashCooldown);
        player.AnimatorController.AnimatorSetBool("IsDashing", true);
        Fabric.EventManager.Instance.PostEvent("Dash");

        gravity = 0;
        moveSpeed =+ DashSpeed;

        yield return new WaitForSeconds(dashTime);

        gravity = dashGravity;
        moveSpeed = startSpeed;
        player.AnimatorController.AnimatorSetBool("IsDashing", false);

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

        while(currentInvincibilityDuration <= invincibilityDuration)
        {
            currentInvincibilityDuration += Time.deltaTime;
            yield return null;
        }

        player.ChangePlayerState(PlayerState.NORMAL);
        invincibilityCoroutine = null;
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

        float startTime = Time.time;
        float totalDistanceToSpawnPoint = Vector2.Distance(transform.position, spawnPoint);

        while ((Vector2)transform.position != spawnPoint)
        {
            float currentDuration = Time.time - startTime;
            float journeyFraction = currentDuration / totalDistanceToSpawnPoint;
            transform.position = Vector2.Lerp(transform.position, spawnPoint, journeyFraction);
            yield return null;
        }

        Instantiate(ResourceManager.Instance.GetPrefabByIndex(5, 1), transform.position, Quaternion.identity);
        player.AnimatorController.AnimatorSetBool("HasDied", false);
        player.ChangePlayerState(PlayerState.INVINCIBILITY);
        respawnCoroutine = null;
    }

    #endregion COROUTINES
}