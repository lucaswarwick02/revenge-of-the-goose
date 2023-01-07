using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 Position { get; private set; }

    public float speed = 5f;

    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        GameHandler.OnMapAreaChanged += OnMapAreaChanged;
    }

    private void OnDisable()
    {
        GameHandler.OnMapAreaChanged -= OnMapAreaChanged;
    }

    private void Update()
    {
        Position = transform.position;
    }

    private void FixedUpdate() {
        if (GameHandler.IsPaused) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(h, 0, v);
        velocity.Normalize();

        rb.velocity = velocity * speed;

        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void OnMapAreaChanged(bool inNeutralArea)
    {
        transform.position = Vector3.zero;
        animator.SetBool("Neutral", inNeutralArea);
    }
}
