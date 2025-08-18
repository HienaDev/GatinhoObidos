using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CatState : MonoBehaviour
{
    [SerializeField] private ActionPoint startingActionPoint;
    private ActionPoint currentActionPoint;

    private Animator animator;

    [SerializeField] private float speed = 5f;
    private bool moving;

    [SerializeField] private AnimationClip idleAnimation;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currentActionPoint = startingActionPoint;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckInput(string input)
    {
        if (moving)
        {
            Debug.LogWarning("Cat is currently moving. Please wait until the action is completed.");
            return;
        }

        if (currentActionPoint == null)
        {
            Debug.LogWarning("No current action point set.");
            return;
        }

        // Check if the input matches any of the possible actions in the current action point
        foreach (Decision action in currentActionPoint.PossibleActions)
        {
            string actionName = action.action.ToString();
            if (Settings.GetText(actionName).Equals(input, System.StringComparison.OrdinalIgnoreCase) && action.active)
            {
                Debug.Log("Found interaction: " + actionName);

                // Perform the action
                PerformAction(action);

                return;
            }
        }
        Debug.Log("No matching action found for input: " + input);
    }



    public async void PerformAction(Decision action)
    {
        moving = true;

        for (int i = 0; i < action.path.Length; i++)
        {
            PathPoint point = action.path[i];
            Transform target = point.point;

            // --- Play animation for this path point ---
            if (point.animation != null)
                animator.Play(point.animation.name);

            // --- Horizontal facing via SpriteRenderer flip ---
            bool movingLeft = transform.position.x - target.position.x > 0;
            spriteRenderer.flipX = movingLeft;

            // --- Head tilt (Z rotation) ---
            Vector3 direction = (target.position - transform.position).normalized;
            float angleZ = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;
            if (movingLeft) angleZ = -angleZ;

            // Snap Z rotation instantly
            transform.rotation = Quaternion.Euler(0f, 0f, angleZ);

            // --- Move at constant speed ---
            await transform.DOMove(target.position, speed)
                           .SetSpeedBased()
                           .SetEase(Ease.Linear)
                           .AsyncWaitForCompletion();

            // Ensure 0 0 0 rotation
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            // --- Invoke UnityEvent ---
            if (point.hasEvent && point.onReachPoint != null)
                point.onReachPoint.Invoke();

            // --- Activate dependent decisions ---
            if (point.activateDependentDecisions && action.dependentDecisions != null)
            {
                foreach (DependentDecision dep in action.dependentDecisions)
                {
                    if (dep.actionPoint != null)
                        dep.actionPoint.ActivateInteraction(dep.action);
                }
            }
        }

        // Advance to next action point
        currentActionPoint = action.nextActionPoint;
        moving = false;

        // --- Play idle animation after movement ---
        if (idleAnimation != null)
            animator.Play(idleAnimation.name);
    }

}
