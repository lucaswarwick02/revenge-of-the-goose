using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private GameObject repeatingEnvironment;
    [SerializeField] private Transform environmentCheck;
    private float environmentCheckRadius = 1f;

    BossCombat bossCombat;
    BossManager bossManager;
    Transform player;

    private int numberOfEnvironments = 1;
    private float environmentOffset = 35.6f;

    private float loseDistance = 30f;

    private void Awake() {
        bossCombat = GetComponent<BossCombat>();
        bossManager = GetComponent<BossManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.InNeutralMode) return;

        bool doesEnvironmentExist = Physics.CheckSphere(environmentCheck.position, environmentCheckRadius, LayerMask.GetMask("Environment"));
        if (!doesEnvironmentExist) extendEnvironment();
        
        transform.position += Vector3.forward * Time.deltaTime * getSpeed();

        if (Vector3.Distance(transform.position, player.position) > loseDistance) {
            GameHandler.LoseBossFight(player);
        }
    }

    private void extendEnvironment () {
        if (GameHandler.InNeutralMode) return;

        Instantiate(repeatingEnvironment, new Vector3(0, 0, (numberOfEnvironments * 100) + environmentOffset), Quaternion.identity);
        numberOfEnvironments++;
    }

    private float getSpeed () {
        return GameHandler.InNeutralMode ? 0f : 3.5f;
    }
}
