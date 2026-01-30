using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterPickUp : MonoBehaviour
{

    private ActionWord actionWord;
    private int index;

    private LevelManager levelManager;

    public TextMeshProUGUI text;
    public TextMeshProUGUI textBackground;
    [SerializeField] private float rotateDuration = 0.2f; // fast rotation time
    [SerializeField] private float popScaleMultiplier = 2f;
    [SerializeField] private float pauseDuration = 0.5f;
    [SerializeField] private float shrinkDuration = 0.3f;
    private Transform targetTransform; // Optional target to move towards
    //private Vector3 initialScale;

    [SerializeField] private ParticleSystem collectEffect;

    [SerializeField] private AudioSource collectSound;
    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] private AudioSource letterSlideSource;
    public void Instantiate(ActionWord actionWord, int index, LevelManager levelManager, Transform targetTransform)
    {
        this.actionWord = actionWord;
        this.index = index;
        this.levelManager = levelManager;
        this.targetTransform = targetTransform;


        UpdateText();
    }

    void UpdateText()
    {
        Debug.Log(actionWord.ToString() + " :Updating letter text for index: " + index );// + Settings.GetText(actionWord.ToString())[index]);
        text.text = Settings.GetText(actionWord.ToString())[index].ToString();
        textBackground.text = text.text;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (levelManager != null)
            {
                PlayTween(text.transform, targetTransform);
                PlayTween(textBackground.transform, targetTransform);
                collectEffect.gameObject.SetActive(true);
                collectEffect.Play();
                Collider2D collider2D = GetComponent<Collider2D>();

                if (collectSound != null && audioClips.Length > 0)
                {
                    collectSound.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
                }

                if (collider2D != null)
                    collider2D.enabled = false; // Disable further collisions
            }
            else
            {
                Debug.LogError("LevelManager not found in the scene.");
            }
        }
    }

    private void PlayTween(Transform objectToAnimate, Transform targetTransform = null)
    {
        Vector3 initialScale = objectToAnimate.localScale;
        Vector3 initialPosition = objectToAnimate.position;

        // Step 1: Pop up in scale and rotate fast
        objectToAnimate
            .DOScale(initialScale * popScaleMultiplier, rotateDuration)
            .SetEase(Ease.OutBack);

        objectToAnimate
            .DORotate(new Vector3(0, 360, 0), rotateDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // Step 2: Pause
                DOVirtual.DelayedCall(pauseDuration, () =>
                {
                    letterSlideSource.Play();
                    // Step 3: Rotate again while shrinking to 0 and moving to target (if any)
                    Tween scaleTween = objectToAnimate
                        .DOScale(Vector3.zero, shrinkDuration)
                        .SetEase(Ease.InBack);

                    Tween rotateTween = objectToAnimate
                        .DORotate(new Vector3(0, 360, 0), shrinkDuration, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear);

                    if (targetTransform != null)
                    {
                        Tween moveTween = objectToAnimate
                            .DOMove(targetTransform.position, shrinkDuration)
                            .SetEase(Ease.InBack).OnComplete(() =>
                            {
                                levelManager.CollectLetter(index);
                            });
                    }

                    // Combine tweens with OnComplete callback
                    DOTween.Sequence()
                        .Join(scaleTween)
                        .Join(rotateTween)
                        .AppendInterval(0) // needed to keep the sequence alive
                        .OnComplete(() =>
                        {
                            gameObject.SetActive(false);
                            objectToAnimate.localScale = initialScale;
                            objectToAnimate.position = initialPosition;
                        });
                });
            });
    }

}
