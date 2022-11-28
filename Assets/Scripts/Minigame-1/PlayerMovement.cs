using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform mask;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 velocity = new Vector2(h, v);
        velocity.Normalize();

        rb.velocity = velocity * speed;

        if (velocity.x != 0f) {
            bool isLeft = velocity.x < 0f;
            spriteRenderer.flipX = isLeft;
            mask.rotation = Quaternion.Euler(0f, isLeft ? 180f : 0f, 0f);
        }
    }
}
