using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{

    [Header("Collisions")]
    [Tooltip("Mask of layers the controller will collide with")]
    public LayerMask collisionMask;

    const float skinWidth = .015f;
    [Tooltip("Count of horizontal checks made along the Y axis")]
    public int horizontalRayCount = 4;
    [Tooltip("Count of vertical checks made along the X axis")]
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    //Components
    BoxCollider2D boxCollider;

    //Info
    #region info structs
    RaycastOrigins raycastOrigins;
    struct RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    [HideInInspector] //TODO: expose as label with custom inspector
    public CollisionInfo collisions;
    [System.Serializable]
    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public void Reset(){
            above = below = left = right = false;
        }
    }
    #endregion

    private void Start() {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity){
        UpdateRaycastOrigins();
        collisions.Reset();

        if(velocity.x != 0)
            HorizontalCollisions(ref velocity);
        if(velocity.y != 0)
            VerticalCollisions(ref velocity);

        transform.Translate(velocity);
    }

    #region CALCULATIONS
    void HorizontalCollisions(ref Vector3 velocity){
        float directionX = Mathf.Sign(velocity.x);
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;
        for(int i = 0; i < horizontalRayCount; i++){
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up *( horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

            if(hit){
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLenght = hit.distance;
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity){
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;
        for(int i = 0; i < verticalRayCount; i++){
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right *( verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLenght, Color.red);

            if(hit){
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void UpdateRaycastOrigins() {
        if(boxCollider == null){
            if( (boxCollider = GetComponent<BoxCollider2D>()) == null)
                return;
            CalculateRaySpacing();
        };
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing(){
        Bounds bounds = this.boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount -1 );
        verticalRaySpacing = bounds.size.x / (verticalRayCount -1 );

    }
#endregion

}
