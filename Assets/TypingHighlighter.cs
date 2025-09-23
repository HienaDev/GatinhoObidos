using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach this to a GameObject with a TMP_Text.
/// It highlights typed letters in sequence if they match the target word.
/// Backspace removes highlight. Case sensitive. Spaces are ignored while typing.
/// </summary>
public class TypingHighlighter : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    private string targetWord;
    private string targetScene;
    [SerializeField] private Color highlightColor = Color.green;

    private LevelPicker levelPicker;

    private int currentIndex = 0; // Tracks how many characters matched

    public void Initialize(string targetWord, string sceneName, LevelPicker levelPicker)
    {
        this.targetWord = targetWord;
        this.targetScene = sceneName;
        this.levelPicker = levelPicker;

        if (textMeshPro == null)
            textMeshPro = GetComponent<TMP_Text>();
        RefreshText();
    }

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // Backspace
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    RefreshText();
                }
            }
            else
            {
                // Skip over spaces in targetWord automatically
                while (currentIndex < targetWord.Length && targetWord[currentIndex] == ' ')
                {
                    currentIndex++;
                }

                // Case-sensitive match
                if (currentIndex < targetWord.Length && char.ToUpper(c) == char.ToUpper(targetWord[currentIndex]))
                {
                    currentIndex++;
                    RefreshText();
                }
            }
        }
    }

    /// <summary>
    /// Rebuilds the TMP text with proper coloring based on currentIndex
    /// </summary>
    private void RefreshText()
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(highlightColor);
        string highlightedPart = "";
        string remainingPart = "";

        if (currentIndex > 0)
        {
            highlightedPart = $"<color=#{hexColor}>{targetWord.Substring(0, currentIndex)}</color>";
        }

        if (currentIndex < targetWord.Length)
        {
            remainingPart = targetWord.Substring(currentIndex);
        }

        textMeshPro.text = highlightedPart + remainingPart;

        if (currentIndex >= targetWord.Length)
        {
            OnWordCompleted();
        }
    }

    private void OnWordCompleted()
    {
        levelPicker.AnimateDown(targetScene);


    }
}
