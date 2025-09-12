using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private ActionWord missingWord;
    [SerializeField] private int numberOfMissingLetters = 3;
    private List<int> missingLetters;
    [SerializeField] private Transform[] letterPositions;
    [SerializeField] private LetterPickUp letterPrefab;
    [SerializeField] private Transform letterCollectTarget; // Optional target for letters to move towards

    private List<LetterPickUp> spawnedLetters = new List<LetterPickUp>();

    // Start is called before the first frame update
    IEnumerator Start()
    {

        yield return new WaitForSeconds(0.1f); // Wait for Settings to initialize

        if (letterPositions.Length < numberOfMissingLetters)
        {
            Debug.LogError("Not enough letter positions for the number of missing letters.");
            yield break;
        }

        for (int i = 0; i < numberOfMissingLetters; i++)
        {
            Transform pos = letterPositions[i];
            LetterPickUp letterObj = Instantiate(letterPrefab, pos.position, Quaternion.identity);
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
            }
        }
    }

    public void CollectLetter(int index)
    {
        if(missingLetters.Contains(index))
        {
            missingLetters.Remove(index);
        }
        else
        {
            Debug.Log($"Letter {index} is not part of the missing word.");
        }

        if (missingLetters.Count == 0)
        {
            Debug.Log($"All letters collected! You can now perform the action: {missingWord}");
            
            CatState catState = FindObjectOfType<CatState>();

            if (catState != null) catState.UnlockAction(missingWord);

            Settings.UpdateUnlockedWordsDisplayGlobal();
        }
    }
}
