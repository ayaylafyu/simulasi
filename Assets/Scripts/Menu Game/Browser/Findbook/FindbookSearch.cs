using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class FindbookSearch : MonoBehaviour
{
    [Header("Search")]
    public TMP_InputField searchInput;
    public Button searchButton; // Tambahkan referensi ke tombol pencarian di Inspector Unity
    public TMP_Text notFoundText;
    

    [Header("UI Panel")]
    public GameObject searchPanel;
    public GameObject profilePanel;
    public GameObject chatPanel;

    [Header("Text User")]
    public TMP_Text tentangUserText;
    public TMP_Text namaPengguna;
    public TMP_Text namaSearch;
    public Button buttonChat;

    [Header("Pesan")]
    public TMP_InputField pesanInput;
    public Button buttonSend;
    public TMP_Text namaChat;
    public TMP_Text kirimPesanText;
    public TMP_Text balasPesanText;
    private string balasPesan;
    private bool isFirstChatButtonClick = true;

    public GameObject bubblePrefab;
    public GameObject bubbleKirimPrefab;
    public GameObject bubbleBalasPrefab;
    public GameObject bubbleMengetik;

    private GameObject bubbleMengetikInstance;
    private GameObject pindahBubbleKirimParent;
    private GameObject pindahBubbleBalasParent;

    [Header("SO User")]
    public UserSO userSO;
    public static FindbookSearch Instance;
    public QuestManager questManager;

    public static bool replyMessageShown { get; set; } = false;

    public static string namaDicari;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Sembunyikan panel profil saat memulai game
        profilePanel.SetActive(false);
        kirimPesanText.text = "";
        replyMessageShown = false;
        //QuestManager.SendMessageQuestCompleted == false;
        //textComponent.text = "";
        if (questManager != null)
        {
            Debug.Log("ditemukan questmanager");
        }

        // Tambahkan event listener pada tombol pencarian
        searchButton.onClick.AddListener(SearchButtonClicked);
    }

    public void SearchButtonClicked()
    {
        // Dipanggil saat tombol pencarian diklik
        string inputName = searchInput.text;

        if (!string.IsNullOrEmpty(inputName))
        {
            StartCoroutine(Search(inputName));
            searchInput.text = "";
            chatPanel.SetActive(false);
        }
        else
        {
            // Tampilkan pesan "Nama tidak boleh kosong"
            notFoundText.text = "Nama tidak boleh kosong";
            notFoundText.gameObject.SetActive(true);
            StartCoroutine(HideNotFoundText(2));
            
        }
    }

    private IEnumerator HideNotFoundText(float delay)
    {
        yield return new WaitForSeconds(delay);
        notFoundText.text = "";
        notFoundText.gameObject.SetActive(false);
    }

    private IEnumerator Search(string inputName)
    {
        UserSO.User foundUser = FindUserByName(inputName);

        if (foundUser != null)
        {
            // Jika ditemukan, tampilkan informasi di UI Profil
            DisplayProfile(foundUser);

            // Aktifkan panel profil dan nonaktifkan panel pencarian
            profilePanel.SetActive(true);
            searchPanel.SetActive(false);
            notFoundText.gameObject.SetActive(false);

            // Setelah menampilkan profil, periksa apakah pengguna sudah ditemukan
            /*if (questManager != null)
            {
                questManager.CheckUserAndProceed(namaDicari);
            }*/
        }
        else
        {
            // Jika tidak ditemukan, tampilkan pesan "Nama tidak ditemukan"
            notFoundText.text = "Nama tidak ditemukan";
            searchInput.text = "";
            notFoundText.gameObject.SetActive(true);
            //yield return new WaitForSeconds(2);
            //notFoundText.text = "";
            yield return StartCoroutine(HideNotFoundText(2f));
        }
    }

    private UserSO.User FindUserByName(string name)
    {
        // Cari user berdasarkan nama
        foreach (var user in userSO.user)
        {
            if (string.Equals(user.namaUser, name, StringComparison.OrdinalIgnoreCase))
            {
                return user;
            }
        }
        return null;
    }



    public void DisplayProfile(UserSO.User user)
    {
        // kirimPesanText.text = "";
        //balasPesanText.text = "";
        //textComponent.text = "";
        isFirstChatButtonClick = true;

        // Tampilkan informasi profil di UI Profil
        tentangUserText.text = user.tentangUser;
        namaPengguna.text = user.namaUser;
        namaSearch.text = user.namaUser;

        namaDicari = user.namaUser;

        namaChat.text = user.namaUser;
        //kirimPesanText.text = "Kirim Pesan: " + user.kirimPesan;
        //balasPesanText.text = "Balas Pesan: " + user.balasPesan;
        //StartCoroutine(questManager.CheckUserAndProceed(namaDicari));
        questManager.CheckUserAndProceed(namaDicari);


        // Set nilai balasPesan ke user.balasPesan
        balasPesan = user.balasPesan;

        buttonSend.interactable = true;

        DestroyPreviousObjects();


        buttonChat.onClick.RemoveAllListeners();
        buttonChat.onClick.AddListener(() => OnChatButtonClicked(user.kirimPesan));
    }

    private void OnChatButtonClicked(string pesan)
    {
        if (!chatPanel.activeSelf)
        {
            // Dipanggil saat tombol chat diklik
            if (isFirstChatButtonClick)
            {
                pesanInput.text = pesan;
                isFirstChatButtonClick = false;
            }
            else
            {
                pesanInput.text = "";
            }
            // Dipanggil saat tombol chat diklik
            //pesanInput.text = pesan;
            pesanInput.interactable = false;
            chatPanel.SetActive(true);
            
        }
        else
        {
            chatPanel.SetActive(false);
            //pesanInput.interactable = false;
        }
        // Hapus semua listener yang ada pada buttonSend
        buttonSend.onClick.RemoveAllListeners();

        buttonSend.onClick.AddListener(SendButtonClicked);
    }

    private void SendButtonClicked()
    {
        Debug.Log("button send diklik");


        string pesan = pesanInput.text;

        ShowChatMessage(pesan);

        // Membersihkan input field
        pesanInput.text = "";

        buttonSend.interactable = false;

        StartCoroutine(ShowReplyMessage(balasPesan));
    }

    private IEnumerator ShowReplyMessage(string message)
    {
        Debug.Log("show reply");

        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("wait 2 detik");
        
        bubbleMengetik.transform.SetAsLastSibling();
        bubbleMengetik.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);


        bubbleMengetik.SetActive(false);
        //replyMessageShown = true;

        pindahBubbleBalasParent = Instantiate(bubbleBalasPrefab, bubblePrefab.transform); // Sesuaikan parent transform jika perlu
        Transform childBubbleBalas = pindahBubbleBalasParent.transform.Find("BubbleBalas");
        TMP_Text bubbleBalasText = childBubbleBalas.transform.Find("TextBalasPesan").GetComponent<TMP_Text>();

        if (bubbleBalasText != null)
        {
            Debug.Log("bubble balas ditemukan");

            bubbleBalasText.text = message;

            float fixedWidth = 171f;
            float padding = 10f;

            // Atur lebar teks sesuai panjang pesan
            RectTransform textRectTransform = bubbleBalasText.rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, bubbleBalasText.preferredHeight);

            // Mengatur lebar dan tinggi bubble kirim agar sama dengan teks
            RectTransform bubbleKirimRectTransform = pindahBubbleBalasParent.transform.Find("BubbleBalas").GetComponent<RectTransform>();
            //bubbleKirimRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, bubbleKirimText.preferredHeight + padding * 2);
            bubbleKirimRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, bubbleBalasText.preferredHeight + padding * 2);


            // Atur lebar dan tinggi parent bubble sesuai dengan lebar dan tinggi teks
            RectTransform bubbleRectTransform = pindahBubbleBalasParent.GetComponent<RectTransform>();
            //bubbleRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x + padding * 2, textRectTransform.sizeDelta.y + padding * 2);
            bubbleRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, bubbleBalasText.preferredHeight + padding * 2);


            HorizontalLayoutGroup horizontal = pindahBubbleBalasParent.GetComponent<HorizontalLayoutGroup>();

            if (horizontal != null)
            {
                horizontal.childAlignment = TextAnchor.UpperLeft;
                horizontal.childControlWidth = true;
            }
        }
        pindahBubbleBalasParent.SetActive(true);
        replyMessageShown = true;
        if (replyMessageShown)
        {
            //StartCoroutine(questManager.CheckUserAndProceed(namaDicari));
            questManager.MessageSent();
        }
        
    }

    private void ShowChatMessage(string pesan)
    {
        pindahBubbleKirimParent = Instantiate(bubbleKirimPrefab, bubblePrefab.transform); // Sesuaikan parent transform jika perlu
        Transform childBubbleKirim = pindahBubbleKirimParent.transform.Find("BubbleKirim");
        TMP_Text bubbleKirimText = childBubbleKirim.transform.Find("TextKirimPesan").GetComponent<TMP_Text>();

        if (bubbleKirimText != null)
        {
            Debug.Log("bubble kirim ditemukan");

            bubbleKirimText.text = pesan;

            float fixedWidth = 171f;
            float padding = 10f;

            // Atur lebar teks sesuai panjang pesan
            RectTransform textRectTransform = bubbleKirimText.rectTransform;
            textRectTransform.sizeDelta = new Vector2(fixedWidth, bubbleKirimText.preferredHeight);

            // Mengatur lebar dan tinggi bubble kirim agar sama dengan teks
            RectTransform bubbleKirimRectTransform = pindahBubbleKirimParent.transform.Find("BubbleKirim").GetComponent<RectTransform>();
            //bubbleKirimRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, bubbleKirimText.preferredHeight + padding * 2);
            bubbleKirimRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, bubbleKirimText.preferredHeight + padding * 2);

            // Atur lebar dan tinggi parent bubble sesuai dengan lebar dan tinggi teks
            RectTransform bubbleRectTransform = pindahBubbleKirimParent.GetComponent<RectTransform>();
            //bubbleRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x + padding * 2, textRectTransform.sizeDelta.y + padding * 2);
            bubbleRectTransform.sizeDelta = new Vector2(fixedWidth + padding * 2, bubbleKirimText.preferredHeight + padding * 2);

            HorizontalLayoutGroup horizontal = pindahBubbleKirimParent.GetComponent<HorizontalLayoutGroup>();

            if (horizontal != null)
            {
                horizontal.childAlignment = TextAnchor.UpperRight;
                horizontal.childControlWidth = true;
            }
        }
        pindahBubbleKirimParent.SetActive(true);
        buttonSend.interactable = false;
    }
    private void DestroyPreviousObjects()
    {
        if (bubbleMengetikInstance != null)
        {
            Destroy(bubbleMengetikInstance);
        }

        if (pindahBubbleKirimParent != null)
        {
            Destroy(pindahBubbleKirimParent);
        }

        if (pindahBubbleBalasParent != null)
        {
            Destroy(pindahBubbleBalasParent);
        }
    }

    public bool IsUserFound(string userName)
    {
        // Implementasikan logika pengecekan apakah user dengan userName ditemukan
        // Misalnya, menggunakan list user atau data lainnya untuk melakukan pengecekan.
        // Kembalikan nilai true jika ditemukan, false jika tidak.
        //return false; // Gantilah dengan implementasi sesuai kebutuhan.

        //UserSO.User foundUser = FindUserByName(userName);
        //return foundUser != null;

        //return string.Equals(searchInput.text, userName, StringComparison.OrdinalIgnoreCase);
        return string.Equals(userName, namaDicari, StringComparison.OrdinalIgnoreCase);
    }
}