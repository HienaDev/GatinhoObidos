using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string nextLevelName;

    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
    }
}
