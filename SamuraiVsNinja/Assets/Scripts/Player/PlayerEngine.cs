using System.Collections;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    public GameObject RangedAttackPrefab;

    [SerializeField]
    private LayerMask hitLayer;
    [SerializeField]
    private float minJumpHeight = 1f;
    [SerializeField]
    private float maxJumpHeight = 4f;
    [SerializeField]
    private float timeToJumpApex = 0.5f;
    [SerializeField]
    private float moveSpeed = 6f;
    private float startSpeed;
    [SerializeField]
    private float maxWallSlideSpeed = 3f;

    private float accelerationTimeAirbourne = 0.2f;
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
    private float dashTime = 0.2f;

    private CharacterController2D controller2D;

    private void Awake()
    {
        controller2D = GetComponent<CharacterController2D>();
    }

    private void Start()
    {
        startSpeed = moveSpeed;
        gravity = dashGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2* Mathf.Abs(gravity) * minJumpHeight);
    }

    public void CalculateMovement()
    {
        CalculateVelocity();
        HandleWallSliding();
        
        controller2D.Move(velocity * Time.deltaTime, directionalInput);

        if (controller2D.Collisions.Above || controller2D.Collisions.Below)
        {
            velocity.y = 0;
        }
    }

    private void HandleWallSliding()
    {
        wallDirectionX = (controller2D.Collisions.Left) ? -1 : 1;
        wallSliding = false;

        if ((controller2D.Collisions.Left || controller2D.Collisions.Right) && !controller2D.Collisions.Below && velocity.y < 0)
        {
            if (directionalInput.x == 0)
            {
                return;
            }

            wallSliding = true;

            if (velocity.y < -maxWallSlideSpeed)
            {
                velocity.y = -maxWallSlideSpeed;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller2D.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirbourne);
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

        if (controller2D.Collisions.Below)
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

    public void OnAttack()
    {
        var foo = controller2D.Collisions.FaceDirection;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + new Vector2(1, 0) * foo , Vector2.right * foo, 4f, hitLayer);

        //Debug.DrawRay((Vector2)transform.position + new Vector2(1,0)* foo, (Vector2.right * foo) * 4);

        if (hit)
        {

            Debug.Log(hit.collider.tag);
        }
                    
    }

    public void OnRangedAttack()
    {
       var currentDir = controller2D.Collisions.FaceDirection;
       var bullet = Instantiate(RangedAttackPrefab, transform.position, Quaternion.Euler(new Vector3(currentDir, 0, 0)));
       bullet.GetComponent<RangedAmmo>().BulletMove(currentDir);
    }

    public void OnDash()
    {
        if(isDashing == false)
        {
            StartCoroutine(IDash());        
        }
    }

    private IEnumerator IDash()
    {
        isDashing = true;
        gravity = 0;
        moveSpeed = isDashing ? dashSpeed + moveSpeed : moveSpeed;

        yield return new WaitForSeconds(dashTime);

 
        gravity = dashGravity;
        moveSpeed = startSpeed;

        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
}