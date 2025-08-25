using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.Examples.ObjectSpin;

public enum ActionWord
{
    right,
    left,
    jump,
    climb,
    interact,
    hide,
    scratch,
    sleep,
}

[System.Serializable]
public class ActionTypeEvent : UnityEngine.Events.UnityEvent<ActionWord> { }


[Serializable]
public class PathPoint
{
    public bool activateDependentDecisions = false;
    public AnimationClip animation;
    public Transform point;
    public bool hasEvent;
    public UnityEvent onReachPoint;
}

[Serializable]
public class Decision
{
    public bool active = true;
    public ActionPoint nextActionPoint;
    public ActionWord action;

    public DependentDecision[] dependentDecisions;

    [ReorderableList]
    public PathPoint[] path;

}

[Serializable]
public class DependentDecision
{
    public ActionPoint actionPoint;
    public ActionWord action;
}

public class ActionPoint : MonoBehaviour
{

    [Header("Action Point")]
    [SerializeField] private Decision[] possibleActions;
    public Decision[] PossibleActions => possibleActions;

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

    public void ActivateInteraction(ActionWord action)
    {

        foreach (Decision decision in possibleActions)
        {
            if (!decision.active && decision.action == action)
            {
                decision.active = true;
            }
        }
    }

}
