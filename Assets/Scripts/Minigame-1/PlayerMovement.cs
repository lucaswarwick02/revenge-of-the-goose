using UnityEngine;

[RequireComponent(typeof(MouseTracker))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform mask;

    //[SerializeField] private SpriteRenderer arms;

    private Rigidbody rb;
    private MouseTracker mouseTracker;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mouseTracker = GetComponent<MouseTracker>();
    }

    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(h, 0, v);
        velocity.Normalize();

        rb.velocity = velocity * speed;

        //if (mouseTracker.MouseWorldPosition.HasValue)
        //{
        //    Vector3 gunDir = new Vector3(mouseTracker.MouseWorldPosition.Value.x, 0, mouseTracker.MouseWorldPosition.Value.z) - transform.position;
        //    arms.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(Vector3.Angle(Vector3.right, gunDir) - 90, -45, 45));
        //}
        //else
        //{
        //    arms.transform.localRotation = Quaternion.identity;
        //}
    }
}
