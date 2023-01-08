using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private GameObject bulletIcon1;
    [SerializeField] private GameObject bulletIcon2;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        PlayerCombat.OnReloadStarted += DoReloadAnimation;
        PlayerCombat.OnBulletFired += UpdateBulletBox;
        PlayerCombat.OnReloadEnded += UpdateBulletBox;
    }

    private void OnDisable()
    {
        PlayerCombat.OnReloadStarted -= DoReloadAnimation;
        PlayerCombat.OnBulletFired -= UpdateBulletBox;
        PlayerCombat.OnReloadEnded -= UpdateBulletBox;
    }

    private void DoReloadAnimation(int bulletsRemaining)
    {
        anim.SetTrigger("Reload");
    }

    private void UpdateBulletBox(int bulletsRemaining)
    {
        bulletIcon1.SetActive(bulletsRemaining > 0);
        bulletIcon2.SetActive(bulletsRemaining > 1);
    }
}
