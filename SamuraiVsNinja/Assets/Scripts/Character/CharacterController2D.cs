using UnityEngine;

public class CharacterController2D : RaycastController
{
    private CollisionInfo collisions;
    private Vector2 playerInput;
    private Character player;

    public CollisionInfo Collisions
    {
        get
        {
            return collisions;
        }
    }
    public Collider2D Collider2D
    {
        get
        {
            return boxCollider2D;
        }
    }
    public LayerMask CollisionLayer
    {
        get
        {
            return collisionMaskLayer;
        }
    }

    protected override void Awake()
    {
        oneWayCollisionTag = "OneWayObject";
        collisions.FaceDirection = 1;

        base.Awake();
        player = GetComponent<Character>();
    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();

        collisions.Reset();
        collisions.MoveAmountOld = moveAmount;
        playerInput = input;

        if (moveAmount.x != 0)
        {
            collisions.FaceDirection = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (standingOnPlatform)
        {
            collisions.Below = true;
        }
    }

    private void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.FaceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;

        if (Mathf.Abs(moveAmount.x) < SKIN_WIDTH)
        {
            rayLength = 2 * SKIN_WIDTH;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMaskLayer);

            DebugManager.Instance.DrawRay(rayOrigin, Vector2.right, directionX, Color.red);
          
            if(hit)
            {
                DebugManager.Instance.DebugMessage(1, hit.transform.name);

                if (hit.distance == 0)
                {
                    continue;
                }

                moveAmount = collisions.MoveAmountOld;

                moveAmount.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance;

                collisions.Left = directionX == -1;
                collisions.Right = directionX == 1;
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMaskLayer);

            DebugManager.Instance.DrawRay(rayOrigin, Vector2.up, directionY, Color.red);

            if (hit)
            {
                if (hit.collider.CompareTag(oneWayCollisionTag))
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.FallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1 && InputManager.Instance.A_ButtonDown(player.PlayerData.ID))
                    {
                        collisions.FallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.2f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;

                collisions.Below = directionY == -1;
                collisions.Above = directionY == 1;
            }
        }
    }

    private void ResetFallingThroughPlatform()
    {
        collisions.FallingThroughPlatform = false;
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