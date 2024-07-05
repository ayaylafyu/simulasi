using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHome : MonoBehaviour
{
    public GameObject searchPanelFB;
    public GameObject profilePanelFB;
    public GameObject findbookParent;
    public GameObject chatPanel;
    public Button[] buttonHome;

    private void Start()
    {
        foreach (Button button in buttonHome)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    public void OnButtonClick()
    {
        profilePanelFB.SetActive(false);
        searchPanelFB.SetActive(true);
        chatPanel.SetActive(false);


        // Nonaktifkan parent "Findbook"
        findbookParent.SetActive(false);
    }

}
