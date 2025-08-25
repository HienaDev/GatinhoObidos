using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WriteToUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textAboveCat;
    private TextMeshProUGUI textUI;
    private string text;

    //17.30769231
    private RectTransform rect;
    private float defaultRectWidth;

    [SerializeField] private RectTransform rectOutline;
    private float defaultRectOutlineWidth;

    private TypeSounds typeSounds;

    private string wordForPlatforms;

    [SerializeField] private ReadInput catInput;
    [SerializeField] private CatState catState;

    void OnGUI()
    {

        Event e = Event.current;

        //Check the type of the current event, making sure to take in only the KeyDown of the keystroke.
        //char.IsLetter to filter out all other KeyCodes besides alphabetical.
        if (e.type == EventType.KeyDown &&
        e.keyCode.ToString().Length == 1 &&
        char.IsLetter(e.keyCode.ToString()[0]))
        {
            if(text.Length < 26)
            {
                typeSounds.PlayTypeSound();
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width + 17.3077f);// += 17.3077f;
                rectOutline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectOutline.rect.width + 17.3077f);// += 17.3077f;
                text += e.keyCode;
            }
                
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponentInChildren<TextMeshProUGUI>();

        text = "";

        wordForPlatforms = "";

        typeSounds = GetComponent<TypeSounds>();

        rect = gameObject.GetComponent<RectTransform>();
        defaultRectWidth = rect.rect.width;

        defaultRectOutlineWidth = rectOutline.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        

        // Add Space
        if ((Input.GetKeyDown(KeyCode.Space) && text.Length < 26 && text != ""))
        {

            if(text[text.Length - 1] != '_')
            {
                typeSounds.PlayTypeSound();
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width + 17.3077f);
                rectOutline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectOutline.rect.width + 17.3077f);// += 17.3077f;
                text += "_";
            }
        }

        // Delete Char
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
           

            if (text.Length > 0)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width - 17.3077f);
                rectOutline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectOutline.rect.width - 17.3077f);

                typeSounds.PlayTypeSound();
                text = text.Remove(text.Length - 1);
            }
                
        }

        // Input text
        if(Input.GetKeyDown(KeyCode.Return))
        {

            typeSounds.PlayDingSound();

            //catInput.GetInput(text);
            catState.CheckInput(text);

            wordForPlatforms = text;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultRectWidth);
            rectOutline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultRectOutlineWidth);

            text = "";
        }

        textUI.text = text;
        textAboveCat.text = text;
    }

    public string GetWord() => wordForPlatforms;

}
