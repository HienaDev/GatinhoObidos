using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class Scaler : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 0.5f;
    public Ease easeType = Ease.OutBack;
    public bool playOnStart = true;

    private Vector3 _originalScale;

    [SerializeField] private bool deactiveOnKey = false;
    [SerializeField] private KeyCode keyToDeactivate = KeyCode.Tab;

    [SerializeField] private UnityEvent onScaleComplete;

    void Start()
    {
        // Store the original scale
        _originalScale = transform.localScale;
        if (playOnStart)
        {

           transform.localScale = Vector3.zero;
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
        transform.DOScale(_originalScale, duration).SetEase(easeType).SetDelay(2f);
    }

    public void ScaleDown()
    {
        transform.DOScale(Vector3.zero, duration).SetEase(easeType).OnComplete(() => onScaleComplete.Invoke());
    }

    private void Update()
    {
        if(deactiveOnKey && Input.GetKeyDown(keyToDeactivate))
        {
            ScaleDown();
        }
    }
}
