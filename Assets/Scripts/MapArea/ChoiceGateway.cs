using System;
using UnityEngine;

public class ChoiceGateway : MonoBehaviour
{
    public Action OnPlayerEnteredGateway;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnteredGateway?.Invoke();
        }
    }
}
