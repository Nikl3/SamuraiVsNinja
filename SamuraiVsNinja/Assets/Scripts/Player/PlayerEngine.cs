using System.Collections;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    #region VARIABLES

    private Player player;
    [SerializeField]
    private LayerMask hitLayer;

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
    private Vector2 wallJumpClimb = new Vector2(2.5f, 16);
    private Vector2 wallJumpOff = new Vector2(8.5f, 7);
    private Vector2 wallLeap = new Vector2(18, 17);
    private bool wallSliding;
    private int wallDirectionX;

    #endregion WALL_SLIDE

    #region DASH

    [Header("DASH")]
    [SerializeField]
    private float dashSpeed = 20;
    private bool isDashing = false;
    [SerializeField]
    private float dashCooldown = 2f;
    private readonly float dashTime = 0.3f;

    #endregion DASH

    #region RANGE_ATTACK

    [Header("RANGE ATTACK")]

    [SerializeField]
    private Transform ProjectileSpawnPoint;

    private bool isRangeAttacking = false;
    [SerializeField]
    private float rangeAttackCooldown = 2f;

    #endregion RANGE_ATTACK

#endregion VARIABLES

    #region PROPERTIES

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

        set
        {
            dashCooldown = value;
        }
    }

    public float RangeAttackCooldown
    {
        get
        {
            return rangeAttackCooldown;
        }

        set
        {
            rangeAttackCooldown = value;
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
        player.PlayAudioClip(0);
        player.AnimatorController.AnimatorSetTrigger("Attack");
    }

    public void OnRangedAttack()
    {
        if (!isRangeAttacking)
        {
            player.PlayAudioClip(1);
            StartCoroutine(IRangeAttack());
        }
    }

    public void OnDash()
    {
        if(isDashing == false)
        {
            StartCoroutine(IDash());
        }
    }

    public IEnumerator IRangeAttack()
    {
        isRangeAttacking = true;
        player.AnimatorController.AnimatorSetTrigger("Throw");
        player.PlayerInfo.StartRangeCooldown(RangeAttackCooldown);

        var projectile = Instantiate(ResourceManager.Instance.GetPrefabByIndex(3, 0), ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().ProjectileInitialize(player.Controller2D.Collisions.FaceDirection);

        yield return new WaitUntil(() => !player.PlayerInfo.IsRangeCooldown);

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

        yield return new WaitUntil(() => !player.PlayerInfo.IsDashCooldown);
        isDashing = false;
    }
}