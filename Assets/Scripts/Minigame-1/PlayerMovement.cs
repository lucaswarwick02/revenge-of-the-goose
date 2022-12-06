using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform mask;

    [SerializeField] private SpriteRenderer arms;

    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(h, 0, v);
        velocity.Normalize();

        rb.velocity = velocity * speed;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (LinePlaneIntersection(transform.position, new Vector3(0, Camera.main.transform.position.y - transform.position.y, Camera.main.transform.position.z - transform.position.z), ray.origin, ray.direction, out Vector3 intersect))
        {
            Vector3 gunDir = intersect - transform.position;
            arms.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(Vector3.Angle(Vector3.right, gunDir) - 90, -45, 45));
        }
        else
        {
            arms.transform.localRotation = Quaternion.identity;
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
