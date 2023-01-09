using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GauntletAttacker : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackStrength;
    [SerializeField] private float lifetime = 3;
    [SerializeField] private bool controlImageFlip = false;
    [SerializeField] private Transform image;

    private Rigidbody rb;
    private Vector3 attackDir;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        attackDir = -transform.parent.right;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        rb.velocity = attackDir * speed;

        if (controlImageFlip)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerHealth.InflictDamage(damage, transform);
            other.transform.GetComponentInParent<PlayerMovement>().Knockback(Vector3.RotateTowards(Vector3.back, attackDir, Mathf.Deg2Rad * 10, 0), speed / 2, .2f);
        }
    }
}
