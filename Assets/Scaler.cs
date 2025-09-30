using UnityEngine;
using DG.Tweening;

public class Scaler : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 0.5f;
    public Ease easeType = Ease.OutBack;
    public bool playOnStart = true;

    private Vector3 _originalScale;

    void Awake()
    {
        // Store the original scale
        _originalScale = transform.localScale;

        // Reset to zero if it should play on start
        if (playOnStart)
            transform.localScale = Vector3.zero;
    }

    void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    /// <summary>
    /// Plays the scale animation from 0 to the original scale.
    /// </summary>
    public void Play()
    {
        // Ensure scale is zero before animating
        transform.localScale = Vector3.zero;

        // Animate to original scale
        transform.DOScale(_originalScale, duration).SetEase(easeType);
    }
}
