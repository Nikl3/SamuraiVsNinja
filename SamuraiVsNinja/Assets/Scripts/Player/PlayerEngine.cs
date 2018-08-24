using System.Collections;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
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

    private float dashSpeed = 20;
    private bool isDashing = false;
    private float dashCooldown = 2f;
    private float dashTime = 0.2f;

    private CharacterController2D playerController;

    private void Start()
    {
        playerController = GetComponent<CharacterController2D>();

        startSpeed = moveSpeed;
        gravity = dashGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2* Mathf.Abs(gravity) * minJumpHeight);
    }

    public void CalculateMovement()
    {
        CalculateVelocity();
        HandleWallSliding();
        
        playerController.Move(velocity * Time.deltaTime, directionalInput);

        if (playerController.Collisions.Above || playerController.Collisions.Below)
        {
            velocity.y = 0;
        }
    }

    private void HandleWallSliding()
    {
        wallDirectionX = (playerController.Collisions.Left) ? -1 : 1;
        wallSliding = false;

        if ((playerController.Collisions.Left || playerController.Collisions.Right) && !playerController.Collisions.Below && velocity.y < 0)
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
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (playerController.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirbourne);
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

        if (playerController.Collisions.Below)
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