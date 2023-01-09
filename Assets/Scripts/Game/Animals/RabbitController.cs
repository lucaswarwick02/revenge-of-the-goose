using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class RabbitController : MonoBehaviour
{
    [SerializeField] private bool jumpingIsActive = true;
    [SerializeField] private float minJumpInterval = 2;
    [SerializeField] private float maxJumpInterval = 6;
    [SerializeField] private float minJumpSpeed = 2;
    [SerializeField] private float maxJumpSpeed = 3;
    [SerializeField] private float jumpAngle = 50;
    [SerializeField] private Transform image;

    private Rigidbody rb;
    private Animator anim;
    
    private float nextJumpTime;

    public bool IsDead { get; set; }

    public bool IsCompanion { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (jumpingIsActive && Time.time > nextJumpTime)
            {
                Vector3 difference = PlayerCompanions.INSTANCE.bunnySpot.position - transform.position;
                if (!(IsCompanion && Vector3.Magnitude(difference) < 1f)) {
                    nextJumpTime = Time.time + UnityEngine.Random.value * (maxJumpInterval - minJumpInterval) + minJumpInterval;
                    StartJump();
                }
            }

            if (rb.velocity.x > 0.01f)
            {
                image.localScale = new Vector3(-1, 1, 1);
            }
            else if (rb.velocity.x < -0.01f)
            {
                image.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void StartJump()
    {
        anim.SetTrigger("Jump");
    }

    private void Jump()
    {
        if (IsCompanion) {
            // Move towards player
            Vector3 difference = PlayerCompanions.INSTANCE.bunnySpot.position - transform.position;
            float angle = Mathf.Atan2(-difference.x, -difference.z) * Mathf.Rad2Deg;
            angle -= 180;

            float jumpSpeed = getCompanionSpeed(difference);
            rb.velocity = jumpSpeed * (Quaternion.Euler(0, angle, 0) * (Quaternion.Euler(-jumpAngle, 0, 0) * Vector3.forward));
        }
        else {
            float jumpSpeed = UnityEngine.Random.value * (maxJumpSpeed - minJumpSpeed) + minJumpSpeed;
            rb.velocity = jumpSpeed * (Quaternion.Euler(0, UnityEngine.Random.value * 360, 0) * (Quaternion.Euler(-jumpAngle, 0, 0) * Vector3.forward));
        }
    }

    public void StartDeath(RaycastHit hitInfo, Vector3 origin)
    {
        IsDead = true;
        anim.SetTrigger("Death");

        Vector3 impactDir = Vector3.RotateTowards((hitInfo.point - origin).normalized, Vector3.up, Mathf.Deg2Rad * 30, 0);
        rb.velocity = impactDir * 4;

        enabled = false;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void StandGround()
    {
        rb.mass = 100000000;
        jumpingIsActive = false;
    }

    public void StopStandingGround()
    {
        rb.mass = 1;
        jumpingIsActive = true;
    }

    public void UnlockBunnyCompanion () {
        PlaythroughStats.UnlockBunnyCompanion();
    }

    public void MakeCompanion () {
        Destroy(GetComponent<Destructible>());
        Destroy(GetComponent<AnimalCommenter>());
        Destroy(GetComponent<BoxCollider>());

        transform.parent = null;
        IsCompanion = true;
        PlayerCompanions.INSTANCE.bunnyCompanion = gameObject;

        // Set movement speed to keep up with goose speed
        minJumpInterval = 0f;
        maxJumpInterval = 0f;

        anim.speed = 4f;
    }

    private float getCompanionSpeed (Vector3 difference) {
        float distance = Vector3.Magnitude(difference);
        if (distance > 10) {
            // High speed
            return 6f;
        }
        else if (distance < 1) {
            // Low speed;
            return 2f;
        }
        else {
            // Normal speed
            return 4f;
        }
    }
}
