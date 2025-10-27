using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private byte transparencyValue = 0;
    [SerializeField] private Image outlineUI;
    private Color32 outlineColorDefault;
    private Color32 outlineColorTransparent;
    [SerializeField] private Image backgroundUI;
    private Color32 backgroundColorDefault;
    private Color32 backgroundColorTransparent;

    private Color32 textColor;
    private Color32 textColorTransparent;

    private float blinkTimer = 0f;
    private bool showCursor = true;
    [SerializeField] private float blinkInterval = 0.5f; // seconds between blinks

    void OnGUI()
    {

        Event e = Event.current;

        //Check the type of the current event, making sure to take in only the KeyDown of the keystroke.
        //char.IsLetter to filter out all other KeyCodes besides alphabetical.
        if (e.type == EventType.KeyDown &&
        e.keyCode.ToString().Length == 1 &&
        char.IsLetter(e.keyCode.ToString()[0]))
        {
            if (text.Length < 26)
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
        textColor = textUI.color;
        textColorTransparent = new Color32((byte)(outlineUI.color.r * 255), (byte)(outlineUI.color.g * 255), (byte)(outlineUI.color.b * 255), 0);

        text = "";

        wordForPlatforms = "";

        typeSounds = GetComponent<TypeSounds>();

        rect = gameObject.GetComponent<RectTransform>();
        defaultRectWidth = rect.rect.width;

        defaultRectOutlineWidth = rectOutline.rect.width;

        outlineColorDefault = outlineUI.color;
        outlineColorTransparent = new Color32((byte)(outlineUI.color.r * 255), (byte)(outlineUI.color.g * 255), (byte)(outlineUI.color.b * 255), transparencyValue);
        backgroundColorDefault = backgroundUI.color;
        backgroundColorTransparent = new Color32((byte)(backgroundUI.color.r * 255), (byte)(backgroundUI.color.g * 255), (byte)(backgroundUI.color.b * 255), transparencyValue);

        textUI.color = textColorTransparent;
    }

    // Update is called once per frame
    void Update()
    {


        // Add Space
        if ((Input.GetKeyDown(KeyCode.Space) && text.Length < 26 && text != ""))
        {

            if (text[text.Length - 1] != '_')
            {
                typeSounds.PlayTypeSound();
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.rect.width + 17.3077f);
                rectOutline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectOutline.rect.width + 17.3077f);// += 17.3077f;
                text += "_";
            }
        }

        // Delete Char
        if (Input.GetKeyDown(KeyCode.Backspace))
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
        if (Input.GetKeyDown(KeyCode.Return))
        {

            typeSounds.PlayDingSound();

            //catInput.GetInput(text);
            catState.CheckInput(text);

            wordForPlatforms = text;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultRectWidth);
            rectOutline.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultRectOutlineWidth);

            text = "";
        }

        // Blink timer (only when there is text)
        if (text.Length > 0)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                showCursor = !showCursor;
                blinkTimer = 0f;
            }
        }

        // Set cursor color based on blink state
        string cursorColor = (showCursor && text.Length > 0) ? "#000000" : "#00000000";
        textUI.text = text + $"<b><color={cursorColor}>|</color></b>";

        textAboveCat.text = text;

        if (text.Length > 0)
        {
            outlineUI.color = outlineColorDefault;
            backgroundUI.color = backgroundColorDefault;
            textUI.color = textColor;
        }
        else
        {
            outlineUI.color = outlineColorTransparent;
            backgroundUI.color = backgroundColorTransparent;
            textUI.color = textColorTransparent;

        }
    }

    public string GetWord() => wordForPlatforms;

    public void ClearWord() => text = "";

}
