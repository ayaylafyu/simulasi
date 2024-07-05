using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWifi : MonoBehaviour
{
    public Button btnWifi;
    public GameObject panelWifi;

    private void Start()
    {
        btnWifi.onClick.AddListener(ButtonWifiDiklik);
    }

    private void ButtonWifiDiklik()
    {
        if (panelWifi.activeSelf)
        {
            panelWifi.SetActive(false);
        }
        else
        {
            panelWifi.SetActive(true);
        }
    }
}
