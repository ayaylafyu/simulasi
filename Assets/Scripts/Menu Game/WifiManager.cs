using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WifiManager : MonoBehaviour
{
    public GameObject popupUI;
    public TMP_Text wifiNameText;
    public GameObject wifiConnected1;
    public GameObject wifiConnected2;
    public GameObject wifiName1;
    public GameObject wifiName2;
    public TMP_InputField passwordInputField;
    public TMP_Text messageText;

    // Dictionary untuk memetakan nama WiFi ke passwordnya
    private Dictionary<string, string> wifiPasswords = new Dictionary<string, string>()
    {
        { "Wifi ke satu", "password1" },
        { "Wifi ke dua", "password2" }
    };
    private string currentWiFiName; // Password WiFi simulasi
    private bool isWiFiConnected1 = false;
    private bool isWiFiConnected2 = false;

    //public static WifiManager Instance;
    private QuestManager questManager;

    private const string LAST_CONNECTED_WIFI_KEY = "LastConnectedWiFi";

    private void Start()
    {
        //Instance = this;
        questManager = QuestManager.Instance;
        LoadWifi();
    }

    public void LoadWifi()
    {
        popupUI.SetActive(false);

        if (PlayerPrefs.HasKey(LAST_CONNECTED_WIFI_KEY))
        {
            currentWiFiName = PlayerPrefs.GetString(LAST_CONNECTED_WIFI_KEY);
            Debug.Log("wifi terakhir: " + currentWiFiName);
            WifiView();
        }
        else
        {
            currentWiFiName = "Wifi ke satu";
            WifiView();
        }
    }

    public void ConnectToWiFi(string wifiName)
    {
        currentWiFiName = wifiName; // Menyimpan nama WiFi yang dipilih
        wifiNameText.text = wifiName;

        messageText.gameObject.SetActive(false);
        popupUI.SetActive(true);
    }

    public void OnConnectButtonClicked()
    {
        string inputPassword = passwordInputField.text;

        if (wifiPasswords.ContainsKey(currentWiFiName)) // Memeriksa apakah nama WiFi ada di dalam dictionary
        {
            string correctPassword = wifiPasswords[currentWiFiName];
            if (inputPassword == correctPassword)
            {
                WifiView();
                
                PlayerPrefs.SetString(LAST_CONNECTED_WIFI_KEY, currentWiFiName);
                questManager.SliderRestart();
                popupUI.SetActive(false);
            }
            else
            {
                StartCoroutine(WrongPass());
                
            }
            passwordInputField.text = "";
        }
        else
        {
            messageText.text = "WiFi not found!";
        }
    }

    private void WifiView()
    {
        // Setel status koneksi WiFi yang benar
        if (currentWiFiName == "Wifi ke satu")
        {
            //passwordInputField.text = "";
            isWiFiConnected1 = true;
            wifiConnected1.SetActive(true);
            wifiName1.SetActive(false);
            // Matikan status koneksi WiFi 2 jika ada
            isWiFiConnected2 = false;
            wifiConnected2.SetActive(false);
            wifiName2.SetActive(true);

        }
        else if (currentWiFiName == "Wifi ke dua")
        {
            isWiFiConnected2 = true;
            wifiConnected2.SetActive(true);
            wifiName2.SetActive(false);
            // Matikan status koneksi WiFi 1 jika ada
            isWiFiConnected1 = false;
            wifiConnected1.SetActive(false);
            wifiName1.SetActive(true);
        }
    }
    private IEnumerator WrongPass()
    {
        messageText.text = "Incorrect password!";
        messageText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        messageText.gameObject.SetActive(false);
    }

    public void ClosePopup()
    {
        popupUI.SetActive(false);
    }
}
