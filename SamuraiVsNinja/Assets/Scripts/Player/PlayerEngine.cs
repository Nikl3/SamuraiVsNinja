using System.Collections;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    #region VARIABLES

    [SerializeField]
    private LayerMask hitLayer;
    [SerializeField]
    private float minJumpHeight = 1f;
    [SerializeField]
    private float maxJumpHeight = 4f;
    [SerializeField]
    private readonly float timeToJumpApex = 0.5f;
    [SerializeField]
    private float moveSpeed = 6f;
    private float startSpeed;
    [SerializeField]
    private readonly float maxWallSlideSpeed = 3f;
    [SerializeField]
    private Transform ProjectileSpawnPoint;

    private readonly float accelerationTimeAirbourne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float gravity;
    private float dashGravity;
    private Vector2 velocity;
    private float velocityXSmoothing;

    private Vector2 wallJumpClimb = new Vector2(2.5f, 16);
    private Vector2 wallJumpOff = new Vector2(8.5f, 7);
    private Vector2 wallLeap = new Vector2(18, 17);

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirectionX;

    [SerializeField]
    private float dashSpeed = 20;
    private bool isDashing = false;
    private float dashCooldown = 2f;
    private readonly float dashTime = 0.2f;

    private bool isRangeAttacking = false;
    private float rangeAttackCooldown = 2f;

    private Player player;

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

        if (player.Controller2D.Collisions.Above || player.Controller2D.Collisions.Below)
        {
            velocity.y = 0;
            player.Animator.SetBool("WallSlide", false);
        }
    }

    private void HandleWallSliding()
    {
        wallDirectionX = (player.Controller2D.Collisions.Left) ? -1 : 1;
        wallSliding = false;

        if ((player.Controller2D.Collisions.Left || player.Controller2D.Collisions.Right) && !player.Controller2D.Collisions.Below && velocity.y < 0)
        {
            if (directionalInput.x == 0)
            {
                return;
            }

            wallSliding = true;
            player.Animator.SetBool("WallSlide", true);


            if (velocity.y < -maxWallSlideSpeed)
            {
                velocity.y = -maxWallSlideSpeed;
            }
        } 
        else
        {
            player.Animator.SetBool("WallSlide", false);
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

    //public void OnMeleeAttack()
    //{

    //}

    public void OnRangedAttack()
    {
        if(!isRangeAttacking)
        StartCoroutine(IRangeAttack());     
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
        player.Animator.SetTrigger("Throw");
        player.PlayerInfo.StartRangeCooldown(RangeAttackCooldown);

        var projectile = Instantiate(ResourceManager.Instance.GetPrefabByIndex(3, 0), ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().ProjectileInitialize(player.Controller2D.Collisions.FaceDirection);

        yield return new WaitUntil(() => !player.PlayerInfo.IsCooldown);
        //yield return new WaitForSeconds(RangeAttackCooldown);

        isRangeAttacking = false;
    }

    private IEnumerator IDash()
    {
        player.Animator.SetBool("Dash", true);
        isDashing = true;
        gravity = 0;
        moveSpeed = isDashing ? DashSpeed + moveSpeed : moveSpeed;

        yield return new WaitForSeconds(dashTime);

        gravity = dashGravity;
        moveSpeed = startSpeed;
        player.Animator.SetBool("Dash", false);

        yield return new WaitForSeconds(DashCooldown);
        isDashing = false;
    }
}