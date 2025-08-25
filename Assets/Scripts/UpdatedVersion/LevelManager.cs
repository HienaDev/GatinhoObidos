using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private ActionWord missingWord;
    [SerializeField] private int numberOfMissingLetters = 3;
    private List<char> missingLetters;
    [SerializeField] private Transform[] letterPositions;
    [SerializeField] private GameObject letterPrefab;
    [SerializeField] private Transform letterCollectTarget; // Optional target for letters to move towards

    // Start is called before the first frame update
    void Start()
    {
        if (letterPositions.Length < numberOfMissingLetters)
        {
            Debug.LogError("Not enough letter positions for the number of missing letters.");
            return;
        }

        missingLetters = new List<char>();

        string missingWordStr = missingWord.ToString();
        List<int> availableIndices = Enumerable.Range(0, missingWordStr.Length).ToList();

        // Shuffle indices
        for (int i = 0; i < availableIndices.Count; i++)
        {
            int swapIndex = Random.Range(i, availableIndices.Count);
            (availableIndices[i], availableIndices[swapIndex]) = (availableIndices[swapIndex], availableIndices[i]);
        }

        for (int i = 0; i < numberOfMissingLetters; i++)
        {
            Transform pos = letterPositions[i];
            GameObject letterObj = Instantiate(letterPrefab, pos.position, Quaternion.identity);
            LetterPickUp letterPickUp = letterObj.GetComponent<LetterPickUp>();

            if (letterPickUp != null)
            {
                // Use shuffled indices instead of random.Range
                char randomLetter = missingWordStr[availableIndices[i]];
                letterPickUp.Instantiate(randomLetter, this, letterCollectTarget);
                missingLetters.Add(randomLetter);
            }
            else
            {
                Debug.LogError("Letter prefab does not have a LetterPickUp component.");
            }
        }

    }

    public void CollectLetter(char letter)
    {
        if(missingLetters.Contains(letter))
        {
            missingLetters.Remove(letter);
        }
        else
        {
            Debug.Log($"Letter {letter} is not part of the missing word.");
        }

        if (missingLetters.Count == 0)
        {
            Debug.Log($"All letters collected! You can now perform the action: {missingWord}");
            
            CatState catState = FindObjectOfType<CatState>();

            if (catState != null) catState.UnlockAction(missingWord);
        }
    }
}
