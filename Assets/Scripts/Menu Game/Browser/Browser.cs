using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Browser : MonoBehaviour
{
    public Button buttonFindbook;
    public Button buttonBugDB;

    public GameObject panelBrowser;
    public GameObject panelBug;
    public GameObject panelFindbook;

    private void Awake()
    {
        panelBrowser.SetActive(true);
        panelBug.SetActive(false);
        panelFindbook.SetActive(false);
    }

    public void OnButtonFindbookClicked()
    {
        panelBrowser.SetActive(false);
        panelBug.SetActive(false);
        panelFindbook.SetActive(true);
    }

    public void OnButtonBugClicked()
    {
        panelBrowser.SetActive(false);
        panelBug.SetActive(true);
        panelFindbook.SetActive(false);
    }
}
