using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionFollow : MonoBehaviour
{

    private float jumpAngle = 35;
    private Rigidbody rb;

    private bool isJumping = false;
    private Vector3 difference;
    private float distance;

    private Transform followTarget;
    private Transform image;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        image = GetComponentInChildren<SpriteRenderer>().transform;
    }

    private void OnEnable() {
        GameHandler.OnMapAreaChanged += MoveToFollowTarget;
    }

    private void OnDisable() {
        GameHandler.OnMapAreaChanged -= MoveToFollowTarget;
    }

    private void Update() {
        if (followTarget == null) {
            Debug.Log(gameObject.name);
        }
        difference = followTarget.position - transform.position;
        distance = Vector3.Magnitude(difference);

        if (distance < 0.65f) return;

        if (isJumping) {
            isJumping = Vector3.Magnitude(rb.velocity) > 0.75f;
        }
        else {
            isJumping = true;
            Jump();
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

    private void MoveToFollowTarget () {
        transform.position = followTarget.position;
    }

    public void SetFollowTarget (Transform target) {
        followTarget = target;
    }

    private void Jump()
    {
        // Move towards player
        float angle = Mathf.Atan2(-difference.x, -difference.z) * Mathf.Rad2Deg;
        angle -= 180;

        float jumpSpeed = getCompanionSpeed(difference);
        rb.velocity = jumpSpeed * (Quaternion.Euler(0, angle, 0) * (Quaternion.Euler(-jumpAngle, 0, 0) * Vector3.forward));
    }

    private float getCompanionSpeed (Vector3 difference) {
        float distance = Vector3.Magnitude(difference);
        if (distance > 10) {
            // High speed
            return 10f;
        }
        else if (distance < 2) {
            // Low speed
            return 3f;
        }
        else {
            // Normal speed
            return 7f;
        }
    }
}
