using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class MouseTracker : MonoBehaviour
{
    [SerializeField] private float mousePlaneHeight = 0.1f;

    public Vector3? MouseWorldPosition { get; private set; }

    public float MousePlaneHeight => mousePlaneHeight;

    void Update()
    {
        Vector3 origin = new Vector3(0, mousePlaneHeight, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (LinePlaneIntersection(origin, Vector3.up, ray.origin, ray.direction, out Vector3 intersect))
        {
            MouseWorldPosition = intersect;
        }
        else
        {
            MouseWorldPosition = null;
        }
    }

    public static bool LinePlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint, Vector3 lineDirection, out Vector3 intersectPoint)
    {
        if (Vector3.Dot(planeNormal, lineDirection.normalized) == 0)
        {
            intersectPoint = Vector3.zero;
            return false;
        }

        float t = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint)) / Vector3.Dot(planeNormal, lineDirection.normalized);
        intersectPoint = linePoint + lineDirection.normalized * t;
        return true;
    }
}
