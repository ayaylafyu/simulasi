using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserPrefManager : MonoBehaviour
{
    [Header("Button UI")]
    public Button buttonHome;
    public Button buttonDisplay;

    public GameObject uiHome;
    public GameObject uiDisplay;
    public GameObject panelUserPref;

    public Sprite[] selectedUI;
    public Sprite[] normalUI;

    [Header("Button Progress")]
    public Button buttonLanjut;
    public Button buttonUlangi;

    public Sprite[] selected;
    public Sprite[] normal;

    [Header("User")]
    public TMP_Text user;
    public TMP_Text level;
    public TMP_Text exp;

    private string nama;

    public QuestManager questManager;
    public FileManager fileManager;
    public BugDBManager bugDB;
    public Player player;
    public WifiManager wifi;
    public ButtonClose button;
    public QuestGiver questGiver;

    private void Start()
    {
        buttonLanjut.image.sprite = normal[0];
        buttonUlangi.image.sprite = normal[1];
        uiHome.SetActive(true);
        uiDisplay.SetActive(false);

        buttonHome.onClick.AddListener(BtnHomeClicked);
        buttonDisplay.onClick.AddListener(BtnDisplayClicked);

        buttonLanjut.onClick.AddListener(BtnLanjutClicked);
        buttonUlangi.onClick.AddListener(BtnUlangiClicked);

        nama = PlayerPrefs.GetString("UserName");
    }

    private void Update()
    {
        user.text = "Halo, " + nama;
        level.text = "Level " + Player.currentLevel;
        exp.text = "Exp " + Player.currentExp;

        if (uiDisplay.activeSelf)
        {
            buttonHome.image.sprite = normalUI[0];
            buttonDisplay.image.sprite = selectedUI[1];
        }
        else if (uiHome.activeSelf)
        {
            buttonHome.image.sprite = selectedUI[0];
            buttonDisplay.image.sprite = normalUI[1];
        }
    }

    private void BtnHomeClicked()
    {
        buttonHome.image.sprite = selectedUI[0];
        buttonDisplay.image.sprite = normalUI[1];
        uiDisplay.SetActive(false);
    }

    private void BtnDisplayClicked()
    {
        buttonHome.image.sprite = normalUI[0];
        buttonDisplay.image.sprite = selectedUI[1];
        uiDisplay.SetActive(true);
    }

    private void BtnLanjutClicked()
    {
        buttonLanjut.image.sprite = selected[0];
        buttonUlangi.image.sprite = normal[1];
        panelUserPref.SetActive(false);
    }

    public void BtnUlangiClicked()
    {
        //bugDB.ResetAllButtonStates();
        buttonLanjut.image.sprite = selected[0];
        buttonUlangi.image.sprite = normal[1];
        PlayerPrefs.DeleteAll();


        //QuestManager.currentTime = 10;
        

        Debug.Log("player has " + PlayerPrefs.HasKey("CurrentTimer"));
        //PlayerPrefs.SetInt("MainMenuOpened", 0);
        PlayerPrefs.Save();

        Debug.Log("player name : " + nama);
        

        Debug.Log("file download : " + PlayerPrefs.GetString("DownloadedFiles"));
        Debug.Log("bug db : " + PlayerPrefs.GetInt("ButtonState"));
        //Debug.Log("bug db pt2 : " + PlayerPrefs.HasKey(ButtonStatePrefix + buttonText));

        
        questManager.StartQuestManager();
        fileManager.FungsiLoad();
        bugDB.ResetAllButtonStates();
        player.LoadGame();
        wifi.LoadWifi();
        button.ButtonTerminalClicked();
        questManager.AktifQuest(0);
        //questGiver.ViewPanelWin();
        //bugDB.FungsiLoadBugDB();
    }
}
