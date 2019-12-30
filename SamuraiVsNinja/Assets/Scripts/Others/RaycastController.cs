using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public abstract class RaycastController : MonoBehaviour
    {
        #region VARIABLES

        private const float DISTANCE_BETWEEN_RAYS = 0.25f;
        protected const float SKIN_WIDTH = 0.15f;
        protected int HorizontalRayCount;
        protected int VerticalRayCount;
        protected float horizontalRaySpacing;
        protected float verticalRaySpacing;
        protected RaycastOrigins raycastOrigins;

        private Collider2D hitCollider2D;

        #endregion VARIABLES

        #region UNITY_FUNCTIONS

        protected virtual void Awake()
        {
            hitCollider2D = GetComponentInChildren<Collider2D>();
        }

        protected virtual void Start()
        {
            CalculateRaySpacing();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected struct RaycastOrigins
        {
            public Vector2 TopLeft;
            public Vector2 TopRight;
            public Vector2 BottomLeft;
            public Vector2 BottomRight;
        }

        protected void UpdateRaycastOrigins()
        {
            var bounds = hitCollider2D.bounds;
            bounds.Expand(SKIN_WIDTH * -2);

            raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private void CalculateRaySpacing()
        {
            var bounds = hitCollider2D.bounds;
            bounds.Expand(SKIN_WIDTH * -2);

            var boundsWidth = bounds.size.x;
            var boundsHeight = bounds.size.y;

            HorizontalRayCount = Mathf.RoundToInt(boundsHeight / DISTANCE_BETWEEN_RAYS);
            VerticalRayCount = Mathf.RoundToInt(boundsWidth / DISTANCE_BETWEEN_RAYS);

            horizontalRaySpacing = bounds.size.y / (HorizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}