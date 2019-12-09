using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Controller2D : RaycastController
    {
        #region VARIABLES

        public LayerMask CollisionMask;

        private const int ONE_WAY_COLLIDER_LAYER = 10;

        private const float MAX_SLOPE_ANGLE = 55f;

        public CollisionInfo Collisions;

        private Character character;
        private Vector2 playerInput;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            character = GetComponent<Character>();    
        }

        protected override void Start()
        {
            base.Start();

            Collisions.FaceDirection = character.SpriteRenderer.flipX == false ? 1 : -1;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public struct CollisionInfo
        {
            public bool IsAbove;
            public bool IsBelow;
            public bool IsLeft;
            public bool IsRight;
            public bool IsClimbingSlope;
            public bool IsDescendingSlope;
            public bool IsSlidingDownMaxSlope;
            public float SlopeAngle;
            public float SlopeAngle_Previous;
            public Vector2 deltaMove_Previous;
            public Vector2 SlopeNormal;
            public int FaceDirection;

            public void Reset()
            {
                IsAbove = false;
                IsBelow = false;
                IsLeft = false;
                IsRight = false;
                IsClimbingSlope = false;
                IsDescendingSlope = false;
                IsSlidingDownMaxSlope = false;
                SlopeAngle_Previous = SlopeAngle;
                SlopeNormal = Vector2.zero;
                SlopeAngle = 0;
            }
        }

        public void Move(Vector2 deltaMove, bool isStandingOnPlatform = false)
        {
            Move(deltaMove, Vector2.zero, isStandingOnPlatform);           
        }

        public void Move(Vector2 deltaMove, Vector2 input, bool isStandinOnPlatform = false)
        {
            UpdateRaycastOrigins();

            Collisions.Reset();

            Collisions.deltaMove_Previous = deltaMove;

            playerInput = input;

            if(deltaMove.y < 0)
            {
                DescendSlope(ref deltaMove);
            }

            if(deltaMove.x != 0)
            {
                Collisions.FaceDirection = (int)Mathf.Sign(deltaMove.x);
            }

            HorizontalCollisions(ref deltaMove);

            if(deltaMove.y != 0)
            {
                VerticalCollisions(ref deltaMove);
            }

            transform.Translate(deltaMove);

            if(isStandinOnPlatform)
            {
                Collisions.IsBelow = true;
            }
        }

        private void HorizontalCollisions(ref Vector2 deltaMove)
        {
            var direction_X = Collisions.FaceDirection;
            var rayLenght = Mathf.Abs(deltaMove.x) + SKIN_WIDTH;

            if(Mathf.Abs(deltaMove.x) < SKIN_WIDTH)
            {
                rayLenght = 2 * SKIN_WIDTH;
            }

            for(int i = 0; i < HorizontalRayCount; i++)
            {
                var rayOrigin
                    = (direction_X == -1
                    ? raycastOrigins.BottomLeft
                    : raycastOrigins.BottomRight);

                rayOrigin += Vector2.up * (horizontalRaySpacing * i);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction_X, rayLenght, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * direction_X, Color.red);

                if(hit)
                {
                    if(hit.distance == 0)
                    {
                        continue;
                    }

                    var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if(i == 0 && slopeAngle <= MAX_SLOPE_ANGLE)
                    {
                        if(Collisions.IsDescendingSlope)
                        {
                            Collisions.IsDescendingSlope = false;
                            deltaMove = Collisions.deltaMove_Previous;
                        }

                        var distanceToSlopeStart = 0f;
                        if(slopeAngle != Collisions.SlopeAngle_Previous)
                        {
                            distanceToSlopeStart = hit.distance - SKIN_WIDTH;
                            deltaMove.x -= distanceToSlopeStart * direction_X;
                        }
                        ClimbSlope(ref deltaMove, slopeAngle, hit.normal);
                        deltaMove.x += distanceToSlopeStart * direction_X;
                    }

                    if(Collisions.IsClimbingSlope == false || slopeAngle > MAX_SLOPE_ANGLE)
                    {
                        deltaMove.x = (hit.distance - SKIN_WIDTH) * direction_X;
                        rayLenght = hit.distance;

                        if(Collisions.IsClimbingSlope)
                        {
                            deltaMove.y = Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(deltaMove.x);
                        }

                        Collisions.IsLeft = direction_X == -1;
                        Collisions.IsRight = direction_X == 1;
                    }                  
                }
            }
        }

        private void VerticalCollisions(ref Vector2 deltaMove)
        {
            var direction_Y = Mathf.Sign(deltaMove.y);
            var rayLenght = Mathf.Abs(deltaMove.y) + SKIN_WIDTH;

            for(int i = 0; i < VerticalRayCount; i++)
            {              
                var rayOrigin
                    = (direction_Y == -1
                    ? raycastOrigins.BottomLeft
                    : raycastOrigins.TopLeft);

                rayOrigin += Vector2.right * (verticalRaySpacing * i + deltaMove.x);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction_Y, rayLenght, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * direction_Y, Color.red);

                if(hit)
                {
                    if(hit.collider.gameObject.layer == ONE_WAY_COLLIDER_LAYER)
                    {
                        if(direction_Y == 1 || hit.distance == 0)
                        {
                            continue;
                        }

                        // Drop down... fix me!!!
                        if(playerInput.y == -1 && InputManager.Instance.InputActions.Player.Jump.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
                        {
                            continue;
                        }
                    }

                    deltaMove.y = (hit.distance - SKIN_WIDTH) * direction_Y;
                    rayLenght = hit.distance;

                    if(Collisions.IsClimbingSlope)
                    {
                        deltaMove.x = deltaMove.y / Mathf.Tan(Collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(deltaMove.x);
                    }

                    Collisions.IsBelow = direction_Y == -1;
                    Collisions.IsAbove = direction_Y == 1;
                }
            }

            if(Collisions.IsClimbingSlope)
            {
                var direction_X = Mathf.Sign(deltaMove.x);
                rayLenght = Mathf.Abs(deltaMove.x) + SKIN_WIDTH;

                var rayOrigin = 
                    ((direction_X == -1) 
                    ? raycastOrigins.BottomLeft 
                    : raycastOrigins.BottomRight) * Vector2.up * deltaMove.y;

                var hit = Physics2D.Raycast(
                    rayOrigin, 
                    Vector2.right * direction_X,
                    rayLenght,
                    CollisionMask);

                if(hit)
                {
                    var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if(slopeAngle != Collisions.SlopeAngle)
                    {
                        deltaMove.x = (hit.distance - SKIN_WIDTH) * direction_X;
                        Collisions.SlopeAngle = slopeAngle;
                        Collisions.SlopeNormal = hit.normal;
                    }
                }
            }
        }

        private void ClimbSlope(ref Vector2 deltaMove, float slopeAngle, Vector2 slopeNormal)
        {
            var moveDistance = Mathf.Abs(deltaMove.x);
            var climbMove_Y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            if(deltaMove.y <= climbMove_Y)
            {
                deltaMove.y = climbMove_Y;
                deltaMove.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(deltaMove.x);
                Collisions.IsBelow = true;
                Collisions.IsClimbingSlope = true;
                Collisions.SlopeAngle = slopeAngle;
                Collisions.SlopeNormal = slopeNormal;
            }
        }

        private void DescendSlope(ref Vector2 deltaMove)
        {
            var maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.BottomLeft, Vector2.down, Mathf.Abs(deltaMove.y) + SKIN_WIDTH, CollisionMask);
            var maxSlopeHitRight= Physics2D.Raycast(raycastOrigins.BottomRight, Vector2.down, Mathf.Abs(deltaMove.y) + SKIN_WIDTH, CollisionMask);

            if(maxSlopeHitLeft ^ maxSlopeHitRight)
            {
                SlideDownMaxSlope(maxSlopeHitLeft, ref deltaMove);
                SlideDownMaxSlope(maxSlopeHitRight, ref deltaMove);
            }

            if(Collisions.IsSlidingDownMaxSlope)
            {
                return;
            }

            var direction_X = Mathf.Sign(deltaMove.x);
            var rayOrigin = (direction_X == -1) ? raycastOrigins.BottomRight : raycastOrigins.BottomLeft;
            var hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, CollisionMask);

            if(hit)
            {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != 0 && slopeAngle <= MAX_SLOPE_ANGLE)
                {
                    if(Mathf.Sign(hit.normal.x) == direction_X)
                    {
                        if(hit.distance - SKIN_WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(deltaMove.x))
                        {
                            var moveDistance = Mathf.Abs(deltaMove.x);
                            var descendMove_Y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            deltaMove.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(deltaMove.x);
                            deltaMove.y -= descendMove_Y;

                            Collisions.SlopeAngle = slopeAngle;
                            Collisions.IsDescendingSlope = true;
                            Collisions.IsBelow = true;
                            Collisions.SlopeNormal = hit.normal;
                        }
                    }
                }
            }
        }

        private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 deltaMove)
        {
            if(hit)
            {
                var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(slopeAngle > MAX_SLOPE_ANGLE)
                {
                    deltaMove.x = hit.normal.x * (Mathf.Abs(deltaMove.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                    Collisions.SlopeAngle = slopeAngle;
                    Collisions.IsSlidingDownMaxSlope = true;
                    Collisions.SlopeNormal = hit.normal;
                }
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

