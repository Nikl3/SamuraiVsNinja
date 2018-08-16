using UnityEngine;

public class CharacterController2D : RaycastController
{
    public CollisionInfo Collisions;
    public Vector2 PlayerInput { get; private set; }

    protected override void Awake()
    {
        collisionLayerName = "Obstacle";
        oneWayCollisionTag = "OneWayObject";
        Collisions.FaceDirection = 1;
        base.Awake();
    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();

        Collisions.Reset();
        Collisions.MoveAmountOld = moveAmount;
        PlayerInput = input;

        if (moveAmount.x != 0)
        {
            Collisions.FaceDirection = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            Collisions.Below = true;
        }
    }

    private void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = Collisions.FaceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;

        if (Mathf.Abs(moveAmount.x) < SKIN_WIDTH)
        {
            rayLength = 2 * SKIN_WIDTH;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                moveAmount = Collisions.MoveAmountOld;

                moveAmount.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance;

                Collisions.Left = directionX == -1;
                Collisions.Right = directionX == 1;
            }
        }
    }

    private void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + SKIN_WIDTH;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if (hit.collider.CompareTag(oneWayCollisionTag))
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (Collisions.FallingThroughPlatform)
                    {
                        continue;
                    }
                    if (PlayerInput.y == -1)
                    {
                        Collisions.FallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.5f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;

                Collisions.Below = directionY == -1;
                Collisions.Above = directionY == 1;
            }
        }
    }

    private void ResetFallingThroughPlatform()
    {
        Collisions.FallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool Above, Below;
        public bool Left, Right;

        public Vector2 MoveAmountOld;
        public int FaceDirection;
        public bool FallingThroughPlatform;

        public void Reset()
        {
            Above = Below = false;
            Left = Right = false;
        }
    }
}