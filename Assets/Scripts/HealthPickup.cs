using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private Transform player;
    private float speed = 2f;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;    
    }

    private void Update() {
        float distance = Vector3.Magnitude(player.position - transform.position);

        if (distance < 2.5f) {
            // Move towards player
            transform.position += (player.position - transform.position) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
