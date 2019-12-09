using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class PlatformController : RaycastController
    {
        #region VARIABLES

        public bool IsCycling;

        [Range(0, 2)] public float EaseAmount;
        public float Speed;
        public float WaitTime;
        private float nextMoveTime;
        private float distancePercentBetweenWaypoints;
        private int fromWaypointIndex;

        public LayerMask PassengerMask;
        public Vector2[] LocalWayPoints;
        private Vector2[] globalWaypoints;
        public Color WaypointColor = Color.red;

        private List<PassengerMovement> passengerMovement;
        private Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Start()
        {
            base.Start();

            globalWaypoints = new Vector2[LocalWayPoints.Length];

            for(int i = 0; i < globalWaypoints.Length; i++)
            {
                globalWaypoints[i] = LocalWayPoints[i] + (Vector2)transform.position;
            }
        }

        private void Update()
        {
            UpdateRaycastOrigins();

            var velocity = CalculatePlatformMovement();

            CalculatePassengerMovement(velocity);

            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);
        }

        private void OnDrawGizmos()
        {
            if(LocalWayPoints == null)
            {
                return;
            }

            if(LocalWayPoints.Length == 0)
            {
                return;
            }

            Gizmos.color = WaypointColor;
            var size = 0.25f;

            for(int i = 0; i < LocalWayPoints.Length; i++)
            {
                var globalWaypoint = (Application.isPlaying) ? globalWaypoints[i] : LocalWayPoints[i] + (Vector2)transform.position;
                Gizmos.DrawLine(globalWaypoint + Vector2.down * size, globalWaypoint + Vector2.up * size);
                Gizmos.DrawLine(globalWaypoint + Vector2.left * size, globalWaypoint + Vector2.right * size);
            }

        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private struct PassengerMovement
        {
            public Transform Transform;
            public Vector2 Velocity;
            public bool IsStandingOnPlatform;
            public bool IsMovingBeforePlatform;

            public PassengerMovement(Transform transform, Vector2 velocity, bool isStandingOnPlatform, bool isMovingBeforePlatform)
            {
                Transform = transform;
                Velocity = velocity;
                IsStandingOnPlatform = isStandingOnPlatform;
                IsMovingBeforePlatform = isMovingBeforePlatform;
            }
        }

        private float Ease(float x)
        {
            var a = EaseAmount + 1;
            return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
        }

        private Vector2 CalculatePlatformMovement()
        {
            if(Time.time < nextMoveTime)
            {
                return Vector2.zero;
            }

            fromWaypointIndex %= globalWaypoints.Length;

            var toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;

            var distanceBetweenWaypoints = Vector2.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);

            distancePercentBetweenWaypoints += Time.deltaTime * Speed / distanceBetweenWaypoints;

            distancePercentBetweenWaypoints = Mathf.Clamp01(distancePercentBetweenWaypoints);
            var easedPercentBetweenWaypoints = Ease(distancePercentBetweenWaypoints);

            var newPosition = Vector2.Lerp(
                    globalWaypoints[fromWaypointIndex],
                    globalWaypoints[toWaypointIndex],
                    easedPercentBetweenWaypoints);

            if(distancePercentBetweenWaypoints >= 1)
            {
                distancePercentBetweenWaypoints = 0;
                fromWaypointIndex++;

                if(IsCycling == false)
                {
                    if(fromWaypointIndex >= globalWaypoints.Length - 1)
                    {
                        fromWaypointIndex = 0;
                        Array.Reverse(globalWaypoints);
                    }
                }

                nextMoveTime = Time.time + WaitTime;
            }

            return newPosition - (Vector2)transform.position;
        } 

        private void MovePassengers(bool beforeMovePlatform)
        {
            foreach(var passenger in passengerMovement)
            {
                if(passengerDictionary.ContainsKey(passenger.Transform) == false)
                {
                    passengerDictionary.Add(passenger.Transform, passenger.Transform.GetComponent<Controller2D>());
                }

                if(passenger.IsMovingBeforePlatform == beforeMovePlatform)
                {
                    passengerDictionary[passenger.Transform].Move(passenger.Velocity, passenger.IsStandingOnPlatform);
                }
            }
        }

        private void CalculatePassengerMovement(Vector2 velocity)
        {
            var movedPassengers = new HashSet<Transform>();
            passengerMovement = new List<PassengerMovement>();

            var direction_X = Mathf.Sign(velocity.x);
            var direction_Y = Mathf.Sign(velocity.y);

            // Vertically moving platform...
            if(velocity.y != 0)
            {
                var rayLenght = Mathf.Abs(velocity.y) + SKIN_WIDTH;

                for(int i = 0; i < VerticalRayCount; i++)
                {
                    var rayOrigin
                        = (direction_Y == -1
                        ? raycastOrigins.BottomLeft
                        : raycastOrigins.TopLeft);

                    rayOrigin += Vector2.right * (verticalRaySpacing * i);

                    var hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction_Y, rayLenght, PassengerMask);

                    if(hit && hit.distance != 0)
                    {
                        if(movedPassengers.Contains(hit.transform) == false)
                        {
                            movedPassengers.Add(hit.transform);

                            var push_X = direction_Y == 1 ? velocity.x : 0;
                            var push_Y = velocity.y - (hit.distance - SKIN_WIDTH) * direction_Y;

                            passengerMovement.Add(
                                new PassengerMovement(
                                    hit.transform, 
                                    new Vector2(push_X, push_Y),
                                    direction_Y == 1, 
                                    true)
                                );
                        }
                    }
                }
            }

            // Horizontally moving platform...
            if(velocity.x != 0)
            {
                var rayLenght = Mathf.Abs(velocity.x) * SKIN_WIDTH;

                for(int i = 0; i < HorizontalRayCount; i++)
                {
                    var rayOrigin = (direction_X == -1) ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;
                    rayOrigin += Vector2.up * (horizontalRaySpacing * 1);

                    var hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction_X, rayLenght, PassengerMask);

                    if(hit && hit.distance != 0)
                    {
                        if(movedPassengers.Contains(hit.transform) == false)
                        {
                            movedPassengers.Add(hit.transform);

                            var push_X = velocity.x - (hit.distance - SKIN_WIDTH) * direction_X;
                            var push_Y = -SKIN_WIDTH;

                            passengerMovement.Add(
                                new PassengerMovement(
                                    hit.transform,
                                    new Vector2(push_X, push_Y),
                                    false, 
                                    true)
                                );
                        }
                    }
                }
            }

            // Passenger on top of horizontally or downward moving platform
            if(direction_Y == -1 || velocity.y == 0 && velocity.x != 0)
            {
                var rayLenght = SKIN_WIDTH * 2;

                for(int i = 0; i < VerticalRayCount; i++)
                {
                    var rayOrigin = raycastOrigins.TopLeft + Vector2.right * (verticalRaySpacing * i);
                    var hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLenght, PassengerMask);

                    if(hit && hit.distance != 0)
                    {
                        if(movedPassengers.Contains(hit.transform) == false)
                        {
                            movedPassengers.Add(hit.transform);
                            var push_X = velocity.x;
                            var push_Y = velocity.y;

                            passengerMovement.Add(
                                new PassengerMovement(
                                    hit.transform,
                                    new Vector2(push_X, push_Y),
                                    true,
                                    false)
                                );
                        }
                    }
                }
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

