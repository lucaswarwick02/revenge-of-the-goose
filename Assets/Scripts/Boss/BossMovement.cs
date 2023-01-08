using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4f;

    [SerializeField] private GameObject repeatingEnvironment;

    private int numberOfEnvironments = 1;
    private float environmentOffset = 35.6f;

    // Start is called before the first frame update
    void Start()
    {
        float environmentDelay = (100 / speed) - 5f;
        float firstDelay = ((100 - 30) / speed) - 5f;
        InvokeRepeating("extendEnvironment", firstDelay, environmentDelay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }

    private void extendEnvironment () {
        Debug.Log("Extending environment");
        Instantiate(repeatingEnvironment, new Vector3(0, 0, (numberOfEnvironments * 100) + environmentOffset), Quaternion.identity);
        numberOfEnvironments++;
    }
}
