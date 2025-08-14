using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English,
    Portuguese,
}

public class Settings : MonoBehaviour
{
    [Header("Settings")]
    public Language currentLanguage = Language.English;
    public string csvFileName = "localization.csv";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
