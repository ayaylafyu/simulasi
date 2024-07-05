using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
    [Header("UI")]
    //public Image pictHint;
    public TMP_Text descHint;

    [Header("Panel")]
    public GameObject panelHint;

    [Header("Apreciate SO")]
    public HintSO[] scriptHints;

    private QuestManager questManager;

    private void Awake()
    {
        questManager = QuestManager.Instance;
        if (questManager != null)
        {
            Debug.Log("quest manager");
        }

    }
    private void Start()
    {
        panelHint.SetActive(false);
    }

    private void Update()
    {
        if (panelHint.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Time.timeScale = 0;
                panelHint.SetActive(false);
                
                //QuestManager.apresiasiClosed = true;
                //QuestManager.OnAppreciationPopupClosed();
                //questManager.OnAppreciationPopupClosed();
                //questManager.HelpViewer();
            }
        }
    }

    public void PopUpHint(int index)
    {

        panelHint.SetActive(true);
        //pictHint.sprite = scriptHints[index].imageHint;
        descHint.text = scriptHints[index].descHint;

        Debug.Log("menampilkan apre dari " + scriptHints[index].titleHint);
        //QuestManager.showHint = true;
    }
}
