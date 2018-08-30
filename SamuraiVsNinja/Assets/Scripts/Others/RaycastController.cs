using UnityEngine;

public abstract class RaycastController : MonoBehaviour
{
    protected const float DISTANCE_BETWEEN_RAYS = 0.25f;
    protected const float SKIN_WIDTH = 0.015f;

    protected int horizontalRayCount;
    protected int verticalRayCount;
    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    protected LayerMask collisionMaskLayer;
    protected string collisionLayerName;
    protected string oneWayCollisionTag;

    protected RaycastOrigins raycastOrigins;
    private BoxCollider2D boxCollider2D;

    //public Transform player;

    protected virtual void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        collisionMaskLayer = LayerMask.GetMask(collisionLayerName);
    }

    private void Start()
    {
        CalculateRaySpacing();
    }

    private void Update() {

        // boxcastiä seinän sisäänmenon estämiseksi
        //var boxcast = Physics2D.BoxCast(player.position, new Vector2(0.5f, 2), 0, Vector2.right);
        //if (boxcast) {
        //    Debug.DrawLine(player.position, boxcast.point, Color.blue);
        //}
    }

    protected void UpdateRaycastOrigins()
    {
        // if 

        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

   

    private void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider2D.bounds;
        bounds.Expand(SKIN_WIDTH * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / DISTANCE_BETWEEN_RAYS);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / DISTANCE_BETWEEN_RAYS);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    protected struct RaycastOrigins
    {
        public Vector2 TopLeft, TopRight;
        public Vector2 BottomLeft, BottomRight;
    }
}
