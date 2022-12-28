using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreateEffectOnMesh : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] effects;
    [SerializeField] private UnityAction trigger;

    public void TriggerEffect(RaycastHit hitInfo)
    {
        foreach (ParticleSystem effect in effects)
        {
            if (effect.main.simulationSpace == ParticleSystemSimulationSpace.Local)
            {
                Instantiate(effect, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal), transform);
            }
            else
            {
                Instantiate(effect, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
            }
        }
    }
}
