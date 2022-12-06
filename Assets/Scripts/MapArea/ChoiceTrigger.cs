using System;
using UnityEngine;

public class ChoiceTrigger : MonoBehaviour
{
    public Action OnPlayerEnteredTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnteredTrigger?.Invoke();
        }
    }
}
