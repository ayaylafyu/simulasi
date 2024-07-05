using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpManager : MonoBehaviour
{
    [Header("UI Help Panel")]
    public Image pictHelp;
    public TMP_Text textHelp;
    public TMP_Text descHelp;

    [Header("Panel")]
    public GameObject btnHelp;
    public GameObject panelBtnHelp;
    public GameObject panelHelp;
    public GameObject panelCanvaPause;

    [Header("Help SO")]
    public HelpSO[] scriptHelps;

    private void Start()
    {
        //GameObject btnHelp = transform.GetCompo()
        for (int i = 0; i < scriptHelps.Length; i++)
        {
            GameObject clone = Instantiate(btnHelp, panelBtnHelp.transform);

            clone.GetComponentInChildren<TMP_Text>().text = scriptHelps[i].titleHelp;

            Button buttonHelp = clone.GetComponent<Button>();
            int currentIndex = i;
            buttonHelp.onClick.AddListener(() => ButtonHelpClicked(currentIndex));
        }
    }

    public void ButtonHelpClicked(int index)
    {

        panelHelp.SetActive(true);
        panelCanvaPause.SetActive(true);
        pictHelp.sprite = scriptHelps[index].imageHelp;
        textHelp.text = scriptHelps[index].titleHelp;
        descHelp.text = scriptHelps[index].descHelp;


        Debug.Log("menampilkan help dari " + textHelp.text);
    }
}
