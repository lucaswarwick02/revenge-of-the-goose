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
        GameObject effectGO = Instantiate(effect, hitInfo.point, Quaternion.Euler(hitInfo.normal), transform).gameObject;
    }
}
