namespace Game.Utility
{
    using UnityEngine;

    public static class VectorMaths
    {
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
}
