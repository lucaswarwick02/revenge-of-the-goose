using TMPro;
using UnityEngine;

[RequireComponent(typeof(Destructible))]
public class AnimalCommenter : MonoBehaviour
{
    [SerializeField] private Vector3 commentPosition;
    [SerializeField] private GameObject commentPrefab;

    private Canvas canvas;
    private GameObject currentComment;

    public void Comment(string text)
    {
        if (currentComment != null)
        {
            Debug.LogWarning("Can't create comment as one was already present");
            return;
        }

        currentComment = Instantiate(commentPrefab, canvas.transform.Find("CommentHolder").transform);

        TMP_Text textElem = currentComment.GetComponentInChildren<TMP_Text>();
        textElem.text = text;

        if ((transform.position + commentPosition).x < 0)
        {
            currentComment.transform.localScale = new Vector3(-1, 1, 1);
            textElem.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Debug.Log("Will last " + currentComment.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(currentComment, currentComment.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    private void Awake()
    {
        // disable this script when the animal dies
        GetComponent<Destructible>().OnDestroyed.AddListener(DisableSafely);
        canvas = FindObjectOfType<Canvas>();
    }
    private void OnEnable() {
        AnimalCommentController.animalCommenters.Add(this);
    }

    private void OnDisable()
    {
        GetComponent<Destructible>().OnDestroyed.RemoveListener(DisableSafely);
        AnimalCommentController.animalCommenters.Remove(this);
    }

    private void Update()
    {
        if (currentComment != null)
        {
            currentComment.transform.position = Camera.main.WorldToScreenPoint(transform.position + commentPosition);
        }
    }

    private void DisableSafely(RaycastHit hit, Vector3 pos)
    {
        if (currentComment != null)
        {
            Destroy(currentComment);
        }

        enabled = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + commentPosition, 0.05f);
    }
#endif
}
