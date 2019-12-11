﻿using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Controller2D : RaycastController
    {
        #region VARIABLES

        public LayerMask CollisionMask;

        private const int ONE_WAY_COLLIDER_LAYER = 10;

        private const float MAX_SLOPE_ANGLE = 55f;

        public CollisionInfo Collisions;

        private CharacterEngine character;
        private Vector2 playerInput;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            character = GetComponent<CharacterEngine>();    
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

            public Vector2 deltaMove_Previous;
            public int FaceDirection;

            public void Reset()
            {
                IsAbove = false;
                IsBelow = false;
                IsLeft = false;
                IsRight = false;
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

                DebugManager.Instance.DrawRay(rayOrigin, Vector2.right * direction_X);

                if(hit)
                {
                    if(hit.distance == 0)
                    {
                        continue;
                    }
         
                    deltaMove.x = (hit.distance - SKIN_WIDTH) * direction_X;
                    rayLenght = hit.distance;

                    Collisions.IsLeft = direction_X == -1;
                    Collisions.IsRight = direction_X == 1;                                  
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

                DebugManager.Instance.DrawRay(rayOrigin, Vector2.up * direction_Y);

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

                    Collisions.IsBelow = direction_Y == -1;
                    Collisions.IsAbove = direction_Y == 1;
                }
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

