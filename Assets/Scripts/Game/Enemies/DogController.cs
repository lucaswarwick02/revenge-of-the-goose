using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DogController : MonoBehaviour
{
    private const float RAYCAST_PLANE_HEIGHT = 0.25f;

    [SerializeField] private float speed;
    [SerializeField] private float maxSightRange = 10;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float attackInterval = 3;
    [SerializeField] private float attackDamage = 20;

    private Rigidbody rb;
    private Animator anim;

    private Transform player;
    private float nextAttackTime;

    public bool IsDead { get; set; }

    [SerializeField] private AudioSource barkSound;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (CanSeePlayer(out Vector3 dogToPlayerVec) && dogToPlayerVec.magnitude > attackRange)
            {
                rb.velocity = dogToPlayerVec.normalized * speed;
                anim.SetBool("IsWalking", true);
            }
            else
            {
                rb.velocity = Vector3.zero;
                anim.SetBool("IsWalking", false);
            }

            if (dogToPlayerVec.magnitude <= attackRange && Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackInterval;
                AttackPlayer();
            }
        }
    }

    private bool CanSeePlayer(out Vector3 dogToPlayerVec)
    {
        Vector3 origin = new Vector3(transform.position.x, RAYCAST_PLANE_HEIGHT, transform.position.z);
        Vector3 playerPos = new Vector3(player.position.x, RAYCAST_PLANE_HEIGHT, player.position.z);
        dogToPlayerVec = playerPos - origin;
        return Physics.Raycast(origin, dogToPlayerVec, out RaycastHit hit, maxSightRange) && hit.collider.transform.CompareTag("Player");
    }

    private void AttackPlayer()
    {
        anim.SetTrigger("Attack");
        PlayerHealth.InflictDamage(attackDamage, transform);
    }

    public void Bark()
    {
        // make bark noise
        barkSound.Play();
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
}
