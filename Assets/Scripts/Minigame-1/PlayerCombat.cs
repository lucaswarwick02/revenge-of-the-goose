using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public SpriteRenderer muzzleFlash;

    public float shootDelay = 0.25f;
    float shootTimer = 0f;

    public float flashDelay = 0.15f;
    float flashTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && shootTimer <= 0f) {
            // Show the muzzle flash
            muzzleFlash.enabled = true;

            // Reset the shoot delay timer
            shootTimer = shootDelay;
            flashTimer = flashDelay;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.collider.name);            
            }
        }

        shootTimer = Mathf.Clamp(shootTimer -= Time.deltaTime, 0f, shootDelay);
        flashTimer = Mathf.Clamp(flashTimer - Time.deltaTime, 0f, flashDelay);

        if (flashTimer <= 0f && muzzleFlash.enabled) {
            muzzleFlash.enabled = false;
        }
    }
}
