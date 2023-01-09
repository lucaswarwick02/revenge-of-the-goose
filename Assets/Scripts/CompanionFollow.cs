using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    private float jumpAngle = 50;
    private Rigidbody rb;

    private bool isJumping = false;
    private Vector3 difference;
    private float distance;

    private Transform followTarget;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        GameHandler.OnMapAreaChanged += MoveToFollowTarget;
    }

    private void OnDisable() {
        GameHandler.OnMapAreaChanged -= MoveToFollowTarget;
    }

    private void Update() {
        difference = followTarget.position - transform.position;
        distance = Vector3.Magnitude(difference);

        if (distance < 0.65f) return;

        if (isJumping) {
            isJumping = rb.velocity != Vector3.zero;
        }
        else {
            isJumping = true;
            Jump();
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
        else if (distance < 1) {
            // Super low speed
            return 2f;
        }
        else if (distance < 2) {
            // Low speed
            return 3f;
        }
        else {
            // Normal speed
            return 5f;
        }
    }
}
