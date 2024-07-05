using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


public class QuestGiver : MonoBehaviour
{
    [Header("Chat")]
    public TMP_Text senderQuest;
    public TMP_Text descQuest;
    public TMP_Text expReward;
    private Image bgColor;

    [Header("Isi Chat")]
    public TMP_Text senderChat;
    public TMP_Text descChat;
    public TMP_Text expRewardChat;

    [Header("Panel")]
    public Button chatPanelButton;

    public GameObject uiPesanList;
    public GameObject chatPrefab;
    //private Image bgColor;
    public GameObject uiPesanPrefab;
    public GameObject btnAccPrefab;
    public GameObject btnBalasPrefab;

    [Header("Balas Chat")]
    //public GameObject bubbleChat;
    public GameObject chatSendPrefab;
    public GameObject chatReplyPrefab;
    public TMP_Text senderName;
    public TMP_Text senderSend;
    public TMP_Text sendChat;
    public TMP_Text replyChat;

    //private GameObject currentAccButton;
    public GameObject currentActionButton;
    public Button replyButtonComponent;

    [Header("Button")]
    public Button arrowBack;
    public Button kotakMasuk;

    /*[Header("Win")]
    public GameObject panelWin;
    public Button ulangi;
    public Button kembali;*/
    //public TMP_Text activeQuestTeks;

    //private QuestSO activeQuest;
    //public TMP_Text activeQuestTeks;

    [Header("Relasi")]
    public QuestManager questManager;
    public UserPrefManager user;

    private bool replyShow = false;
    private bool repButton = false;
    private bool openOrActive = false;
    private bool anotherQuest = false;

    /*[Header("Reference")]
    private List<TMP_Text> senderQuestMoves = new List<TMP_Text>();
    private List<TMP_Text> descQuestMoves = new List<TMP_Text>();
    private List<TMP_Text> expRewardMoves = new List<TMP_Text>();*/


    private void Awake()
    {
        //LoadButtonState();
        // Mengambil instance QuestManager
        //questManager = QuestManager.Instance;
        if (questManager != null)
        {
            Debug.Log("quest manager ditemukan");
        }
        else
        {
            Debug.Log("quest null");
        }

        Debug.Log("nilai reply button " + replyButtonComponent);

    }

    private void Start()
    {
        //panelWin.SetActive(false);
        arrowBack.onClick.AddListener(ButtonBack);
        kotakMasuk.onClick.AddListener(ButtonBack);
        //MenampilkanQuestSaatIni();
        OnDisplayedQuestsUpdatedHandler();
    }

    private void Update()
    {
        if (replyShow)
        {
            if (!uiPesanPrefab)
            {
                QuestManager.chatClosed = true;
            }
        }
        //MenampilkanQuestSaatIni();
        //CheckCompleted(questIni);
    }

    /*public void ViewPanelWin()
    {
        panelWin.SetActive(false);
    }*/

    private void SaveButtonState()
    {
        if (currentActionButton != null)
        {
            PlayerPrefs.SetString("CurrentActionButton", currentActionButton.name);
            Debug.Log("save current action button");
        }
        else
        {
            PlayerPrefs.SetString("CurrentActionButton", "");
        }

        if (replyButtonComponent != null)
        {
            PlayerPrefs.SetString("ReplyButtonComponent", replyButtonComponent.name);
        }
        else
        {
            PlayerPrefs.SetString("ReplyButtonComponent", "");
        }

        PlayerPrefs.SetInt("IndexQuestActive", questManager.indexQuestAktif);

        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        SaveButtonState();
    }

    public void LoadButtonState()
    {
        string currentActionButtonName = PlayerPrefs.GetString("CurrentActionButton", "");
        string replyButtonComponentName = PlayerPrefs.GetString("ReplyButtonComponent", "");
        if (!string.IsNullOrEmpty(currentActionButtonName) && !string.IsNullOrEmpty(replyButtonComponentName))
        {
            // Rekonstruksi currentActionButton
            if (currentActionButtonName.Contains("Balas"))
            {
                currentActionButton = Instantiate(btnBalasPrefab, uiPesanPrefab.transform);
                replyButtonComponent = currentActionButton.GetComponent<Button>();
                repButton = PlayerPrefs.GetInt("RepButton", 0) == 1;
                if (repButton)
                {
                    replyButtonComponent.interactable = true;
                }
                else
                {
                    replyButtonComponent.interactable = false;
                }
                
            }
            else if (currentActionButtonName.Contains("Acc"))
            {
                currentActionButton = Instantiate(btnAccPrefab, uiPesanPrefab.transform);
            }
        }

        questManager.indexQuestAktif = PlayerPrefs.GetInt("IndexQuestActive");
        
        /*if ()
        {
            // Rekonstruksi replyButtonComponent
            if (replyButtonComponentName.Contains("Balas"))
            {
                GameObject replyButtonObject = Instantiate(btnBalasPrefab, uiPesanPrefab.transform);
                replyButtonComponent = replyButtonObject.GetComponent<Button>();
            }
        }*/
    }


    private void ButtonBack()
    {
        uiPesanPrefab.SetActive(false);
    }

    private void OnEnable()
    {
        QuestManager.OnDisplayedQuestsUpdated += OnDisplayedQuestsUpdatedHandler;
    }

    private void OnDisable()
    {
        QuestManager.OnDisplayedQuestsUpdated -= OnDisplayedQuestsUpdatedHandler;
        
    }

    private void OnDisplayedQuestsUpdatedHandler()
    {
        MenampilkanQuestSaatIni();
        
    }

    public void MenampilkanQuestSaatIni()
    {
        Debug.LogWarning("masuk ke menampilkan quest");

        
        
        foreach (Transform child in uiPesanList.transform)
        {
            Destroy(child.gameObject);
        }

        // for (int i = 0; i < questManager.displayedQuests.Count; i++)
        for (int i = 0; i < questManager.displayedQuests.Count; i++)
        //foreach (QuestSO questIni in questManager.displayedQuests)
        {
            Debug.Log("total display: " + questManager.displayedQuests.Count);
            // mengambil dari active quest di quest manager, sesuai arraunya
            QuestSO questIni = questManager.displayedQuests[i];

            if (questIni != null)
            {
                GameObject pindah = Instantiate(chatPrefab, uiPesanList.transform);
                uiPesanPrefab.SetActive(false); // Mengatur awalnya menjadi tidak aktif

                TMP_Text senderQuestMove = pindah.transform.Find("NamaPengirim").GetComponent<TMP_Text>();
                TMP_Text descQuestMove = pindah.transform.Find("OverviewPesan").GetComponent<TMP_Text>();
                TMP_Text expRewardMove = pindah.transform.Find("ExpReward").GetComponent<TMP_Text>();

                bgColor = pindah.transform.Find("BgColor").GetComponent<Image>();

                senderQuestMove.text = questIni.questName;
                descQuestMove.text = questIni.overviewChat;
                expRewardMove.text = questIni.expReward.ToString() + " Exp";

                /*senderQuestMoves.Add(senderQuestMove);
                descQuestMoves.Add(descQuestMove);
                expRewardMoves.Add(expRewardMove);*/

                // Menambahkan fungsionalitas panel chat sebagai tombol
                Button chatPanelButton = pindah.GetComponent<Button>();
                chatPanelButton.onClick.AddListener(() => ChatDiklik(uiPesanPrefab, questIni));

                CheckCompleted(questIni, bgColor);
            }
            

        }

        // dieksekusi ketika availablequests 0
        if (questManager.activeQuests.Count == 0)
        {
            //questText.text = "Semua quest selesai!";
            Debug.Log("semua quest sudah selesai");
            /*panelWin.SetActive(true);
            if (Setting.isSoundOn)
            {
                Setting.backgroundSuccess.Play();
            }
            kembali.onClick.AddListener(questManager.ButtonKembaliDiklik);
            ulangi.onClick.AddListener(user.BtnUlangiClicked);*/
        }
    }

    private void CheckCompleted(QuestSO questIni, Image bgColor)
    {
        if (QuestManager.completedQuests.Contains(questIni))
        {
            Debug.LogError("Quest completed: " + questIni.questName);
            bgColor.gameObject.SetActive(true);
        }
        else
        {
            bgColor.gameObject.SetActive(false);
        }
    }

    /*private void SetNonBoldTexts(int selectedIndex)
    {
        for (int i = 0; i < senderQuestMoves.Count; i++)
        {
            if (i != selectedIndex)
            {
                senderQuestMoves[i].fontStyle = FontStyles.Normal;
                descQuestMoves[i].fontStyle = FontStyles.Normal;
                expRewardMoves[i].fontStyle = FontStyles.Normal;
            }
        }
    }*/
    public void GoesToChatDiklik(QuestSO questIni)
    {
        ChatDiklik(uiPesanPrefab, questIni);
    }


    public void ChatDiklik(GameObject uiPesanPrefab, QuestSO questIni)
    {
        Debug.Log("menampilkan deskripsi dari quest " + questIni.questName);
        // Toggle keadaan UIPesan
        uiPesanPrefab.SetActive(!uiPesanPrefab.activeSelf);
        senderChat.text = questIni.questName;
        descChat.text = questIni.questChat;
        expRewardChat.text = questIni.expReward.ToString() + " Exp";

        if (currentActionButton != null)
        {
            //Debug.LogError("button didestroy");
            Destroy(currentActionButton);
            //Debug.LogError(currentActionButton);
            //currentActionButton = null;
        }

        // Periksa apakah quest sudah aktif atau selesai
        // Periksa status quest
        bool isQuestActive = questManager.questYangAktif != null && questManager.displayedQuests[questManager.indexQuestAktif] == questIni;
        bool isQuestCompleted = QuestManager.completedQuests.Contains(questIni);

        Debug.Log("quest aktif : " + questManager.questYangAktif != null);
        //Debug.Log("quest yang mana : " + questManager.displayedQuests[questManager.indexQuestAktif]);
        Debug.Log("quest ini : " + questIni);
        Debug.Log("is quest active? " + isQuestActive);
        Debug.Log("is quest completed? " + isQuestCompleted);
        // Tetapkan tombol yang sesuai berdasarkan status quest
        if (isQuestCompleted || isQuestActive)
        {
            //Debug.Log("membuka quest yang sudah selesai");
            // Jika quest sudah aktif atau selesai, tampilkan tombol "Balas"
            GameObject replyButton = Instantiate(btnBalasPrefab, uiPesanPrefab.transform);
            replyButtonComponent = replyButton.GetComponent<Button>();
            
            currentActionButton = replyButton;
            if (isQuestCompleted)
            {
                senderSend.text = "Anda";
                sendChat.text = questIni.kirimChat;
                senderName.text = questIni.questName;
                replyChat.text = questIni.balasChat;
                chatReplyPrefab.SetActive(true);
                chatSendPrefab.SetActive(true);
            }
            else if (isQuestActive)
            {
                chatReplyPrefab.SetActive(false);
                chatSendPrefab.SetActive(false);
            }

            if (repButton)
            {
                replyButtonComponent.interactable = true;
                replyButtonComponent.onClick.AddListener(() => ShowSendMessage(questManager.questYangAktif));
            }
            else
            {
                replyButtonComponent.interactable = false;
                replyButtonComponent.onClick.AddListener(() => QuestDipilih(questIni));
            }
            
        }
        /*else if (isQuestActive)
        {
            Debug.Log("membuka quest yang sedang berjalan");
            // Jika quest sudah aktif atau selesai, tampilkan tombol "Balas"
            GameObject replyButton = Instantiate(btnBalasPrefab, uiPesanPrefab.transform);
            Button replyButtonComponent = replyButton.GetComponent<Button>();
            //replyButtonComponent.onClick.AddListener(() => QuestDipilih(questIni));
            //currentActionButton = replyButton;
            chatReplyPrefab.SetActive(false);
            chatSendPrefab.SetActive(false);
            replyButtonComponent.interactable = false;
        }*/
        else
        {
            //Debug.Log("membuka quest yang belum selesai");
            // Jika quest belum aktif atau selesai, tampilkan tombol "Terima"
            GameObject accButton = Instantiate(btnAccPrefab, uiPesanPrefab.transform);
            Button accButtonComponent = accButton.GetComponent<Button>();
            accButtonComponent.onClick.AddListener(() => QuestDipilih(questIni));
            currentActionButton = accButton;
            chatReplyPrefab.SetActive(false);
            chatSendPrefab.SetActive(false);
            anotherQuest = true;
            if (openOrActive)
            {
                //Debug.Log("openoractive dijalankan");
                GameObject replyButton = Instantiate(btnBalasPrefab, uiPesanPrefab.transform);
                replyButtonComponent = replyButton.GetComponent<Button>();
                replyButtonComponent.gameObject.SetActive(false);
            }
            
        }
        SaveButtonState();
    }

    // Fungsi untuk pemain memilih quest yang akan diaktifkan
    public void QuestDipilih(QuestSO pilihQuest)
    {
        if (currentActionButton != null)
        {
            //Debug.LogWarning("ini didestroy");
            Destroy(currentActionButton);
            //Debug.LogWarning(currentActionButton);
            currentActionButton = null; // Reset referensi
            
        }

        int pilihQuestIndex = questManager.displayedQuests.IndexOf(pilihQuest);

        if (pilihQuestIndex >= 0 && pilihQuestIndex < questManager.displayedQuests.Count)
        {
            QuestSO selectedQuest = questManager.displayedQuests[pilihQuestIndex];

            if (!QuestManager.completedQuests.Contains(selectedQuest))
            {
                //Debug.LogError("menjalankan questdipilih");
                questManager.indexQuestAktif = pilihQuestIndex;
                MenampilkanAktifQUestUI();
                questManager.AktifQuest(pilihQuestIndex);

                // Tampilkan tombol "Balas" dan aktifkan quest
                GameObject replyButton = Instantiate(btnBalasPrefab, uiPesanPrefab.transform);
                replyButtonComponent = replyButton.GetComponent<Button>();
                replyButtonComponent.onClick.AddListener(() => QuestDipilih(pilihQuest));
                replyButtonComponent.interactable = false;
                currentActionButton = replyButton;
                chatReplyPrefab.SetActive(false);
                chatSendPrefab.SetActive(false);
                openOrActive = true;
            }
            else
            {
                Debug.LogWarning("Quest sudah selesai, tidak dapat diaktifkan kembali.");
            }
        }
        else
        {
            Debug.LogWarning("Index quest yang dipilih tidak valid.");
        }

        //balasButton.SetActive(true);
    }


    public void MenampilkanAktifQUestUI()
    {
        if (questManager.indexQuestAktif != -1 && questManager.indexQuestAktif < questManager.displayedQuests.Count)
        {
            QuestSO activeQuest = questManager.displayedQuests[questManager.indexQuestAktif];
            // Menampilkan informasi quest aktif pada teks
            questManager.activeQuestTeks.text = activeQuest.activeQuestText[0];
            //QuestSO activeQuest = questManager.displayedQuests[questManager.indexQuestAktif];

            // Buat string kosong untuk menampung semua teks quest
            /*string allQuestText = "";

            // Gabungkan semua teks quest menjadi satu string
            foreach (string questText in activeQuest.activeQuestText)
            {
                allQuestText += questText + "\n"; // Tambahkan teks quest ke string allQuestText
                Debug.Log("quest text " + questText);
            }

            // Menampilkan informasi quest aktif pada teks
            questManager.activeQuestTeks.text = allQuestText;*/
            SaveButtonState();
        }
        else
        {
            Debug.Log("Tidak ada quest aktif.");
        }
    }

    public void ButtonReplyActive()
    {
        if (anotherQuest)
        {
            replyButtonComponent.interactable = true;
        }
        replyButtonComponent.interactable = true;
        repButton = true;
        PlayerPrefs.SetInt("RepButton", repButton ? 1 : 0);
        PlayerPrefs.Save();
        replyButtonComponent.onClick.AddListener(() => ShowSendMessage(questManager.questYangAktif));

    }

    private void ShowSendMessage(QuestSO quest)
    {
        senderSend.text = "Anda";
        sendChat.text = quest.kirimChat;

        chatSendPrefab.SetActive(true);

        StartCoroutine(ShowReplyMessage(quest));
    }

    private IEnumerator ShowReplyMessage(QuestSO quest)
    {
        yield return new WaitForSecondsRealtime(1f);

        senderName.text = quest.questName;
        replyChat.text = quest.balasChat;

        chatReplyPrefab.SetActive(true); 

        questManager.CompleteQuest(quest);

        questManager.GantiQuest();

        questManager.CekQuestStep();

        //replyShow = true;

        replyButtonComponent.interactable = false;
        repButton = false;
        PlayerPrefs.SetInt("RepButton", repButton ? 1 : 0);
        PlayerPrefs.Save();
        openOrActive = false;
        anotherQuest = false;
        //Destroy(replyButtonComponent);
        //Destroy(currentActionButton);

        //Image bgColor = currentActionButton.GetComponent<Image>();
        //Image bgColor = chatPrefab.GetComponent<Image>();
        //CheckCompleted(quest, bgColor);
        MenampilkanQuestSaatIni();
    }



}