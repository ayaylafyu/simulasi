using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public TMP_Text level;
    public TMP_Text hint;
    public Slider sliderExp;
    public TMP_Text tooltipText;
    public GameObject bgTooltipText;

    public static int currentExp;
    private int[] expToNextLevel = { 30, 50, 70, 90, 110, 130, 150 };
    public static int currentLevel = 1;
    public static int currentHint = 3;

    private const string CURRENT_EXP_KEY = "CurrentExp";
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    private const string CURRENT_HINT_KEY = "CurrentHint";

    public static bool isHintOpen = false;
    public static bool hintEmpty = false;
    //public int expSliderMax = 100;

    private void Awake()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        currentExp = PlayerPrefs.GetInt(CURRENT_EXP_KEY);
        currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY);
        currentHint = PlayerPrefs.GetInt(CURRENT_HINT_KEY);
        if (!PlayerPrefs.HasKey(CURRENT_EXP_KEY))
        {
            sliderExp.value = 0f;
        }
        else
        {
            float sliderValue = (currentExp * 1f) / (expToNextLevel[currentLevel - 1] * 1f);
            Debug.Log("slider value : " + sliderValue);
            sliderExp.value = sliderValue;
        }

        if (!PlayerPrefs.HasKey(CURRENT_LEVEL_KEY))
        {
            currentLevel = 1;
            level.text = currentLevel.ToString();
        }
        else
        {
            level.text = currentLevel.ToString();
        }

        if (!PlayerPrefs.HasKey(CURRENT_HINT_KEY))
        {
            currentHint = 3;
            hint.text = "Jumlah hint: " + currentHint.ToString();
        }
        else
        {
            hint.text = "Jumlah hint: " + currentHint.ToString();
        }


        //sliderExp.value = 0f;


        //level.text = currentLevel.ToString();
        //hint.text = "Jumlah hint: " + currentHint.ToString();
        Debug.Log("cureent hint yaitu " + currentHint);
        //LoadPlayerData();
    }

    private void Update()
    {
        if (isHintOpen)
        {
            currentHint--;
            isHintOpen = false;
        }
        hint.text = "Jumlah hint: " + currentHint.ToString();
        if (currentHint == 0)
        {
            hintEmpty = true;
        }
        else if (currentHint > 0)
        {
            hintEmpty = false;
        }
    }

    private void Start()
    {
        bgTooltipText.SetActive(false);
        // Tambahkan event pointer enter dan exit ke slider Exp
        EventTrigger trigger = sliderExp.gameObject.AddComponent<EventTrigger>();
        // Event Pointer Enter
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => { OnPointerEnter((PointerEventData)eventData); });
        trigger.triggers.Add(entryEnter);
        // Event Pointer Exit
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) => { OnPointerExit((PointerEventData)eventData); });
        trigger.triggers.Add(entryExit);
    }

    /*private void SavePlayerData()
    {
        // Save current exp, level, and hint to PlayerPrefs
        PlayerPrefs.SetInt(CURRENT_EXP_KEY, currentExp);
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevel);
        PlayerPrefs.SetInt(CURRENT_HINT_KEY, currentHint);
    }
    private void LoadPlayerData()
    {
        // Load current exp, level, and hint from PlayerPrefs
        currentExp = PlayerPrefs.GetInt(CURRENT_EXP_KEY);
        currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY);
        currentHint = PlayerPrefs.GetInt(CURRENT_HINT_KEY);
    }*/

    public void AddExperience(int exp)
    {
        
       
        Debug.Log("Menambahkan " + exp + " exp ke player");
        currentExp += exp;
        //PlayerPrefs.SetInt(CURRENT_EXP_KEY, currentExp);

        Debug.Log("currentExp: " + currentExp + ", expToNextLevel: " + expToNextLevel[currentLevel - 1]);

        CurrentLevel();
    }

    private void CurrentLevel()
    {
        if (currentLevel < expToNextLevel.Length && currentExp >= expToNextLevel[currentLevel - 1])
        {
            currentExp -= expToNextLevel[currentLevel - 1];
            currentLevel++;
            currentHint++;
            level.text = currentLevel.ToString();
            hint.text = "Jumlah hint: " + currentHint.ToString();
            Debug.Log("Level Up! Current Level: " + currentLevel);
            sliderExp.value = 0f;
            
            Debug.Log("nilai exp saat ini " + currentExp);
        }
        float sliderValue = (currentExp * 1f) / (expToNextLevel[currentLevel - 1] * 1f);
        Debug.Log("slider value : " + sliderValue);
        sliderExp.value = sliderValue;
        PlayerPrefs.SetInt(CURRENT_EXP_KEY, currentExp);
        PlayerPrefs.SetInt(CURRENT_HINT_KEY, currentHint);
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevel);
        //SavePlayerData();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateTooltip(); // Perbarui tooltip saat cursor masuk
        bgTooltipText.SetActive(true); // Tampilkan tooltip
    }

    // Dipanggil saat cursor keluar dari area slider
    public void OnPointerExit(PointerEventData eventData)
    {
        bgTooltipText.SetActive(false); // Sembunyikan tooltip saat cursor keluar
    }

    // Perbarui teks tooltip dengan informasi tentang exp
    private void UpdateTooltip()
    {
        int exp = currentExp;
        int expNeeded = expToNextLevel[currentLevel - 1];

        tooltipText.text = exp + "/" + expNeeded;
    }
}
