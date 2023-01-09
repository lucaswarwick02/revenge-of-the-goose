using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    private float jumpAngle = 50;
    private Rigidbody rb;

    private bool isJumping = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (isJumping) {
            isJumping = rb.velocity != Vector3.zero;
        }
        else {
            Jump();
        }
    }

    private void Jump()
    {
        isJumping = true;

        // Move towards player
        Vector3 difference = PlayerCompanions.INSTANCE.bunnySpot.position - transform.position;
        float angle = Mathf.Atan2(-difference.x, -difference.z) * Mathf.Rad2Deg;
        angle -= 180;

        float jumpSpeed = getCompanionSpeed(difference);
        rb.velocity = jumpSpeed * (Quaternion.Euler(0, angle, 0) * (Quaternion.Euler(-jumpAngle, 0, 0) * Vector3.forward));
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
