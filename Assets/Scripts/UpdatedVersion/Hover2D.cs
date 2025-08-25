using UnityEngine;
using DG.Tweening;

public class Hover2D : MonoBehaviour
{
    [SerializeField] private float hoverAmount = 0.5f;   // How high/low it moves
    [SerializeField] private float duration = 1f;        // Time to go up or down
    [SerializeField] private Ease easeType = Ease.InOutSine; // Exposed in inspector
    [SerializeField] private bool startImmediately = true;

    private Vector3 startPosition;
    private Tweener hoverTweener;

    void Start()
    {
        startPosition = transform.position;

        if (startImmediately)
            StartHover();
    }

    public void StartHover()
    {
        // Kill any existing tween to avoid duplicates
        hoverTweener?.Kill();

        // Tween up and down infinitely
        hoverTweener = transform.DOMoveY(startPosition.y + hoverAmount, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(easeType); // Use inspector-selected ease
    }

    public void StopHover()
    {
        hoverTweener?.Kill();
        transform.position = startPosition;
    }
}
