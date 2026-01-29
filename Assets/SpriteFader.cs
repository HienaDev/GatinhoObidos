using UnityEngine;
using DG.Tweening;

public class SpriteFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private bool fadeOnStart = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get SpriteRenderer on this object
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Ensure starting alpha is 1
    }

    private void Start()
    {
        if (fadeOnStart)
            FadeOut();
    }

    public void FadeOut()
    {
        if (spriteRenderer != null)
        {
            // Fade alpha to 0
            spriteRenderer.DOFade(0f, fadeDuration);
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on " + gameObject.name);
        }
    }

    public void FadeIn()
    {
        if (spriteRenderer != null)
        {
            // Fade alpha to 1
            spriteRenderer.DOFade(1f, fadeDuration);
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on " + gameObject.name);
        }
    }
}
