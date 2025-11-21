using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private ActionWord missingWord;
    [SerializeField] private int numberOfMissingLetters = 3;
    private List<int> missingLetters;
    private string missingWordLocalized;
    [SerializeField] private Transform[] letterPositions;
    [SerializeField] private LetterPickUp letterPrefab;
    [SerializeField] private Transform letterCollectTarget; // Optional target for letters to move towards
    private Vector3 wordCollectTarget;
    private Animator noteBookAnimator;

    [SerializeField] private GameObject particleExplosion;

    private List<LetterPickUp> spawnedLetters = new List<LetterPickUp>();

    private int lettersCollected = 0;

    [SerializeField] private Color letterColor = Color.black;

    [SerializeField] private string currentLevelName;

    [SerializeField] private TextMeshProUGUI wordMissingUI;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitForSeconds(0.1f); // Wait for Settings to initialize

        wordCollectTarget = Camera.main.ScreenToWorldPoint(FindAnyObjectByType<TAG_WORDS>().transform.position);
        noteBookAnimator = FindAnyObjectByType<TAG_WORDS>().GetComponent<Animator>();

        UnlockLevel(currentLevelName);

        if (letterPositions.Length < numberOfMissingLetters)
        {
            Debug.LogError("Not enough letter positions for the number of missing letters.");
            yield break;
        }

        for (int i = 0; i < numberOfMissingLetters; i++)
        {
            Transform pos = letterPositions[i];
            LetterPickUp letterObj = Instantiate(letterPrefab, pos.position, Quaternion.identity);
            letterObj.text.color = letterColor;
            LetterPickUp letterPickUp = letterObj.GetComponent<LetterPickUp>();

            spawnedLetters.Add(letterObj);

        }

        UpdateLetters();

        LocalizationEvents.OnLanguageChanged += UpdateLetters;
    }

    private void UpdateLetters()
    {

        missingLetters = new List<int>();

        string missingWordStr = Settings.GetText(missingWord.ToString());
        missingWordLocalized = missingWordStr;
        wordMissingUI.text = missingWordStr;
        List<int> availableIndices = Enumerable.Range(0, missingWordStr.Length).ToList();

        // Shuffle indices
        for (int i = 0; i < availableIndices.Count; i++)
        {
            int swapIndex = Random.Range(i, availableIndices.Count);
            (availableIndices[i], availableIndices[swapIndex]) = (availableIndices[swapIndex], availableIndices[i]);
        }

        for (int i = 0; i < spawnedLetters.Count; i++)
        {
            if (spawnedLetters[i] != null)
            {
                // Use shuffled indices instead of random.Range
                char randomLetter = missingWordStr[availableIndices[i]];
                spawnedLetters[i].Instantiate(missingWord, availableIndices[i], this, letterCollectTarget);
                missingLetters.Add(availableIndices[i]);

                wordMissingUI.text = ReplaceLetter(wordMissingUI.text, availableIndices[i], '_');
            }
        }
    }

    private string ReplaceLetter(string word, int index, char c)
    {
        Debug.Log($"Try replace {c} letter ({index}) in word {word}"); 
        return word.Substring(0, index) + c + word.Substring(index + 1);
    }

    public void CollectLetter(int index)
    {
        if(missingLetters.Contains(index))
        {
            lettersCollected++;
            wordMissingUI.text = ReplaceLetter(wordMissingUI.text, index, missingWordLocalized[index]);
            missingLetters.Remove(index);
        }
        else
        {
            Debug.Log($"Letter {index} is not part of the missing word.");
        }

        if (lettersCollected >= numberOfMissingLetters)
        {
            Debug.Log($"All letters collected! You can now perform the action: {missingWord}");
            
            CatState catState = FindObjectOfType<CatState>();

            ScaleDownAndDeactivateWord(letterCollectTarget.GetComponent<RectTransform>(), 2f);

            if (catState != null) catState.UnlockAction(missingWord);

            Settings.UpdateUnlockedWordsDisplayGlobal();
        }
    }

    private void ScaleDownAndDeactivateWord(RectTransform target, float duration)
    {


        // Create tween
        target.DOMove(wordCollectTarget, duration).SetEase(Ease.InBack);

        target.DOScale(Vector3.zero, duration)
              .SetEase(Ease.InBack) // nice easing effect
              .OnComplete(() =>
              {
                  target.gameObject.SetActive(false);
                  particleExplosion = Instantiate(particleExplosion);
                  particleExplosion.transform.position = target.transform.position;
                  noteBookAnimator.SetTrigger("Writing");
              });
    }

    /// <summary>
    /// Unlocks a new level and saves it in PlayerPrefs
    /// </summary>
    public void UnlockLevel(string levelName)
    {
        string unlocked = PlayerPrefs.GetString("UnlockedLevels", "");

        if (!unlocked.Contains(levelName))
        {
            unlocked += levelName + ";"; // Add separator
            PlayerPrefs.SetString("UnlockedLevels", unlocked);
            PlayerPrefs.Save();
            Debug.Log($"Unlocked: {levelName}");
        }
    }
}
