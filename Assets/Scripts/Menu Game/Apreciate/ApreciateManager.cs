using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApreciateManager : MonoBehaviour
{
    [Header("UI")]
    //public Image pictApre;
    public TMP_Text descApre;

    [Header("Panel")]
    public GameObject panelApreciate;

    [Header("Apreciate SO")]
    public ApreciateSO[] scriptApreciates;

    public QuestManager questManager;

    private void Awake()
    {
        //questManager = QuestManager.Instance;
        if (questManager != null)
        {
            Debug.Log("QuestManager successfully found in ApreciateManager.Awake");
        }
        else
        {
            Debug.LogError("QuestManager is null in ApreciateManager.Awake");
        }
    }

    private void Start()
    {
        if (panelApreciate != null)
        {
            panelApreciate.SetActive(false);
            Debug.Log("PanelApreciate successfully set to inactive in ApreciateManager.Start");
        }
        else
        {
            Debug.LogError("PanelApreciate is null in ApreciateManager.Start");
        }
    }

    private void Update()
    {
        if (panelApreciate == null)
        {
            Debug.LogError("PanelApreciate is null in ApreciateManager.Update");
            return;
        }

        if (panelApreciate.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Time.timeScale = 0;
                panelApreciate.SetActive(false);
                QuestManager.apresiasiClosed = true;
                Debug.Log("PanelApreciate closed, apresiasiclosed = " + QuestManager.apresiasiClosed);

                if (questManager != null)
                {
                    questManager.HelpViewer();
                    Debug.Log("HelpViewer called successfully");
                }
                else
                {
                    Debug.LogError("QuestManager is null in ApreciateManager.Update when trying to call HelpViewer");
                }

                Debug.Log("apre after helpviewer closed, apresiasiclosed = " + QuestManager.apresiasiClosed);
            }
        }
    }

    public void PopUpApreciate(int index)
    {
        if (panelApreciate != null && scriptApreciates != null && index < scriptApreciates.Length)
        {
            panelApreciate.SetActive(true);
            //pictApre.sprite = scriptApreciates[index].imageApre;
            descApre.text = scriptApreciates[index].descApre;

            Debug.Log("menampilkan apre dari " + scriptApreciates[index].titleApre);
        }
        else
        {
            Debug.LogError("panelApreciate or scriptApreciates is null, or index is out of range in ApreciateManager.PopUpApreciate");
        }
    }

}
