using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject HUD;

    private void OnEnable()
    {
        GameHandler.OnGameOver += Deactivate;
        GameHandler.OnGameLoaded += Activate;
    }

    private void OnDisable()
    {
        GameHandler.OnGameOver -= Deactivate;
        GameHandler.OnGameLoaded -= Activate;
    }

    private void Deactivate(Transform killer)
    {
        HUD.SetActive(false);
    }

    private void Activate()
    {
        HUD.SetActive(true);
    }
}
