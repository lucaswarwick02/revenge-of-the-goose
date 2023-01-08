using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private GameObject repeatingEnvironment;

    BossCombat bossCombat;
    BossManager bossManager;
    Transform player;

    private int numberOfEnvironments = 1;
    private float environmentOffset = 35.6f;

    private void Awake() {
        bossCombat = GetComponent<BossCombat>();
        bossManager = GetComponent<BossManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        float environmentDelay = 25f;
        InvokeRepeating("extendEnvironment", 0f, environmentDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.InNeutralMode) return;
        
        transform.position += Vector3.forward * Time.deltaTime * getSpeed();
    }

    private void extendEnvironment () {
        if (GameHandler.InNeutralMode) return;

        Instantiate(repeatingEnvironment, new Vector3(0, 0, (numberOfEnvironments * 100) + environmentOffset), Quaternion.identity);
        numberOfEnvironments++;
    }

    private float getSpeed () {
        if (GameHandler.InNeutralMode) return 0f;

        float distance = Vector3.Distance(transform.position, player.position);

        bossCombat.canAttack = distance < 20f;

        if (distance > 20) {
            // Off the map
            return 0f;
        }
        else if (distance > 10.5f) {
            // Too far away
            return 3f;
        }
        else if (distance < 5) {
            // Too close
            return 7f;
        }
        else {
            // Perfect distance
            return 5f;
        }
    }
}
