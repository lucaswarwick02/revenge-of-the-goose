using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreateEffectOnMesh : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private UnityAction trigger;

    public void TriggerEffect(RaycastHit hitInfo)
    {
        Instantiate(effect, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
    }
}
