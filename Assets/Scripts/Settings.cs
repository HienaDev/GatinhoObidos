using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public static class LocalizationEvents
{
    public static event System.Action OnLanguageChanged;

    public static void NotifyLanguageChanged()
    {
        OnLanguageChanged?.Invoke();
    }
}

public class Settings : MonoBehaviour
{

    private static Settings instance;

    [Header("General Settings")]
    [SerializeField] private GameObject gameMenu;

    [Header("Localization Settings")]
    public string startingLanguage = "English";
    private string currentLanguage; 
    public string csvFileName = "localization.csv";
    [SerializeField] private TMP_Dropdown languageDropdown;

    private string csvContent;
    private Dictionary<string, Dictionary<string, string>> localizedData;
    public static string GetText(string key)
    {

        return instance.localizedData[instance.currentLanguage][key];
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentLanguage = startingLanguage;

        StartCoroutine(LoadCSV());
    }

    private void Update()
    {
        // This is just to demonstrate that the localization works
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameMenu.SetActive(!gameMenu.activeSelf);
        }
    }

    IEnumerator LoadCSV()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, csvFileName);

        if (File.Exists(filePath))
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs))
                {
                    csvContent = reader.ReadToEnd();
                }

                Debug.Log("Loaded CSV:\n" + csvContent);
            }
            catch (IOException ex)
            {
                Debug.LogError("Error reading CSV: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("File not found at: " + filePath);
        }

        yield return null;

        ParseDict();
    }


    private void ParseDict()
    {
        localizedData = new Dictionary<string, Dictionary<string, string>>();

        string[] lines = csvContent.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) return; // need header + at least 1 row

        // --- First line is the header ---
        string[] headers = lines[0].Split(',');
        // headers[0] == "word", headers[1] == "Portuguese", headers[2] == "English"

        // Create dictionary entry for each language column
        for (int i = 1; i < headers.Length; i++)
        {
            string lang = headers[i].Trim();
            if (!localizedData.ContainsKey(lang))
            {
                localizedData[lang] = new Dictionary<string, string>();
            }
        }

        // --- Now process each row ---
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = SplitCsvLine(lines[i]); // custom split to handle quotes
            if (parts.Length != headers.Length) continue;

            string key = parts[0].Trim();

            for (int j = 1; j < headers.Length; j++)
            {
                string lang = headers[j].Trim();
                string value = parts[j].Trim().Trim('"'); // remove quotes

                localizedData[lang][key] = value;
            }
        }

        Debug.Log("Dictionary built!");
        Debug.Log("Portuguese[right] = " + localizedData["Portugues"]["right"]);
        Debug.Log("English[right] = " + localizedData["English"]["right"]);
        Debug.Log("Spanish[right] = " + localizedData["Espanol"]["right"]);

        InitiateDropdown();
    }

    // Handles CSV lines with quotes (like tutorial_direita row)
    private string[] SplitCsvLine(string line)
    {
        var values = new List<string>();
        bool inQuotes = false;
        string current = "";

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }
        values.Add(current);
        return values.ToArray();
    }

    private void InitiateDropdown()
    {
        languageDropdown.ClearOptions();

        // Create a list of strings
        List<string> options = new List<string>(localizedData.Keys.ToList());

        // Add them to the dropdown
        languageDropdown.AddOptions(options);

        languageDropdown.onValueChanged.AddListener(ChangeLanguage);

        languageDropdown.value = 1; // Set default value to English (index 1)

        ChangeLanguage(1);// Set default language to English (index 1)
    }

    public void ChangeLanguage(int index)
    {
        Debug.Log("Changing language to index: " + index);
        Debug.Log(languageDropdown.options[index].text);

        currentLanguage = languageDropdown.options[index].text;
        LocalizationEvents.NotifyLanguageChanged();
    }
}
