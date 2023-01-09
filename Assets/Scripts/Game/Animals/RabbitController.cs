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
                nextJumpTime = Time.time + UnityEngine.Random.value * (maxJumpInterval - minJumpInterval) + minJumpInterval;
                StartJump();
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
        float jumpSpeed = UnityEngine.Random.value * (maxJumpSpeed - minJumpSpeed) + minJumpSpeed;
        rb.velocity = jumpSpeed * (Quaternion.Euler(0, UnityEngine.Random.value * 360, 0) * (Quaternion.Euler(-jumpAngle, 0, 0) * Vector3.forward));
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

    public void MakeCompanion () {
        if (GetComponent<CompanionFollow>()) return;
        
        PlaythroughStats.UnlockBunnyCompanion();
        
        Destroy(GetComponent<Destructible>());
        Destroy(GetComponent<AnimalCommenter>());
        Destroy(GetComponent<BoxCollider>());
        transform.parent = null;

        gameObject.AddComponent<CompanionFollow>().SetFollowTarget(PlayerCompanions.GetBunnySpot());

        Destroy(this);
    }
}
