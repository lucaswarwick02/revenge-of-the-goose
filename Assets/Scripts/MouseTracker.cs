using UnityEngine;
using Game.Utility;

public class MouseTracker : MonoBehaviour
{
    public const float MOUSE_PLANE_HEIGHT = 0.25f;

    public Vector3? MouseWorldPosition { get; private set; }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (MousePlaneIntersection(ray.origin, ray.direction, out Vector3 intersect))
        {
            MouseWorldPosition = intersect;
        }
        else
        {
            MouseWorldPosition = null;
        }
    }

    public static bool MousePlaneIntersection(Vector3 linePoint, Vector3 lineDirection, out Vector3 intersectPoint)
        => VectorMaths.LinePlaneIntersection(new Vector3(0, MOUSE_PLANE_HEIGHT, 0), Vector3.up, linePoint, lineDirection, out intersectPoint);
}
