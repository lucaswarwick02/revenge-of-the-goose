using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    private static readonly Dictionary<int, string> LABELS = new () { { 0, "A." }, { 1, "B." }, { 2, "C." }, { 3, "D." } };
    
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text text;

    public void SetText(int choiceOrdering, string text)
    {
        label.text = LABELS[choiceOrdering];
        this.text.text = text;
    }
}
