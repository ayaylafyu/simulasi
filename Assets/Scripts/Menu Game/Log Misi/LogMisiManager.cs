using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogMisiManager : MonoBehaviour
{
    private int index;
    
    public GameObject panelLog;

    public GameObject logView;
    public GameObject[] log;
    public TMP_Text[] titleLog;
    public TMP_Text[] descLog;

    public Button[] logMisi;
    public Button[] softLog;
    public Sprite[] normal;
    public Sprite[] selected;
    public LogSO[] logSO;

    // wifi crack
    /*public static bool wifiCrack = false;
    public static bool userPref = false;
    public static bool openDmail = false;

    // snort
    public static bool snort = false;
    public static bool snort2 = false;
    public static bool snort3 = false;
    public static bool snort4 = false;

    // nmap
    public static bool nmap = false;
    public static bool nmap2 = false;
    public static bool nmap3 = false;

    // exploit
    public static bool exploit = false;
    public static bool exploit2 = false;
    public static bool exploit3 = false;

    // osfinger
    public static bool osFinger = false;
    public static bool osFinger2 = false;

    // netscan
    public static bool netScan = false;

    // netcat
    public static bool netcat = false;
    public static bool netcat2 = false;
    public static bool netcat3 = false;
    public static bool netcat4 = false;*/

    private static bool _wifiCrack;
    private static bool _userPref;
    private static bool _openDmail;
    private static bool _snort;
    private static bool _snort2;
    private static bool _snort3;
    private static bool _snort4;
    private static bool _nmap;
    private static bool _nmap2;
    private static bool _nmap3;
    private static bool _exploit;
    private static bool _exploit2;
    private static bool _exploit3;
    private static bool _osFinger;
    private static bool _osFinger2;
    private static bool _netScan;
    private static bool _netcat;
    private static bool _netcat2;
    private static bool _netcat3;
    private static bool _netcat4;

    public static bool wifiCrack { get { return _wifiCrack; } set { _wifiCrack = value; OnBooleanChanged(); } }
    public static bool userPref { get { return _userPref; } set { _userPref = value; OnBooleanChanged(); } }
    public static bool openDmail { get { return _openDmail; } set { _openDmail = value; OnBooleanChanged(); } }
    public static bool snort { get { return _snort; } set { _snort = value; OnBooleanChanged(); } }
    public static bool snort2 { get { return _snort2; } set { _snort2 = value; OnBooleanChanged(); } }
    public static bool snort3 { get { return _snort3; } set { _snort3 = value; OnBooleanChanged(); } }
    public static bool snort4 { get { return _snort4; } set { _snort4 = value; OnBooleanChanged(); } }
    public static bool nmap { get { return _nmap; } set { _nmap = value; OnBooleanChanged(); } }
    public static bool nmap2 { get { return _nmap2; } set { _nmap2 = value; OnBooleanChanged(); } }
    public static bool nmap3 { get { return _nmap3; } set { _nmap3 = value; OnBooleanChanged(); } }
    public static bool exploit { get { return _exploit; } set { _exploit = value; OnBooleanChanged(); } }
    public static bool exploit2 { get { return _exploit2; } set { _exploit2 = value; OnBooleanChanged(); } }
    public static bool exploit3 { get { return _exploit3; } set { _exploit3 = value; OnBooleanChanged(); } }
    public static bool osFinger { get { return _osFinger; } set { _osFinger = value; OnBooleanChanged(); } }
    public static bool osFinger2 { get { return _osFinger2; } set { _osFinger2 = value; OnBooleanChanged(); } }
    public static bool netScan { get { return _netScan; } set { _netScan = value; OnBooleanChanged(); } }
    public static bool netcat { get { return _netcat; } set { _netcat = value; OnBooleanChanged(); } }
    public static bool netcat2 { get { return _netcat2; } set { _netcat2 = value; OnBooleanChanged(); } }
    public static bool netcat3 { get { return _netcat3; } set { _netcat3 = value; OnBooleanChanged(); } }
    public static bool netcat4 { get { return _netcat4; } set { _netcat4 = value; OnBooleanChanged(); } }

    private static event System.Action BooleanChanged;


    private void Start()
    {
        BooleanChanged += UpdateLogListeners;
        UpdateLogListeners();
    }

    private void OnDestroy()
    {
        BooleanChanged -= UpdateLogListeners;
    }

    private static void OnBooleanChanged()
    {
        BooleanChanged?.Invoke();
    }

    private void UpdateLogListeners()
    {
        // Hapus semua listener onClick untuk tombol
        for (int i = 0; i < logMisi.Length; i++)
        {
            logMisi[i].onClick.RemoveAllListeners();
            softLog[i].onClick.RemoveAllListeners();
        }
        // 0
        if (wifiCrack)
        {
            logMisi[0].onClick.AddListener(WifiCrack);
            softLog[0].onClick.AddListener(WifiCrack);
        }
        // 1
        if (snort)
        {
            logMisi[1].onClick.AddListener(Snort);
            softLog[1].onClick.AddListener(Snort);
        }
        // 2
        if (nmap)
        {
            logMisi[2].onClick.AddListener(NMAP);
            softLog[2].onClick.AddListener(NMAP);
        }
        // 3
        if (exploit)
        {
            logMisi[3].onClick.AddListener(Exploit);
            softLog[3].onClick.AddListener(Exploit);
        }
        // 4
        if (osFinger)
        {
            logMisi[4].onClick.AddListener(OsFinger);
            softLog[4].onClick.AddListener(OsFinger);
        }
        // 5
        if (netScan)
        {
            logMisi[5].onClick.AddListener(NetScan);
            softLog[5].onClick.AddListener(NetScan);
        }
        // 6
        if (netcat)
        {
            logMisi[6].onClick.AddListener(Netcat);
            softLog[6].onClick.AddListener(Netcat);
        }
    }

    private void LogManager(int i)
    {
        LogSO logIni = logSO[i];

        // wifi crack
        if (i == 0) { index = 0; }
        else if (i == 1) { index = 1; }
        else if (i == 2) { index = 2; }

        // snort
        else if (i == 3) { index = 0; }
        else if (i == 4) { index = 1; }
        else if (i == 5) { index = 2; }
        else if (i == 6) { index = 3; }

        // nmap
        else if (i == 7) { index = 0; }
        else if (i == 8) { index = 1; }
        else if (i == 9) { index = 2; }

        // exploit
        else if (i == 10) { index = 0; }
        else if (i == 11) { index = 1; }
        else if (i == 12) { index = 2; }

        // os finger
        else if (i == 13) { index = 0; }
        else if (i == 14) { index = 1; }

        // netscan
        else if (i == 15) { index = 0; }

        // netcat
        else if (i == 16) { index = 0; }
        else if (i == 17) { index = 1; }
        else if (i == 18) { index = 2; }
        else if (i == 19) { index = 3; }

        titleLog[index].text = logIni.nameLog;
        descLog[index].text = logIni.descLog;

        log[index].SetActive(true);

        float fixedWidth = 879.2322f;
        float fixedHeighTitle = 46.93579f;
        float totalHeight = 0f;



        RectTransform bubbleTextRectTransform0 = log[0].GetComponent<RectTransform>();
        if (index == 0)
        {
            RectTransform textRectTransform = descLog[index].rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight);

            bubbleTextRectTransform0 = log[index].GetComponent<RectTransform>();
            bubbleTextRectTransform0.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight + fixedHeighTitle + 10f);
            if (bubbleTextRectTransform0.sizeDelta.y < 529.23f)
            {
                totalHeight = 529.23f;
            }
            else
            {
                totalHeight = bubbleTextRectTransform0.sizeDelta.y;
            }
            
        }
        RectTransform bubbleTextRectTransform1 = log[1].GetComponent<RectTransform>();
        if (index == 1)
        {
            RectTransform textRectTransform = descLog[index].rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight);

            bubbleTextRectTransform1 = log[index].GetComponent<RectTransform>();
            bubbleTextRectTransform1.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight + fixedHeighTitle + 10f);
            float total = bubbleTextRectTransform0.sizeDelta.y + 20f + bubbleTextRectTransform1.sizeDelta.y;
            if (total < 529.23f)
            {
                totalHeight = 529.23f;
            }
            else
            {
                totalHeight = bubbleTextRectTransform0.sizeDelta.y + 20f + bubbleTextRectTransform1.sizeDelta.y;
            }
        }
        RectTransform bubbleTextRectTransform2 = log[2].GetComponent<RectTransform>();
        if (index == 2)
        {
            RectTransform textRectTransform = descLog[index].rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight);

            bubbleTextRectTransform2 = log[index].GetComponent<RectTransform>();
            bubbleTextRectTransform2.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight + fixedHeighTitle + 10f);
            float total = bubbleTextRectTransform0.sizeDelta.y + 20f + bubbleTextRectTransform1.sizeDelta.y + 20f + bubbleTextRectTransform2.sizeDelta.y;
            if (total < 529.23f)
            {
                totalHeight = 529.23f;
            }
            else
            {
                totalHeight = bubbleTextRectTransform0.sizeDelta.y + 20f + bubbleTextRectTransform1.sizeDelta.y + 20f + bubbleTextRectTransform2.sizeDelta.y;
            }
        }
        RectTransform bubbleTextRectTransform3 = log[3].GetComponent<RectTransform>();
        if (index == 3)
        {
            RectTransform textRectTransform = descLog[index].rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight);

            bubbleTextRectTransform3 = log[index].GetComponent<RectTransform>();
            bubbleTextRectTransform3.sizeDelta = new Vector2(fixedWidth, descLog[index].preferredHeight + fixedHeighTitle + 10f);
            totalHeight += bubbleTextRectTransform0.sizeDelta.y + 20f + bubbleTextRectTransform1.sizeDelta.y + 20f + bubbleTextRectTransform2.sizeDelta.y + 20f + bubbleTextRectTransform3.sizeDelta.y;
        }
        
        RectTransform bubbleScrollRectTransform = logView.GetComponent<RectTransform>();
        bubbleScrollRectTransform.sizeDelta = new Vector2(fixedWidth, totalHeight);

        VerticalLayoutGroup vertical = log[index].GetComponent<VerticalLayoutGroup>();

        if (vertical != null)
        {
            vertical.childAlignment = TextAnchor.UpperLeft;
            //horizontal.childControlWidth = true;
        }
    }

    // index 0-2
    private void WifiCrack()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        softLog[0].image.sprite = selected[0];
        for (int i = 1; i < 7; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        if (wifiCrack)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(0);
        }
        if (userPref)
        {
            log[2].SetActive(false);
            log[3].SetActive(false);
            LogManager(0);
            LogManager(1);
        }
        if (openDmail)
        {
            log[3].SetActive(false);
            LogManager(0);
            LogManager(1);
            LogManager(2);
        }
    }

    // index 3-6
    private void Snort()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        // wifi crack normal
        softLog[0].image.sprite = normal[0];

        softLog[1].image.sprite = selected[1];
        for (int i = 2; i < 7; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        if (snort)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(3);
        }
        if (snort2)
        {
            log[2].SetActive(false);
            log[3].SetActive(false);
            LogManager(3);
            LogManager(4);
        }
        if (snort3)
        {
            log[3].SetActive(false);
            LogManager(3);
            LogManager(4);
            LogManager(5);
        }
        if (snort4)
        {
            LogManager(3);
            LogManager(4);
            LogManager(5);
            LogManager(6);
        }
    }

    // index 7-9
    private void NMAP()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        // wifi crack dan snort normal
        for (int i = 0; i < 2; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        softLog[2].image.sprite = selected[2];
        for (int i = 3; i < 7; i++)
        {
            softLog[i].image.sprite = normal[i];
        }
        //titleLog.text = logSO[2].nameLog;
        //descLog.text = logSO[2].descLog;

        if (nmap)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(7);
        }
        if (nmap2)
        {
            log[2].SetActive(false);
            log[3].SetActive(false);
            LogManager(7);
            LogManager(8);
        }
        if (nmap3)
        {
            log[3].SetActive(false);
            LogManager(7);
            LogManager(8);
            LogManager(9);
        }
    }

    // index 10-12
    private void Exploit()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        // normal
        for (int i = 0; i < 3; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        softLog[3].image.sprite = selected[3];
        for (int i = 4; i < 7; i++)
        {
            softLog[i].image.sprite = normal[i];
        }
        //titleLog.text = logSO[3].nameLog;
        //descLog.text = logSO[3].descLog;
        if (exploit)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(10);
        }
        if (exploit2)
        {
            log[2].SetActive(false);
            log[3].SetActive(false);
            LogManager(10);
            LogManager(11);
        }
        if (exploit3)
        {
            log[3].SetActive(false);
            LogManager(10);
            LogManager(11);
            LogManager(12);
        }
    }

    // index 13-14
    private void OsFinger()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        // normal
        for (int i = 0; i < 4; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        softLog[4].image.sprite = selected[4];
        for (int i = 5; i < 7; i++)
        {
            softLog[i].image.sprite = normal[i];
        }
        //titleLog.text = logSO[4].nameLog;
        //descLog.text = logSO[4].descLog;
        if (osFinger)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(13);
        }
        if (osFinger2)
        {
            log[2].SetActive(false);
            log[3].SetActive(false);
            LogManager(13);
            LogManager(14);
        }
    }

    // index 15
    private void NetScan()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        // normal
        for (int i = 0; i < 5; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        softLog[5].image.sprite = selected[5];
        // netcat normal
        softLog[6].image.sprite = normal[6];
        //titleLog.text = logSO[5].nameLog;
        //descLog.text = logSO[5].descLog;
        if (netScan)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(15);
        }
    }

    // index 16-19
    private void Netcat()
    {
        panelLog.SetActive(true);
        panelLog.transform.SetAsLastSibling();
        // normal
        for (int i = 0; i < 6; i++)
        {
            softLog[i].image.sprite = normal[i];
        }

        softLog[6].image.sprite = selected[6];
        //titleLog.text = logSO[6].nameLog;
        //descLog.text = logSO[6].descLog;

        if (netcat)
        {
            for (int i = 1; i < 4; i++)
            {
                log[i].SetActive(false);
            }
            LogManager(16);
        }
        if (netcat2)
        {
            log[2].SetActive(false);
            log[3].SetActive(false);
            LogManager(16);
            LogManager(17);
        }
        if (netcat3)
        {
            log[3].SetActive(false);
            LogManager(16);
            LogManager(17);
            LogManager(18);
        }
        if (netcat4)
        {
            LogManager(16);
            LogManager(17);
            LogManager(18);
            LogManager(19);
        }
    }

}
