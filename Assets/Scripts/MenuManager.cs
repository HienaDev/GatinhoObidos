using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hello");
            SceneManager.LoadScene(1);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
