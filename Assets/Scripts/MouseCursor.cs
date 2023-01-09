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
        GameHandler.OnNeutralModeChange += OnNeutralModeChanged;
        GameHandler.OnGameOver += Deactivate;
        GameHandler.OnGameLoaded += Activate;

        for (int i = 0; i < mouseCursors.Length; i++)
        {
            MouseCursorInfo cursor = mouseCursors[i];
            cursor.Instance = Instantiate(cursor.CursorPrefab, canvas).transform;
            cursorInstances.Add(cursor.CursorID, cursor);
        }
    }

    private void FixedUpdate()
    {
        if (!GameHandler.InNeutralMode)
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
        GameHandler.OnNeutralModeChange -= OnNeutralModeChanged;
        GameHandler.OnGameOver -= Deactivate;
        GameHandler.OnGameLoaded -= Activate;
    }

    public void MoveCursorOverWorldPosition(string cursorID, Vector3 worldPosition)
    {
        MouseCursorInfo cursorInfo = cursorInstances[cursorID];
        cursorInfo.TargetPosition = Camera.main.WorldToScreenPoint(worldPosition);
        cursorInstances[cursorID] = cursorInfo;
    }

    private void OnNeutralModeChanged(bool inNeutralMode)
    {
        foreach (MouseCursorInfo cursor in cursorInstances.Values)
        {
            cursor.Instance.gameObject.SetActive(inNeutralMode == cursor.usedInNeutralMode);
        }
    }

    private void Deactivate(Transform killer)
    {
        foreach (MouseCursorInfo cursor in cursorInstances.Values)
        {
            cursor.Instance.gameObject.SetActive(GameHandler.InNeutralMode == cursor.usedInNeutralMode);
        }
    }

    private void Activate()
    {
        foreach (MouseCursorInfo cursor in cursorInstances.Values)
        {
            cursor.Instance.gameObject.SetActive(GameHandler.InNeutralMode == cursor.usedInNeutralMode);
        }
    }

    [Serializable]
    public struct MouseCursorInfo
    {
        public string CursorID;
        public GameObject CursorPrefab;
        public bool usedInNeutralMode;
        public bool UpdatePositionAutomatically;
        public float LerpSpeed;

        public Transform Instance { get; set; }

        public Vector2 TargetPosition { get; set; }
    }
}
