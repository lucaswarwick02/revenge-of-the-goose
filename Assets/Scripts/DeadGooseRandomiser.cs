using UnityEngine;

public class DeadGooseRandomiser : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gooseSR;
    [SerializeField] private SpriteRenderer bloodSR;
    [Space]
    [SerializeField] private Sprite[] gooseSprites;
    [SerializeField] private Sprite[] bloodSprites;
    [SerializeField] private float gooseMinSize;
    [SerializeField] private float gooseMaxSize;
    [SerializeField] private float bloodMinSize;
    [SerializeField] private float bloodMaxSize;
    [SerializeField] private Vector3 bloodMinPos;
    [SerializeField] private Vector3 bloodMaxPos;

    private void Awake()
    {
        gooseSR.sprite = gooseSprites[UnityEngine.Random.Range(0, gooseSprites.Length)];
        bloodSR.sprite = bloodSprites[UnityEngine.Random.Range(0, bloodSprites.Length)];

        gooseSR.transform.localScale = Vector3.one * (UnityEngine.Random.value * (gooseMaxSize - gooseMinSize) + gooseMinSize);
        bloodSR.transform.localScale = Vector3.one * (UnityEngine.Random.value * (bloodMaxSize - bloodMinSize) + bloodMinSize);

        bloodSR.transform.Rotate(Vector3.forward, 360 * UnityEngine.Random.value);
        bloodSR.transform.localPosition = (UnityEngine.Random.value * (bloodMaxPos - bloodMinPos) + bloodMinPos);

        transform.localScale = UnityEngine.Random.value <= .5 ? transform.localScale : new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
