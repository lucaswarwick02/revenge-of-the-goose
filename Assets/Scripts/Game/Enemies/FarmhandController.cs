using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FarmhandController : MonoBehaviour
{
    [SerializeField] private float attackRange = 15;
    [SerializeField] private float attackInterval = 3;
    [SerializeField] private float startAttackDelay = 1.5f;
    [SerializeField] private float attackDamage = 20;

    [SerializeField] private GameObject healthPickupPrefab;

    private Rigidbody rb;
    private Animator anim;
    private AudioSource audioSource;

    private Transform player;
    private float nextAttackTime;
    private bool seenPlayerForFirstTime = false;

    public bool IsDead { get; set; }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (InAttackRangeOfPlayer() && Time.time >= nextAttackTime)
            {
                if (!seenPlayerForFirstTime)
                {
                    seenPlayerForFirstTime = true;
                    nextAttackTime = Time.time + startAttackDelay;
                    anim.SetTrigger("Alert");
                }
                else
                {
                    nextAttackTime = Time.time + attackInterval;
                    AttackPlayer();
                }
            }
        }
    }

    private bool InAttackRangeOfPlayer()
    {
        return player.transform.position.z < transform.position.z && (player.transform.position - transform.position).magnitude <= attackRange;
    }

    private void AttackPlayer()
    {
        audioSource.Play();
        anim.SetTrigger("Attack");
        PlayerHealth.InflictDamage(attackDamage, transform);
    }

    public void StartDeath(RaycastHit hitInfo, Vector3 origin)
    {
        IsDead = true;
        anim.SetTrigger("Death");

        Vector3 impactDir = Vector3.RotateTowards((hitInfo.point - origin).normalized, Vector3.up, Mathf.Deg2Rad * 30, 0);
        rb.velocity = impactDir * 4;

        if (healthPickupPrefab) Instantiate(healthPickupPrefab, transform.position, Quaternion.identity, GameHandler.CurrentMapAreaInstance.transform);

        enabled = false;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
