using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class SheepController : MonoBehaviour
{
    [SerializeField] private Transform image;
    [Space]
    [SerializeField] private BoxCollider obstacleCollider;
    [SerializeField] private BoxCollider imageCollider;

    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private bool makeSounds;
    [SerializeField] private AudioSource sheepSound;
    private float minSoundDelay = 3f;
    private float maxSoundDelay = 6f;
    private float currentSoundDelay = 0f;

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
            if (rb.velocity.x > 0.01f)
            {
                image.localScale = new Vector3(-1, 1, 1);
            }
            else if (rb.velocity.x < -0.01f)
            {
                image.localScale = new Vector3(1, 1, 1);
            }

            if (makeSounds && (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= 7.5f)) {
                currentSoundDelay -= Time.deltaTime;
                if (currentSoundDelay < 0) {
                    SheepSound();
                    currentSoundDelay = Random.Range(minSoundDelay, maxSoundDelay);
                }
            }
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

    public void SheepSound() {
        sheepSound.Play();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void MakeCompanion()
    {
        PlaythroughStats.UnlockSheepCompanion();

        transform.parent = null;
        obstacleCollider.enabled = false;
        imageCollider.gameObject.layer = 1 << LayerMask.GetMask("Companion");

        GetComponent<Destructible>().enabled = false;
        GetComponent<Converser>().enabled = false;
        GetComponent<CompanionFollow>().enabled = true;
        GetComponent<CompanionFollow>().SetFollowTarget(PlayerCompanions.RegisterAsSheepCompanion(gameObject));

        enabled = false;
    }
}
