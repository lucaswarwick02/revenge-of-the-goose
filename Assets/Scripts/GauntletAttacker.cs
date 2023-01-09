using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GauntletAttacker : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackStrength;
    [SerializeField] private float lifetiime = 3;

    private Rigidbody rb;
    private Vector3 attackDir;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        attackDir = -transform.right;
        Destroy(gameObject, lifetiime);
    }

    private void Update()
    {
        rb.velocity = attackDir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerHealth.InflictDamage(damage, transform.position);
            other.transform.GetComponentInParent<PlayerMovement>().Knockback(Vector3.RotateTowards(Vector3.back, attackDir, Mathf.Deg2Rad * 10, 0), speed / 2, .2f);
        }
    }
}
