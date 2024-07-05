using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClose : MonoBehaviour
{
    [Header("Browser")]
    public Button btnCloseBrowser;
    public GameObject pageBrowser;
    public GameObject bugDB;
    public GameObject findbook;
    public GameObject searchUser;
    public GameObject userDisplay;
    public GameObject uiChat;

    [Header("Terminal")]
    public Button btnCloseTerminal;
    private TerminalManager terminal;

    [Header("Dmail")]
    public Button btnCloseDmail;
    public GameObject uiListPesan;
    public GameObject uiPesan;

    [Header("User Pref")]
    public Button btnCloseUser;
    public GameObject uiHome;
    public GameObject uiDisplay;

    private void Start()
    {
        terminal = TerminalManager.Instance;
        btnCloseBrowser.onClick.AddListener(ButtonBrowserClicked);
        btnCloseDmail.onClick.AddListener(ButtonDmailClicked);
        btnCloseTerminal.onClick.AddListener(ButtonTerminalClicked);
        btnCloseUser.onClick.AddListener(ButtonUserClicked);
    }

    private void ButtonBrowserClicked()
    {
        pageBrowser.SetActive(true);
        bugDB.SetActive(false);
        findbook.SetActive(false);
        searchUser.SetActive(true);
        userDisplay.SetActive(false);
        uiChat.SetActive(false);
    }

    private void ButtonDmailClicked()
    {
        uiListPesan.SetActive(true);
        uiPesan.SetActive(false);
    }

    public void ButtonTerminalClicked()
    {
        terminal.CleanUpClonedData();
    }

    private void ButtonUserClicked()
    {
        uiHome.SetActive(true);
        uiDisplay.SetActive(false);
    }
}
