using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class RabbitController : MonoBehaviour
{
    [SerializeField] private float minJumpInterval = 2;
    [SerializeField] private float maxJumpInterval = 6;
    [SerializeField] private float minJumpSpeed = 2;
    [SerializeField] private float maxJumpSpeed = 3;
    [SerializeField] private float jumpAngle = 50;
    [SerializeField] private Transform image;

    private Rigidbody rb;
    private Animator anim;

    private float nextJumpTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.time > nextJumpTime)
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

    private void StartJump()
    {
        anim.SetTrigger("Jump");
    }

    private void Jump()
    {
        float jumpSpeed = UnityEngine.Random.value * (maxJumpSpeed - minJumpSpeed) + minJumpSpeed;
        rb.velocity = jumpSpeed * (Quaternion.Euler(0, UnityEngine.Random.value * 360, 0) * (Quaternion.Euler(-jumpAngle, 0, 0) * Vector3.forward));
    }
}
