using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


public enum ActionWord
{
    Right,
    Left,
    Jump,
    Interact,
}

public enum ActionType
{
    Movement,
    Interaction,
}

[Serializable]
public struct Decision
{
    public ActionWord action;
    public ActionType actionType;

    public Transform[] path;

    [ShowIf("actionType", ActionType.Interaction)]
    public UnityEvent interaction;

}

    public class ActionPoint : MonoBehaviour
{

    [Header("Action Point")]
    [SerializeField] private Decision[] possibleActions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Header("Gizmos")]
    public float lineWidth = 0.2f;
    public float lineHeight = 0.5f;
    public Color lineColor = Color.red;

    void OnDrawGizmos()
    {

        Gizmos.color = lineColor;
        Gizmos.DrawCube(transform.position + new Vector3(0f, lineHeight, 0f), new Vector3(lineWidth, lineHeight * 2, lineWidth));

        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(lineWidth, lineWidth, lineWidth));
    }
}
