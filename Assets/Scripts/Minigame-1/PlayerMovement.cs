using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        MapArea.OnNextMapAreaChosen += OnNewArea;
    }

    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(h, 0, v);
        velocity.Normalize();

        rb.velocity = velocity * speed;

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void OnNewArea(MapArea area)
    {
        transform.position = Vector3.zero;
    }
}
