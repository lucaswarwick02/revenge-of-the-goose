using UnityEngine;

public class HUDController : MonoBehaviour
{
    private void OnEnable()
    {
        GameHandler.OnGameOver += Deactivate;
    }

    private void OnDisable()
    {
        GameHandler.OnGameOver -= Deactivate;
    }

    private void Deactivate(Transform killer)
    {
        gameObject.SetActive(false);
    }
}
