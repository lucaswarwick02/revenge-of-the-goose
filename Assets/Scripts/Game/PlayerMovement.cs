using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Vector3 Position { get; private set; }

    public float speed = 5f;

    private Rigidbody rb;
    private Animator animator;

    private bool beingKnockedBack;
    private float knockbackFinishTime;
    private Vector3 knockbackVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        GameHandler.OnMapAreaChanged += OnMapAreaChanged;
        GameHandler.OnNeutralModeChange += OnNeutralModeChanged;
    }

    private void OnDisable()
    {
        GameHandler.OnMapAreaChanged -= OnMapAreaChanged;
        GameHandler.OnNeutralModeChange -= OnNeutralModeChanged;
    }

    private void Update()
    {
        Position = transform.position;
    }

    private void FixedUpdate() {

        if (GameHandler.IsPaused)
        {
            return;
        }

        if (beingKnockedBack)
        {
            if (Time.time >= knockbackFinishTime)
            {
                beingKnockedBack = false;
            }
            rb.velocity = knockbackVector;
            animator.SetFloat("Speed", 0);
        }
        else
        {
            Vector3 velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            rb.velocity = velocity.normalized * speed;

            animator.SetFloat("Speed", rb.velocity.magnitude);
        }
    }

    public void Knockback(Vector3 dir, float speed, float duration)
    {
        beingKnockedBack = true;
        knockbackVector = dir * speed;
        Debug.Log("Knockback: " + knockbackVector);
        knockbackFinishTime = Time.time + duration;
    }

    private void OnMapAreaChanged()
    {
        transform.position = Vector3.zero;
    }

    private void OnNeutralModeChanged(bool inNeutralMode)
    {
        animator.SetBool("Neutral", inNeutralMode);
    }
}
