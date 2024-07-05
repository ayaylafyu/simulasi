using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    // display quest
    // 3 = quest awal
    // 1 = quest intro snort sniffer, 1 = quest intro snort packet, 2 = quest snort
    // 1 = quest intro nmap, 2 = quest nmap
    // 1 = quest intro exploit, 2 = quest exploit
    // 1 = quest intro os, 1 = quest os
    // 1 = quest netscan
    // 1 = quest intro netcat, 3 = quest netcat
    private int[] questsDitampilkan = {3, 1, 1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 3, 0};
    private int perulanganKe;

    public static QuestManager Instance { get; private set; } // Singleton instance

    // pengaturan quest avail, active, complete, display
    public List<QuestSO> allQuests = new List<QuestSO>();
    public List<QuestSO> availableQuests = new List<QuestSO>();
    public List<QuestSO> activeQuests = new List<QuestSO>();
    public static List<QuestSO> completedQuests = new List<QuestSO>();
    public List<QuestSO> displayedQuests = new List<QuestSO>();
    public static event Action OnDisplayedQuestsUpdated;
    public static bool SendMessageQuestCompleted { get; private set; } = false;

    [Header("Check Quest")]
    // int pengecekan quest
    public int indexQuestAktif = -1;
    //public int stepSaatIni = 0;
    public int indexStepSaatIni;
    public int maxQuestSteps;
    public int indexActiveQuestText;

    [Header("Active Quest")]
    public QuestSO questYangAktif;
    //private QuestSO questSebelum;

    [Header("Script Relasi")]
    public QuestGiver questGiver;    
    public TerminalManager terminal;
    public FileManager fileManager;
    public BugDBManager bugDB;
    public FindbookSearch findbook;
    public HelpManager help;
    public ApreciateManager apre;
    public HintManager hint;
    public Player playerScript;
    public ButtonActiveQuest misi;
    public UserPrefManager user;
    //public Setting setting;
    //public WifiManager wifi;

    [Header("Aktif Quest Desc")]
    public TMP_Text activeQuestTeks;
    public TMP_Text previousTeks;
    public TMP_Text nextTeks;
    public TMP_Text totalStep;
    public GameObject activeQuestObject;
    public GameObject previousObject;
    public GameObject nextObject;
    public GameObject allTextObject;
    public Image[] logMisi;
    public Slider sliderWifi;
    public Image fill;
    public int minTime;
    public int currentTime;
    private Coroutine timerCoroutine; // Coroutine untuk mengurangi waktu

    //public Image fillImage;
    private Button replyButton;
    public TMP_InputField userinput;

    [Header("Button Click")]
    //public List<Button> allButtons = new List<Button>();
    public Button[] allButtons;

    public static bool apresiasiClosed;
    

    [Header("Button Software")]
    public Button softDmail;
    public Button softFile;
    public Button softBrowser;
    public Button bookFind;

    public GameObject[] panelPopUp;

    [Header("Game Over")]
    public GameObject panelGameOver;
    public GameObject panelWin;
    public Button[] kembali;
    public Button[] ulangi;

    private string expectedButtonTag;
    private UnityAction listener;
    //private Dictionary<Button, UnityAction> buttonListeners = new Dictionary<Button, UnityAction>();
    private Dictionary<Button, List<UnityAction>> buttonListeners = new Dictionary<Button, List<UnityAction>>();

    private bool userFound = false;

    public static bool chatClosed = false;

    private string userToFind;

    private bool indexStepZeroExecuted;

    

    [Header("Hint")]
    public Button ikonHint;
    public GameObject popUpConfirm;
    public Button noShow;
    public Button yesShow;
    public static bool showHint;
    public TMP_Text emptyHintText;

    [Header("Tutorial")]
    public GameObject[] panelTutorial;
    public Button[] buttonNext;

    [Header("Save Quest Manager Data")]
    private const string CURRENT_TIMER_KEY = "CurrentTimer";
    private const string CURRENT_QUEST_KEY = "CurrentQuest";
    private const string CURRENT_STEP_KEY = "CurrentStep";
    private const string PERULANGAN_KEY = "PerulanganKe";

    private bool isGameOver = false;

    //private const string CURRENT_HINT_KEY = "CurrentHint";


    private void Awake()
    {



        //sliderWifi.value = 0f;
        //questGiver = QuestGiver.instance;
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("instance game manager = this");
        }
        else
        {
            Destroy(gameObject);
        }

        if (fileManager != null)
        {
            Debug.Log("ini filemanager di questmanager");
        }
        else
        {
            Debug.Log("filemanager masih belum ketemu");
        }

        if (terminal != null)
        {
            Debug.Log("ini terminal di questmanager");
        }
        else
        {
            Debug.Log("terminal masih belum ketemu");
        }

        if (bugDB != null)
        {
            Debug.Log("ini bugdb di questmanager");
        }
        else
        {
            Debug.Log("bugdb masih belum ketemu");
        }
        if (findbook != null)
        {
            Debug.Log("ini findbook di questmanager");
        }
        else
        {
            Debug.Log("findbook masih belum ketemu");
        }
        if (questGiver != null)
        {
            Debug.Log("ini questgiver di questmanager");
        }
        else
        {
            Debug.Log("questgiver masih belum ketemu");
        }
        if (help != null)
        {
            Debug.Log("ini help di questmanager");
        }
        else
        {
            Debug.Log("help masih belum ketemu");
        }
        if (apre != null)
        {
            Debug.Log("ini apre di questmanager");
        }
        else
        {
            Debug.Log("apre masih belum ketemu");
        }


    }


    private void Start()
    {
        StartQuestManager();
    }

    public void StartQuestManager()
    {
        panelGameOver.SetActive(false);
        panelWin.SetActive(false);
        //currentTime = 100;

        displayedQuests.Clear();
        activeQuests.Clear();
        completedQuests.Clear();
        availableQuests.Clear();
        availableQuests.AddRange(allQuests);

        if (!PlayerPrefs.HasKey(CURRENT_QUEST_KEY) || !PlayerPrefs.HasKey(CURRENT_STEP_KEY) || PlayerPrefs.GetInt(CURRENT_TIMER_KEY) == 0)
        {
            currentTime = 600;
            CallHint(14);
            previousObject.SetActive(false);
            nextObject.SetActive(false);
            totalStep.gameObject.SetActive(false);
            ThisText(activeQuestTeks.text);
            Debug.Log("nilai current time sekarang " + currentTime);
            
        }
        else
        {
            questGiver.LoadButtonState();
            currentTime = PlayerPrefs.GetInt(CURRENT_TIMER_KEY);
            Debug.Log("nilai current time sekarang " + currentTime);
            LoadCurrentQuest();
            
        }
        Debug.Log("nilai current time sekarang " + currentTime);
        //Debug.Log("nilai current time sekarang " + currentTime);
        StartTimer();


        Debug.Log("have key? " + PlayerPrefs.HasKey(CURRENT_TIMER_KEY));
        /*currentTime = PlayerPrefs.GetInt(CURRENT_TIMER_KEY);

        if (!PlayerPrefs.HasKey(CURRENT_TIMER_KEY))
        {
            
            Debug.Log("nilai current time sekarang " + currentTime);
            
        }
        else
        {
            
            Debug.Log("nilai current time sekarang " + currentTime);
            
        }*/
        
        WifiColor(sliderWifi.value);

        perulanganKe = PlayerPrefs.GetInt(PERULANGAN_KEY);
        Debug.Log("nilai perulangan ke " + perulanganKe);
        if (!PlayerPrefs.HasKey(PERULANGAN_KEY) && !PlayerPrefs.HasKey("AvailableQuests"))
        {
            perulanganKe = 0;
            
            MenampilkanQuestInspektor();
        }
        else
        {
            perulanganKe = PlayerPrefs.GetInt(PERULANGAN_KEY);
            
            LoadAvailableQuests();
            LoadActiveQuests();
        }


        LoadCompletedQuests();
        LoadQuestBooleans();
        LoadLogMisiColors();

        

        for (int i = 0; i < panelPopUp.Length; i++)
        {
            panelPopUp[i].SetActive(false);
        }

        if (!PlayerPrefs.HasKey("MainMenuOpened"))
        {
            if (displayedQuests.Count > 0)
            {
                AktifQuest(0);
                //questYangAktif = availableQuests[0];
                activeQuestTeks.text = questYangAktif.activeQuestText[indexStepSaatIni];
                ShowTutorial2();
            }
            else
            {
                Debug.Log("Tidak ada quest yang ditampilkan");
            }

            //help.ButtonHelpClicked(0);

            // Setelah selesai inisialisasi, tandai bahwa game telah dibuka
            PlayerPrefs.SetInt("MainMenuOpened", 1);
            PlayerPrefs.Save();

        }
        else
        {
            // Jika "GameOpened" sudah ada dan bernilai 1, tampilkan popup
            if (PlayerPrefs.GetInt("MainMenuOpened") == 1)
            {
                panelPopUp[4].SetActive(true);
                panelTutorial[11].SetActive(false);
            }
        }


        popUpConfirm.SetActive(false);

        

        SendMessageQuestCompleted = false;
        userFound = false;

        emptyHintText.gameObject.SetActive(false);

        buttonNext[0].onClick.AddListener(ShowTutorial2);
        buttonNext[1].onClick.AddListener(ShowTutorial3);
        buttonNext[2].onClick.AddListener(ShowTutorial4);
        buttonNext[3].onClick.AddListener(ShowTutorial5);
        buttonNext[4].onClick.AddListener(ShowTutorial6);
        buttonNext[5].onClick.AddListener(ShowTutorial7);

        if (!PlayerPrefs.HasKey("softDmailInteractable"))
        {
            softDmail.interactable = false;
            softBrowser.interactable = false;
            //softDmail.SetActive(false);
            softFile.interactable = false;
            bookFind.interactable = false;
        }
        else
        {
            LoadInteractableStatus();
        }
    }
    private void Update()
    {
        WifiColor(sliderWifi.value);
        //misi.LogMisi();
    }

    private void ShowTutorial2()
    {
        panelTutorial[11].SetActive(true);
        panelTutorial[0].SetActive(true);

        if (buttonNext[0].onClick != null)
        {
            
            panelTutorial[0].SetActive(false);

            panelTutorial[1].SetActive(true);
            panelTutorial[2].SetActive(true);
        }
    }

    private void ShowTutorial3()
    {
        panelTutorial[1].SetActive(false);
        panelTutorial[2].SetActive(false);

        panelTutorial[3].SetActive(true);
        panelTutorial[4].SetActive(true);
    }

    private void ShowTutorial4()
    {
        panelTutorial[3].SetActive(false);
        panelTutorial[4].SetActive(false);

        panelTutorial[5].SetActive(true);
        panelTutorial[6].SetActive(true);
    }

    private void ShowTutorial5()
    {
        panelTutorial[5].SetActive(false);
        panelTutorial[6].SetActive(false);

        panelTutorial[7].SetActive(true);
        panelTutorial[8].SetActive(true);
    }

    private void ShowTutorial6()
    {
        panelTutorial[7].SetActive(false);
        panelTutorial[8].SetActive(false);

        panelTutorial[9].SetActive(true);
        panelTutorial[10].SetActive(true);
        panelTutorial[12].SetActive(true);
        panelTutorial[13].SetActive(true);
        panelTutorial[14].SetActive(true);

        softDmail.interactable = true;
        softBrowser.interactable = true;
        //softDmail.SetActive(false);
        softFile.interactable = true;
        bookFind.interactable = true;
    }

    private void ShowTutorial7()
    {
        panelTutorial[9].SetActive(false);
        panelTutorial[10].SetActive(false);
        panelTutorial[11].SetActive(false);
        panelTutorial[12].SetActive(false);
        panelTutorial[13].SetActive(false);
        panelTutorial[14].SetActive(false);

        softDmail.interactable = false;
        softBrowser.interactable = false;
        //softDmail.SetActive(false);
        softFile.interactable = false;
        bookFind.interactable = false;


        help.ButtonHelpClicked(0);

    }

    private void SaveCompletedQuests()
    {
        List<string> questNames = new List<string>();
        foreach (var quest in completedQuests)
        {
            questNames.Add(quest.questID);
        }

        string questsString = string.Join(",", questNames);
        Debug.Log("menyimpan quest : " + questsString);
        PlayerPrefs.SetString("CompletedQuests", questsString);
        PlayerPrefs.Save();
    }

    private void SaveAvailableQuests()
    {
        List<string> questNames = new List<string>();
        foreach (var quest in availableQuests)
        {
            questNames.Add(quest.questID); // Gunakan questID untuk menyimpan
        }

        string questsString = string.Join(",", questNames);
        PlayerPrefs.SetString("AvailableQuests", questsString);
        PlayerPrefs.Save();
        Debug.Log("Available quests saved: " + questsString);
    }

    private void SaveActiveQuests()
    {
        List<string> questNames = new List<string>();
        foreach (var quest in activeQuests)
        {
            questNames.Add(quest.questID); // Gunakan questID untuk menyimpan
        }

        string questsString = string.Join(",", questNames);
        PlayerPrefs.SetString("ActiveQuests", questsString);
        PlayerPrefs.Save();
        Debug.Log("Active quests saved: " + questsString);
    }

    private void SaveQuestBooleans()
    {
        // save log
        PlayerPrefs.SetInt("wifiCrack", LogMisiManager.wifiCrack ? 1 : 0);
        PlayerPrefs.SetInt("userPref", LogMisiManager.userPref ? 1 : 0);
        PlayerPrefs.SetInt("openDmail", LogMisiManager.openDmail ? 1 : 0);
        PlayerPrefs.SetInt("snort", LogMisiManager.snort ? 1 : 0);
        PlayerPrefs.SetInt("snort2", LogMisiManager.snort2 ? 1 : 0);
        PlayerPrefs.SetInt("snort3", LogMisiManager.snort3 ? 1 : 0);
        PlayerPrefs.SetInt("snort4", LogMisiManager.snort4 ? 1 : 0);
        PlayerPrefs.SetInt("nmap", LogMisiManager.nmap ? 1 : 0);
        PlayerPrefs.SetInt("nmap2", LogMisiManager.nmap2 ? 1 : 0);
        PlayerPrefs.SetInt("nmap3", LogMisiManager.nmap3 ? 1 : 0);
        PlayerPrefs.SetInt("exploit", LogMisiManager.exploit ? 1 : 0);
        PlayerPrefs.SetInt("exploit2", LogMisiManager.exploit2 ? 1 : 0);
        PlayerPrefs.SetInt("exploit3", LogMisiManager.exploit3 ? 1 : 0);
        PlayerPrefs.SetInt("osFinger", LogMisiManager.osFinger ? 1 : 0);
        PlayerPrefs.SetInt("osFinger2", LogMisiManager.osFinger2 ? 1 : 0);
        PlayerPrefs.SetInt("netScan", LogMisiManager.netScan ? 1 : 0);
        PlayerPrefs.SetInt("netcat", LogMisiManager.netcat ? 1 : 0);
        PlayerPrefs.SetInt("netcat2", LogMisiManager.netcat2 ? 1 : 0);
        PlayerPrefs.SetInt("netcat3", LogMisiManager.netcat3 ? 1 : 0);
        PlayerPrefs.SetInt("netcat4", LogMisiManager.netcat4 ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Quest booleans saved.");
        Debug.Log("save log misi wifi crack = " + LogMisiManager.wifiCrack);
    }

    private void SaveInteractableStatus()
    {
        // software
        PlayerPrefs.SetInt("softDmailInteractable", softDmail.interactable ? 1 : 0);
        PlayerPrefs.SetInt("softBrowserInteractable", softBrowser.interactable ? 1 : 0);
        PlayerPrefs.SetInt("softFileInteractable", softFile.interactable ? 1 : 0);
        PlayerPrefs.SetInt("bookFindInteractable", bookFind.interactable ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Interactable status saved.");
        Debug.Log("softdmail interactable " + softDmail.interactable);
    }

    private void SaveLogMisiColors()
    {
        for (int i = 0; i < logMisi.Length; i++)
        {
            Color32 color = logMisi[i].color;
            string colorString = $"{color.r},{color.g},{color.b},{color.a}";
            PlayerPrefs.SetString($"logMisiColor{i}", colorString);
        }
        PlayerPrefs.Save();
        Debug.Log("LogMisi colors saved.");
    }


    private void LoadCompletedQuests()
    {
        Debug.Log("load game");
        string questsString = PlayerPrefs.GetString("CompletedQuests", "");
        if (!string.IsNullOrEmpty(questsString))
        {
            Debug.Log("string is not null");
            Debug.Log("ques string " + questsString);
            string[] questNames = questsString.Split(',');
            foreach (var questName in questNames)
            {
                Debug.Log("Finding quest with ID: " + questName);
                QuestSO quest = FindQuestByName(questName);
                if (quest != null)
                {
                    completedQuests.Add(quest);

                    Debug.Log("quest yang disimpan : " + quest); 
                }
                else
                {
                    Debug.LogWarning("Quest with ID " + questName + " not found");
                }
            }
        }
    }

    private void LoadAvailableQuests()
    {
        Debug.Log("Loading available quests");
        string questsString = PlayerPrefs.GetString("AvailableQuests");
        if (!string.IsNullOrEmpty(questsString))
        {
            Debug.Log("Available quests string is not null");
            Debug.Log("Available quests string: " + questsString);
            string[] questNames = questsString.Split(',');
            availableQuests.Clear(); // Bersihkan daftar sebelum memuat
            foreach (var questName in questNames)
            {
                Debug.Log("Finding available quest with ID: " + questName);
                QuestSO quest = FindQuestByName(questName);
                if (quest != null)
                {
                    availableQuests.Add(quest);
                    Debug.Log("Available quest found and add: " + quest.questID);
                }
                else
                {
                    Debug.LogWarning("Available quest with ID " + questName + " not found");
                }
            }
        }
        else
        {
            Debug.Log("No available quests to load");
        }
        MenampilkanQuestInspektor();
    }

    private void LoadCurrentQuest()
    {
        string currentQuestID = PlayerPrefs.GetString(CURRENT_QUEST_KEY, "NONE");
        if (currentQuestID != "NONE")
        {
            questYangAktif = FindQuestByName(currentQuestID);
            if (questYangAktif != null)
            {
                maxQuestSteps = questYangAktif.questSteps.Count;
                indexStepSaatIni = PlayerPrefs.GetInt(CURRENT_STEP_KEY); // Atur sesuai kebutuhan Anda
                //questGiver.ChatDiklik(uiPesan, questYangAktif);
                Debug.Log("Quest yang aktif dimuat: " + questYangAktif.questID);
                CekQuestStep();
            }
            else
            {
                Debug.LogWarning("Quest dengan ID " + currentQuestID + " tidak ditemukan.");
                
            }
        }
        else
        {
            questYangAktif = null;
            CallHint(14);
            previousObject.SetActive(false);
            nextObject.SetActive(false);
            totalStep.gameObject.SetActive(false);
            ThisText(activeQuestTeks.text);
        }
    }

    private void LoadActiveQuests()
    {
        Debug.Log("Loading available quests");
        string questsString = PlayerPrefs.GetString("ActiveQuests", "");
        if (!string.IsNullOrEmpty(questsString))
        {
            Debug.Log("Active quests string is not null");
            Debug.Log("Active quests string: " + questsString);
            string[] questNames = questsString.Split(',');
            activeQuests.Clear(); // Bersihkan daftar sebelum memuat
            foreach (var questName in questNames)
            {
                Debug.Log("Finding active quest with ID: " + questName);
                QuestSO quest = FindQuestByName(questName);
                if (quest != null)
                {
                    activeQuests.Add(quest);
                    Debug.Log("Available quest found and add: " + quest.questID);
                }
                else
                {
                    Debug.LogWarning("Available quest with ID " + questName + " not found");
                }
            }
        }
        else
        {
            Debug.Log("No available quests to load");
        }
    }

    private void LoadQuestBooleans()
    {
        // load log
        LogMisiManager.wifiCrack = PlayerPrefs.GetInt("wifiCrack", 0) == 1;
        LogMisiManager.userPref = PlayerPrefs.GetInt("userPref", 0) == 1;
        LogMisiManager.openDmail = PlayerPrefs.GetInt("openDmail", 0) == 1;
        LogMisiManager.snort = PlayerPrefs.GetInt("snort", 0) == 1;
        LogMisiManager.snort2 = PlayerPrefs.GetInt("snort2", 0) == 1;
        LogMisiManager.snort3 = PlayerPrefs.GetInt("snort3", 0) == 1;
        LogMisiManager.snort4 = PlayerPrefs.GetInt("snort4", 0) == 1;
        LogMisiManager.nmap = PlayerPrefs.GetInt("nmap", 0) == 1;
        LogMisiManager.nmap2 = PlayerPrefs.GetInt("nmap2", 0) == 1;
        LogMisiManager.nmap3 = PlayerPrefs.GetInt("nmap3", 0) == 1;
        LogMisiManager.exploit = PlayerPrefs.GetInt("exploit", 0) == 1;
        LogMisiManager.exploit2 = PlayerPrefs.GetInt("exploit2", 0) == 1;
        LogMisiManager.exploit3 = PlayerPrefs.GetInt("exploit3", 0) == 1;
        LogMisiManager.osFinger = PlayerPrefs.GetInt("osFinger", 0) == 1;
        LogMisiManager.osFinger2 = PlayerPrefs.GetInt("osFinger2", 0) == 1;
        LogMisiManager.netScan = PlayerPrefs.GetInt("netScan", 0) == 1;
        LogMisiManager.netcat = PlayerPrefs.GetInt("netcat", 0) == 1;
        LogMisiManager.netcat2 = PlayerPrefs.GetInt("netcat2", 0) == 1;
        LogMisiManager.netcat3 = PlayerPrefs.GetInt("netcat3", 0) == 1;
        LogMisiManager.netcat4 = PlayerPrefs.GetInt("netcat4", 0) == 1;
        Debug.Log("log misi wifi crack = " + LogMisiManager.wifiCrack);
        Debug.Log("log misi nmap = " + LogMisiManager.nmap);
        Debug.Log("log misi nmap2 = " + LogMisiManager.nmap2);
        Debug.Log("Quest booleans loaded.");
    }

    private void LoadInteractableStatus()
    {
        softDmail.interactable = PlayerPrefs.GetInt("softDmailInteractable", 0) == 1;
        softBrowser.interactable = PlayerPrefs.GetInt("softBrowserInteractable", 0) == 1;
        softFile.interactable = PlayerPrefs.GetInt("softFileInteractable", 0) == 1;
        bookFind.interactable = PlayerPrefs.GetInt("bookFindInteractable", 0) == 1;
        Debug.Log("Interactable statuses loaded.");
        Debug.Log("load softdmail interactable " + softDmail.interactable);
        Debug.Log("load softbrow interactable " + softBrowser.interactable);
        Debug.Log("load softfile interactable " + softFile.interactable);
    }

    private void LoadLogMisiColors()
    {
        for (int i = 0; i < logMisi.Length; i++)
        {
            string colorString = PlayerPrefs.GetString($"logMisiColor{i}", "255,255,255,255"); // Default to white
            string[] colorValues = colorString.Split(',');

            if (colorValues.Length == 4 &&
                byte.TryParse(colorValues[0], out byte r) &&
                byte.TryParse(colorValues[1], out byte g) &&
                byte.TryParse(colorValues[2], out byte b) &&
                byte.TryParse(colorValues[3], out byte a))
            {
                logMisi[i].color = new Color32(r, g, b, a);
            }
        }
        Debug.Log("LogMisi colors loaded.");
    }

    private QuestSO FindQuestByName(string questName)
    {
        // Implementasikan logika untuk mencari QuestSO berdasarkan nama.
        // Misalnya, mencari di dalam list semua QuestSO yang ada.
        foreach (var quest in allQuests)
        {
            if (quest.questID == questName)
            {
                return quest;
            }
        }
        return null;
    }
 
    private void StartTimer()
    {
        // Pastikan coroutine belum berjalan sebelumnya
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        // Memulai coroutine untuk mengurangi waktu
        timerCoroutine = StartCoroutine(CountdownTimer());
    }

    // Coroutine untuk mengurangi waktu
    private IEnumerator CountdownTimer()
    {
        //currentTime = minTime;
        Debug.Log("nilai current time sekarang " + currentTime);
        while (currentTime > 0)
        {
            yield return new WaitForSecondsRealtime(1f); // Tunggu 1 detik
            currentTime -= 1; // Kurangi waktu sebanyak 1 detik

            // Update nilai slider
            sliderWifi.value = currentTime / 600.0f;

            PlayerPrefs.SetInt(CURRENT_TIMER_KEY, currentTime);
            //Debug.Log("nilai current time : " + currentTime);

            if (currentTime == 1)
            {
                isGameOver = true;
            }
        }

        // Lakukan sesuatu ketika waktu habis (misalnya, tampilkan pesan atau panggil fungsi tertentu)
        
        if (currentTime == 0 && isGameOver)
        {
            Debug.Log("Waktu telah habis!");
            panelGameOver.SetActive(true);
            if (Setting.isSoundOn)
            {
                Setting.backgroundGameOver.Play();
            }
            
            kembali[0].onClick.AddListener(ButtonKembaliDiklik);
            ulangi[0].onClick.AddListener(user.BtnUlangiClicked);
            isGameOver = false;
        }

    }
    public void ButtonKembaliDiklik()
    {
        SceneManager.LoadScene("MainMenu");
        user.BtnUlangiClicked();
    }

    private void MenampilkanQuestInspektor()
    {
        int questsDiPerulanganIni = questsDitampilkan[perulanganKe];
        Debug.Log("nilai quest di perulangan : " + questsDiPerulanganIni);

        for (int i = 0; i < questsDiPerulanganIni; i++)
        {

            if (availableQuests.Count > 0)
            {
                QuestSO questIni = availableQuests[0]; // Ambil quest pertama dari availableQuests
                if (!displayedQuests.Contains(questIni))
                {
                    activeQuests.Add(questIni); // Tambahkan ke activeQuests
                    displayedQuests.Add(questIni); // tambahkan ke displayedQuests
                    availableQuests.RemoveAt(0); // Hapus dari availableQuests

                    //RemoveQuests(questIni);


                    
                }
            }
            else
            {
                // Semua quest selesai, berhenti menampilkan quest

                break;
            }
        }

        


    }

    

    public void AktifQuest(int indexQuest)
    {
        // quest yang sedang aktif sama dengan quest yang ditampilkan pada index ke- (quest yang dipilih)
        questYangAktif = displayedQuests[indexQuest];
        //questSebelum = questYangAktif;
        Debug.Log("quest yang aktif " + questYangAktif.questID);
        // maksimal step nya sama dengan jumlah step pada quest yang sedang aktif
        maxQuestSteps = questYangAktif.questSteps.Count;
        // mengembalikan nilai index step saat ini ke 0 setiap kali ada quest baru
        indexStepSaatIni = 0;
        // index quest yang sedang aktif
        //indexQuestAktif = indexQuest;
        PlayerPrefs.SetString(CURRENT_QUEST_KEY, questYangAktif.questID);

        // memanggil metode cek quest step untuk mengecek step step nya
        CekQuestStep();
    }

    public  void CekQuestStep()
    {
        Debug.Log("current step index di luar if : " + indexStepSaatIni);
        SendMessageQuestCompleted = false;

        if (questYangAktif != null)
        {
            HelpViewer();

            

            // index step kurang dari jumlah max step
            if (indexStepSaatIni >= 0 && indexStepSaatIni < maxQuestSteps)
            {
                totalStep.text = indexStepSaatIni + 1  + "/" + maxQuestSteps;
                totalStep.gameObject.SetActive(true);

                previousObject.SetActive(indexStepSaatIni > 0);
                nextObject.SetActive(indexStepSaatIni < maxQuestSteps - 1);
                

                // step saat ini mengambil nilai dari method get quest step

                //QuestSO.Quest stepSaatIni = GetQuestStep();
                Debug.Log("Index step saat ini " + indexStepSaatIni);

                QuestSO.Quest stepSaatIni = questYangAktif.questSteps[indexStepSaatIni];
                //QuestSO.Quest stepSaatIni = GetQuestStep();
                Debug.Log("step saat ini " + stepSaatIni.questType);
                if (indexStepSaatIni > 0)
                {
                    previousTeks.text = questYangAktif.activeQuestText[indexStepSaatIni - 1];
                    PreviousText(previousTeks.text);
                }
                
                activeQuestTeks.text = questYangAktif.activeQuestText[indexStepSaatIni];
                ThisText(activeQuestTeks.text);
                if (indexStepSaatIni < maxQuestSteps - 1)
                {
                    nextTeks.text = questYangAktif.activeQuestText[indexStepSaatIni + 1];
                    NextText(nextTeks.text);
                }

                if (stepSaatIni.questType == QuestType.InputCode)
                {
                    userinput.onEndEdit.RemoveAllListeners();
                    userinput.onEndEdit.AddListener((userInput) => HandleInputCodeQuest(userInput, stepSaatIni));
                }
                else if (stepSaatIni.questType == QuestType.DownloadFile)
                {
                    HandleDownloadFileQuest(stepSaatIni);
                }
                else if (stepSaatIni.questType == QuestType.SendMessage)
                {
                    HandleSendMessageQuest(stepSaatIni);
                }
                else if (stepSaatIni.questType == QuestType.ClickButton)
                {
                    Debug.Log("mencari button");

                    for (int i = 0; i < allButtons.Length; i++)
                    {
                        Button button = allButtons[i];
                        if (!buttonListeners.ContainsKey(button))
                        {
                            // Buat list baru untuk menyimpan listener
                            List<UnityAction> listeners = new List<UnityAction>();
                            buttonListeners.Add(button, listeners);
                        }
                        //AddButtonListener(button, stepSaatIni);
                        listener = () => HandleClickButtonQuest(stepSaatIni, button);
                        // Simpan listener dalam dictionary
                        //buttonListeners.Add(button, listener);
                        buttonListeners[button].Add(listener);
                        // Tambahkan listener ke tombol
                        button.onClick.AddListener(listener);

                    }
                }

                PlayerPrefs.SetInt(CURRENT_STEP_KEY, indexStepSaatIni);
            }
        }
        else if (questYangAktif == null)
        {
            CallHint(14);
            previousObject.SetActive(false);
            nextObject.SetActive(false);
            totalStep.gameObject.SetActive(false);
            ThisText(activeQuestTeks.text);
        }
    }

    private void PreviousText(string previousTxt)
    {

        if (previousObject != null && previousTeks != null)
        {
            //Debug.Log("bubble kirim ditemukan");

            previousTeks.text = previousTxt;

            float fixedWidth = 193f;
            float padding = 10f;

            // Atur lebar teks sesuai panjang pesan
            RectTransform textRectTransform = previousTeks.rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, previousTeks.preferredHeight);

            // Mengatur lebar dan tinggi bubble kirim agar sama dengan teks
            RectTransform bubblePreviousRectTransform = previousObject.GetComponent<RectTransform>();
            bubblePreviousRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, previousTeks.preferredHeight + padding * 2);

            // Atur lebar dan tinggi parent bubble sesuai dengan lebar dan tinggi teks
            RectTransform bubbleRectTransform = previousObject.GetComponent<RectTransform>();
            bubbleRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, previousTeks.preferredHeight + padding * 2);

            HorizontalLayoutGroup horizontal = previousObject.GetComponent<HorizontalLayoutGroup>();

            if (horizontal != null)
            {
                horizontal.childAlignment = TextAnchor.MiddleCenter;
                //horizontal.childControlWidth = true;
            }

            //Debug.Log(textRectTransform.sizeDelta);
            //Debug.Log(bubblePreviousRectTransform.sizeDelta);
            //Debug.Log(bubbleRectTransform.sizeDelta);
        }
    }

    private void ThisText(string thisTxt)
    {
        if (activeQuestObject != null && activeQuestTeks != null)
        {
            //Debug.Log("bubble kirim ditemukan");


            activeQuestTeks.text = thisTxt;

            

            float fixedWidth = 262f;
            float padding = 15f;

            // Atur lebar teks sesuai panjang pesan
            RectTransform textRectTransform = activeQuestTeks.rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, activeQuestTeks.preferredHeight);

            if (questYangAktif != null)
            {
                float totalHeight = activeQuestTeks.preferredHeight + totalStep.preferredHeight + 3f;

                // Mengatur lebar dan tinggi bubble kirim agar sama dengan teks
                RectTransform bubbleThisRectTransform = activeQuestObject.GetComponent<RectTransform>();
                bubbleThisRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, totalHeight + padding * 2);

                // Atur lebar dan tinggi parent bubble sesuai dengan lebar dan tinggi teks
                RectTransform bubbleRectTransform = activeQuestObject.GetComponent<RectTransform>();
                bubbleRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, totalHeight + padding * 2);
            }
            else if (questYangAktif == null)
            {
                float totalHeight = activeQuestTeks.preferredHeight;

                // Mengatur lebar dan tinggi bubble kirim agar sama dengan teks
                RectTransform bubbleThisRectTransform = activeQuestObject.GetComponent<RectTransform>();
                bubbleThisRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, totalHeight + padding * 2);

                // Atur lebar dan tinggi parent bubble sesuai dengan lebar dan tinggi teks
                RectTransform bubbleRectTransform = activeQuestObject.GetComponent<RectTransform>();
                bubbleRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, totalHeight + padding * 2);

                //Debug.Log(textRectTransform.sizeDelta);
                //Debug.Log(bubbleThisRectTransform.sizeDelta);
            }
            

            

            VerticalLayoutGroup vertical = activeQuestObject.GetComponent<VerticalLayoutGroup>();

            if (vertical != null)
            {
                vertical.childAlignment = TextAnchor.MiddleCenter;
                //horizontal.childControlWidth = true;
            }

            
            //Debug.Log(bubbleRectTransform.sizeDelta);
        }
    }

    private void NextText(string nextTxt)
    {

        if (nextObject != null && nextTeks != null)
        {
            //Debug.Log("bubble kirim ditemukan");

            nextTeks.text = nextTxt;

            float fixedWidth = 193f;
            float padding = 10f;

            // Atur lebar teks sesuai panjang pesan
            RectTransform textRectTransform = nextTeks.rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, nextTeks.preferredHeight);

            // Mengatur lebar dan tinggi bubble kirim agar sama dengan teks
            RectTransform bubbleNextRectTransform = nextObject.GetComponent<RectTransform>();
            bubbleNextRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, nextTeks.preferredHeight + padding * 2);

            // Atur lebar dan tinggi parent bubble sesuai dengan lebar dan tinggi teks
            RectTransform bubbleRectTransform = nextObject.GetComponent<RectTransform>();
            bubbleRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, nextTeks.preferredHeight + padding * 2);

            HorizontalLayoutGroup horizontal = nextObject.GetComponent<HorizontalLayoutGroup>();

            if (horizontal != null)
            {
                horizontal.childAlignment = TextAnchor.MiddleCenter;
                //horizontal.childControlWidth = true;
            }

            //Debug.Log(textRectTransform.sizeDelta);
            //Debug.Log(bubbleNextRectTransform.sizeDelta);
            //Debug.Log(bubbleRectTransform.sizeDelta);
        }
    }

    private void RemoveButtonListener(Button button)
    {
        if (buttonListeners.ContainsKey(button))
        {
            // Dapatkan list listener dari dictionary
            List<UnityAction> listeners = buttonListeners[button];
            // Hapus semua listener dari tombol
            foreach (UnityAction listener in listeners)
            {
                button.onClick.RemoveListener(listener);
            }
            // Hapus list listener dari dictionary
            buttonListeners.Remove(button);
        }
    }

    public void CompleteQuest(QuestSO quest)
    {
        indexStepSaatIni = 0;
        maxQuestSteps = 0;

        RewardPlayer(questYangAktif.expReward);
        activeQuestTeks.text = "Tidak ada misi yang sedang aktif";
        //indexQuestAktif = -1;



        activeQuests.Remove(quest);
        SaveActiveQuests();
        questYangAktif = null;
        //questSebelum = questYangAktif;


        if (!completedQuests.Contains(quest))
        {
            completedQuests.Add(quest);
            SaveCompletedQuests();
        }

        PlayerPrefs.SetString(CURRENT_QUEST_KEY, "NONE");

        //displayedQuests.Remove(quest);

        /*if (Player.currentLevel >= 2 && Player.currentExp > 0)
        {
            sliderWifi.value += 0.2f;
            if (sliderWifi.value == 1f)
            {
                Debug.Log("gameover");
            }
        }*/

        //Color color = WifiColor(sliderWifi.value);
        //WifiColor();
        Debug.Log("nilai slider wifi = " + sliderWifi.value);

        //GantiQuest();


    }



    public void SliderRestart()
    {
        currentTime = 600;
        WifiColor(1f);
    }

    public void GantiQuest()
    {
        indexStepZeroExecuted = false;
        // lalu jika active quests sudah tidak ada nilainya, maka 
        bool allQuestsCompleted = activeQuests.Count == 0;

        if (allQuestsCompleted)
        {
            //if (chatClosed)
            //{
            //iterasinya ditingkatkan agar quests yang ditampilkan berubah
            perulanganKe++;
            if (perulanganKe == 13)
            {
                panelWin.SetActive(true);
                if (Setting.isSoundOn)
                {
                    Setting.backgroundSuccess.Play();
                }
                kembali[1].onClick.AddListener(ButtonKembaliDiklik);
                ulangi[0].onClick.AddListener(user.BtnUlangiClicked);
            }
            displayedQuests.Clear();
            chatClosed = false;
            
            SaveAvailableQuests();
            MenampilkanQuestInspektor();
            //GantiDisplayedQuests();
            if (OnDisplayedQuestsUpdated != null)
            {
                OnDisplayedQuestsUpdated();
            }
            //}
            

        }
        else
        {
            // Jika masih ada quest yang belum selesai, lanjutkan ke iterasi selanjutnya
        }
        PlayerPrefs.SetInt(PERULANGAN_KEY, perulanganKe);
    }

    private void HandleInputCodeQuest(string userInput, QuestSO.Quest stepSaatIni)
    {
        // Mengambil input dari terminal
        userInput = terminal.userInput;

        // Mengambil langkah quest saat ini
        //QuestSO.Quest currentQuestStep = GetQuestStep();
        Debug.Log("ini nilai userInput : " + userInput);
        if (userInput == stepSaatIni.requiredInput)
        {
            // Input benar, langkah quest selesai
            Debug.Log("Correct Input! Proceed to the next step.");
            userinput.onEndEdit.RemoveAllListeners();
            indexStepSaatIni++;
            if (Setting.isSoundOn)
            {
                Setting.backgroundSuccess.time = 0.5f;
                Setting.backgroundSuccess.Play();
            }
            //indexActiveQuestText++;
            CekQuestStep();
        }
        else
        {
            // Input salah, mungkin memberikan feedback kepada pemain
            Debug.Log("Incorrect Input. Try Again!");
            userinput.text = "";
        }
    }

    private void HandleDownloadFileQuest(QuestSO.Quest stepSaatIni)
    {
        string requiredFile = stepSaatIni.requiredFile;

        Debug.Log("required filenya " + requiredFile);

        if(fileManager != null)
        {
            Debug.Log("filemanager ditemukan");
        }
        // Memeriksa apakah file yang diharapkan sudah ada di FileManager
        Debug.Log(fileManager.IsFileDownloaded(requiredFile));

        StartCoroutine(MenungguFileDidownload(requiredFile));

    }

    private IEnumerator MenungguFileDidownload(string fileName)
    {
        Debug.Log("Memeriksa apakah file sudah diunduh...");

        // Menunggu sampai file diunduh
        while (!fileManager.IsFileDownloaded(fileName))
        {

            Debug.Log("di while");
            yield return new WaitForSecondsRealtime(3f);
            yield return null; // Tunggu frame berikutnya
        }

        // Jika file sudah diunduh, lanjutkan ke langkah berikutnya
        Debug.Log("File sudah diunduh. Lanjutkan ke langkah berikutnya.");

        indexStepSaatIni++;
        if (Setting.isSoundOn)
        {
            Setting.backgroundSuccess.time = 0.5f;
            Setting.backgroundSuccess.Play();
        }
        CekQuestStep();
    }

    public void HandleSendMessageQuest(QuestSO.Quest stepSaatIni)
    {
        userToFind = stepSaatIni.userToFind;
        Debug.Log("user to find " + userToFind);

        //StartCoroutine(CheckUserAndProceed(userToFind));
    }

    public void CheckUserAndProceed(string userName)
    {
        Debug.Log("user name " + userName);
        /*while (!findbook.IsUserFound(userName))
        {
            Debug.Log("User not found! Retrying...");
            yield return new WaitForSeconds(3f); // Tunggu beberapa detik sebelum mencoba lagi
        }*/
        if (userToFind == userName)
        {
            userFound = true;
            Debug.Log("true");
        }
        
    }



    public void MessageSent()
    {
        if (userFound)
        {
            SendMessageQuestCompleted = true;
            // Jika pengguna ditemukan, lanjutkan ke langkah berikutnya
            indexStepSaatIni++;
            if (Setting.isSoundOn)
            {
                Setting.backgroundSuccess.time = 0.5f;
                Setting.backgroundSuccess.Play();
            }
            CekQuestStep();
        }

    }

    private void HandleClickButtonQuest(QuestSO.Quest stepSaatIni, Button clickedButton)
    {
        // Mendapatkan tag tombol yang diklik
        string buttonTag = clickedButton.tag;
        Debug.Log("buttontag " + buttonTag);
        // Mendapatkan tag yang diharapkan dari quest step saat ini
        expectedButtonTag = stepSaatIni.requiredButtonTag;
        Debug.Log("expectedButtonTag " + expectedButtonTag);
        // Jika tag tombol yang diklik sesuai dengan yang diharapkan
        if (buttonTag == expectedButtonTag)
        {
            Debug.Log(buttonTag + " == " + expectedButtonTag);
            // Lanjutkan quest ke langkah berikutnya
            RemoveButtonListener(clickedButton);
            indexStepSaatIni++;
            if (Setting.isSoundOn)
            {
                Setting.backgroundSuccess.time = 0.5f;
                Setting.backgroundSuccess.Play();
            }
            CekQuestStep();
        }
        else
        {
            // Jika tag tombol yang diklik tidak sesuai, berikan umpan balik kepada pemain
            Debug.Log("Incorrect Button Clicked! Try Again!");
        }
    }
    private void RewardPlayer(int expReward)
    {
        Debug.Log("Player menerima reward: " + expReward + " exp");

        // Ambil referensi ke skrip player (pastikan skrip tersebut memiliki fungsi untuk menangani peningkatan level)
        //Player playerScript = FindObjectOfType<Player>();

        if (playerScript != null)
        {
            // Tambahkan exp ke player
            Debug.Log("player ketemu");
            playerScript.AddExperience(expReward);
        }
        else
        {
            Debug.LogError("PlayerScript tidak ditemukan!");
        }
    }

    private void WifiColor(float value)
    {
        if (value <= 0.2f)
        {
            fill.color = Color.red;
        }
        else if (value <= 0.4f)
        {
            fill.color = new Color(1f, 0.5f, 0f); // Orange
        }
        else if (value <= 0.6f)
        {
            fill.color = Color.yellow;
        }
        else if (value <= 0.8f)
        {
            fill.color = new Color(0.6901961f, 1f, 0f); // Light green (a bit yellowish)
        }
        else if (value <= 1.0f)
        {
            fill.color = Color.green;
        }
    }

    public void CallHint(int index)
    {
        ikonHint.onClick.AddListener(() => PopUpShow(index));
        yesShow.onClick.AddListener(() => ShowHint(index));        
    }

    private IEnumerator ShowEmptyHint()
    {
        emptyHintText.gameObject.SetActive(true);
        emptyHintText.text = "Hint telah habis";

        yield return new WaitForSecondsRealtime(2f);

        emptyHintText.gameObject.SetActive(false);
    }

    private void ShowHint(int index)
    {
        if (Player.hintEmpty)
        {
            StartCoroutine(ShowEmptyHint());
        }
        else
        {
            popUpConfirm.SetActive(false);
            hint.PopUpHint(index);
            showHint = true;
            Player.isHintOpen = true;
        }
        
    }

    private void PopUpShow(int index)
    {
        if (!showHint)
        {
            popUpConfirm.SetActive(true);
        }
        else if (showHint)
        {
            hint.PopUpHint(index);
        }
    }

    public void HelpViewer()
    {
        Debug.Log("helpviewer");
        //quest wifi cracking saat pertama kali dibuka

        //QuestSO questSebelum = questYangAktif;

        Debug.Log("quest yang aktif di help viewer : " + questYangAktif.questID);
        //Debug.Log("quest sebelum : " + questSebelum);
        Debug.Log("avail quests : " + availableQuests.Count);
        Debug.Log("displayed quests : " + displayedQuests.Count);

        if (apresiasiClosed)
        {
            if (questYangAktif.questID == "0_Start")
            {
                CompleteQuest(questYangAktif);
                AktifQuest(1);
                apresiasiClosed = false;
                Debug.Log("apresiasiclosed is true");
            }
        }

        if (questYangAktif.questID == "0_Start")
        {
            //red
            logMisi[0].color = new Color32(255, 170, 0, 255);
            //yellow
            for (int i = 1; i < logMisi.Length; i++)
            {
                logMisi[i].color = new Color32(255, 238, 170, 255);
            }
            SaveLogMisiColors();

            CallHint(0);

            // menampilkan help software terminal
            if (indexStepSaatIni == 1)
            {
                help.ButtonHelpClicked(1);
            }
            // menampilkan help cracking wifi
            else if (indexStepSaatIni == 2)
            {
                help.ButtonHelpClicked(2);
            
            }
            else if (indexStepSaatIni == 12)
            {
                LogMisiManager.wifiCrack = true;
                SaveQuestBooleans();
            }
            // jika sudah selesai maka akan lanjut ke quest user preferences
            /*else if (apresiasiClosed)
            {
                CompleteQuest(questYangAktif);
                AktifQuest(1);
                apresiasiClosed = false;
                Debug.Log("apresiasiclosed is true");
            }*/
            else if (indexStepSaatIni == maxQuestSteps )
            {
                // TO DO
                apre.PopUpApreciate(0);
                showHint = false;

            }

        }

        // quest pengenalan software user preferences
        else if (questYangAktif.questID == "1_UserPref")
        {
            if (indexStepSaatIni == 1)
            {
                help.ButtonHelpClicked(3);
            }
            // jika sudah selesai maka software dmail akan terbuka
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.userPref = true;
                SaveQuestBooleans();
                CompleteQuest(questYangAktif);
                AktifQuest(2);
                //AktifQuest(1);
                apresiasiClosed = false;

                //softDmail.SetActive(true);
                softDmail.interactable = true;
                SaveInteractableStatus();
                Debug.Log("software dmail : " + softDmail.interactable);
            }
            
        }

        // quest membuka software dmail
        else if (questYangAktif.questID == "2_OpDmail")
        {
            if (indexStepSaatIni == 0)
            {
                help.ButtonHelpClicked(4);
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.openDmail = true;
                SaveQuestBooleans();
                CompleteQuest(questYangAktif);
                GantiQuest();
            }
        }

        // help misi [6]

        // quest intro snort
        else if (questYangAktif.questID == "3_InSnortSniffer")
        {
            //green
            logMisi[0].color = new Color32(170, 255, 170, 255);
            //red
            logMisi[1].color = new Color32(255, 170, 0, 255);
            //yellow
            for (int i = 2; i < logMisi.Length; i++)
            {
                
                logMisi[i].color = new Color32(255, 238, 170, 255);
            }
            SaveLogMisiColors();
            CallHint(1);

            if (indexStepSaatIni == 0 && !indexStepZeroExecuted)
            {
                help.ButtonHelpClicked(5);
                indexStepZeroExecuted = true;
                showHint = false;
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            /*else if (chatClosed)
            {
                GantiQuest();
                chatClosed = false;
                //CekQuestStep();
            }*/
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.snort = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(1);
                showHint = false;
            }
            
        }

        else if (questYangAktif.questID == "4_InSnortPckt")
        {
            CallHint(2);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.snort2 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(2);
                showHint = false;
                
                Debug.LogWarning(LogMisiManager.snort);
            }
        }

        else if (questYangAktif.questID == "5_SnortSniffer")
        {
            CallHint(3);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.snort3 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(3);
                showHint = false;

            }
        }

        else if (questYangAktif.questID == "6_SnortPacket")
        {
            CallHint(2);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.snort4 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(4);
                showHint = false;

            }
        }

        else if (questYangAktif.questID == "7_InNmap")
        {
            //green
            for (int i = 0; i < 2; i++)
            {
                logMisi[i].color = new Color32(170, 255, 170, 255);
            }
            //red
            logMisi[2].color = new Color32(255, 170, 0, 255);
            //yellow
            for (int i = 3; i < logMisi.Length; i++)
            {
                logMisi[i].color = new Color32(255, 238, 170, 255);
            }
            SaveLogMisiColors();

            CallHint(4);
            if (indexStepSaatIni == 0 && !indexStepZeroExecuted)
            {
                help.ButtonHelpClicked(6);
                indexStepZeroExecuted = true;
                showHint = false;
            }
            else if (indexStepSaatIni == 1)
            {
                help.ButtonHelpClicked(7);
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.nmap = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(5);
                showHint = false;
                
                Debug.LogWarning(LogMisiManager.nmap);
            }
        }

        else if (questYangAktif.questID == "8_Nmap1")
        {
            CallHint(5);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.nmap2 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(6);
                showHint = false;
            }
        }

        else if (questYangAktif.questID == "9_Nmap2")
        {
            CallHint(6);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.nmap3 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(7);
                softBrowser.interactable = true;
                softFile.interactable = true;
                showHint = false;
                SaveInteractableStatus();
            }
        }

        else if (questYangAktif.questID == "10_InExploit")
        {
            //green
            for (int i = 0; i < 3; i++)
            {
                logMisi[i].color = new Color32(170, 255, 170, 255);
            }
            //red
            logMisi[3].color = new Color32(255, 170, 0, 255);
            //yellow
            for (int i = 4; i < logMisi.Length; i++)
            {
                logMisi[i].color = new Color32(255, 238, 170, 255);
            }
            SaveLogMisiColors();

            CallHint(7);
            if (indexStepSaatIni == 0 && !indexStepZeroExecuted)
            {
                help.ButtonHelpClicked(8);
                indexStepZeroExecuted = true;
                showHint = false;
            }
            else if (indexStepSaatIni == 3)
            {
                help.ButtonHelpClicked(9);
            }
            else if (indexStepSaatIni == 5)
            {
                help.ButtonHelpClicked(10);
            }
            else if (indexStepSaatIni == 7)
            {
                help.ButtonHelpClicked(11);
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.exploit = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(8);
                showHint = false;
                
                Debug.LogWarning(LogMisiManager.exploit);
            }
        }

        else if (questYangAktif.questID == "11_Exploit1")
        {
            CallHint(8);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.exploit2 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(9);
                showHint = false;
            }
        }

        else if (questYangAktif.questID == "12_Exploit2")
        {
            CallHint(9);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.exploit3 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(10);
                showHint = false;
            }
        }

        else if (questYangAktif.questID == "13_InOS")
        {
            //green
            for (int i = 0; i < 4; i++)
            {
                logMisi[i].color = new Color32(170, 255, 170, 255);
            }
            //red
            logMisi[4].color = new Color32(255, 170, 0, 255);
            //yellow
            for (int i = 5; i < logMisi.Length; i++)
            {
                logMisi[i].color = new Color32(255, 238, 170, 255);
            }
            SaveLogMisiColors();

            CallHint(10);
            if (indexStepSaatIni == 0 && !indexStepZeroExecuted)
            {
                help.ButtonHelpClicked(12);
                indexStepZeroExecuted = true;
                showHint = false;
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.osFinger = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(11);
                showHint = false;
                
                Debug.LogWarning(LogMisiManager.osFinger);
            }
        }

        else if (questYangAktif.questID == "14_OS")
        {
            CallHint(10);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.osFinger2 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(12);
                showHint = false;
            }
        }

        else if (questYangAktif.questID == "15_NetScan")
        {
            //green
            for (int i = 0; i < 5; i++)
            {
                logMisi[i].color = new Color32(170, 255, 170, 255);
            }
            //red
            logMisi[5].color = new Color32(255, 170, 0, 255);
            //yellow
            logMisi[6].color = new Color32(255, 238, 170, 255);
            SaveLogMisiColors();

            CallHint(11);

            if (indexStepSaatIni == 0 && !indexStepZeroExecuted)
            {
                help.ButtonHelpClicked(13);
                indexStepZeroExecuted = true;
                showHint = false;
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.netScan = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(13);
                bookFind.interactable = true;
                showHint = false;
                
                Debug.LogWarning(LogMisiManager.netScan);
                SaveInteractableStatus();
            }
        }

        else if (questYangAktif.questID == "16_InNc")
        {
            //green
            for (int i = 0; i < 6; i++)
            {
                logMisi[i].color = new Color32(170, 255, 170, 255);
            }
            //red
            logMisi[6].color = new Color32(255, 170, 0, 255);
            SaveLogMisiColors();

            CallHint(12);
            if (indexStepSaatIni == 0 && !indexStepZeroExecuted)
            {
                help.ButtonHelpClicked(14);
                indexStepZeroExecuted = true;
                showHint = false;
            }
            else if (indexStepSaatIni == 2)
            {
                help.ButtonHelpClicked(15);
            }
            else if (apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.netcat = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(14);
                showHint = false;
                
                Debug.LogWarning(LogMisiManager.netcat);
            }
        }

        else if (questYangAktif.questID == "17_Nc1")
        {
            CallHint(13);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.netcat2 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(15);
                showHint = false;
            }
        }

        else if (questYangAktif.questID == "18_Nc2")
        {
            CallHint(13);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.netcat3 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(16);
                showHint = false;
            }
        }

        else if (questYangAktif.questID == "19_Nc3")
        {
            CallHint(13);
            if (indexStepSaatIni == 0)
            {
                showHint = false;
            }
            else if(apresiasiClosed)
            {
                questGiver.ButtonReplyActive();
                apresiasiClosed = false;


                Debug.Log("apresiasiclosed is true");
            }
            else if (indexStepSaatIni == maxQuestSteps)
            {
                LogMisiManager.netcat4 = true;
                SaveQuestBooleans();
                apre.PopUpApreciate(17);
                //green
                logMisi[6].color = new Color32(170, 255, 170, 255);
                showHint = false;
            }
        }

        else
        {
            Debug.Log("tidak ada quest aktif");

        }
    }
}