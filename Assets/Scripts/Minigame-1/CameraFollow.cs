using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float maxY;
    public float minY;

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Clamp(player.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
