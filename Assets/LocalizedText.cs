using TMPro;
using UnityEngine;
using System.Collections;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _localizationKey;
    private TextMeshProUGUI _textComponent;

    IEnumerator Start()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        LocalizationEvents.OnLanguageChanged += UpdateText;

        yield return new WaitForSeconds(0.1f); // Wait for Settings to initialize
        UpdateText(); // Initial update
    }

    void OnDestroy()
    {
        LocalizationEvents.OnLanguageChanged -= UpdateText;
    }

    void UpdateText()
    {
        _textComponent.text = Settings.GetText(_localizationKey);
    }
}