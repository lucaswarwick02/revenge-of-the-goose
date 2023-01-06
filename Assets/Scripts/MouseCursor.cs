using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private readonly Dictionary<string, MouseCursorInfo> cursorInstances = new();

    [SerializeField] private RectTransform canvas;
    [SerializeField] private MouseCursorInfo[] mouseCursors;

    private void OnEnable()
    {
        GameHandler.OnMapAreaChanged += OnMapAreaChanged;

        Cursor.visible = false;

        for (int i = 0; i < mouseCursors.Length; i++)
        {
            MouseCursorInfo cursor = mouseCursors[i];
            cursor.Instance = Instantiate(cursor.CursorPrefab, canvas).transform;
            cursorInstances.Add(cursor.CursorID, cursor);
        }
    }

    private void FixedUpdate()
    {
        if (!GameHandler.InNeutralArea)
        {
            for (int i = 0; i < cursorInstances.Values.Count; i++)
            {
                MouseCursorInfo cursor = cursorInstances.Values.ElementAt(i);

                if (cursor.UpdatePositionAutomatically)
                {
                    cursor.TargetPosition = Input.mousePosition;
                }

                cursor.Instance.position = Vector2.Lerp(cursor.Instance.position, cursor.TargetPosition, cursor.LerpSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDisable()
    {
        GameHandler.OnMapAreaChanged -= OnMapAreaChanged;
    }

    public void MoveCursorOverWorldPosition(string cursorID, Vector3 worldPosition)
    {
        MouseCursorInfo cursorInfo = cursorInstances[cursorID];
        cursorInfo.TargetPosition = Camera.main.WorldToScreenPoint(worldPosition);
        cursorInstances[cursorID] = cursorInfo;
    }

    private void OnMapAreaChanged(bool inNeutralArea)
    {
        if (inNeutralArea)
        {
            foreach (MouseCursorInfo cursor in cursorInstances.Values)
            {
                cursor.Instance.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (MouseCursorInfo cursor in cursorInstances.Values)
            {
                cursor.Instance.gameObject.SetActive(true);
            }
        }
    }

    [Serializable]
    public struct MouseCursorInfo
    {
        public string CursorID;
        public GameObject CursorPrefab;
        public bool UpdatePositionAutomatically;
        public float LerpSpeed;

        public Transform Instance { get; set; }

        public Vector2 TargetPosition { get; set; }
    }
}
