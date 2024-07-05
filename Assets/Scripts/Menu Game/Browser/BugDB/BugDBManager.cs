using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BugDBManager : MonoBehaviour
{
    //public Button downloadButton;
    //public TMP_Text serviceText;
    //public TMP_Text versionText;
    public BugDBSO bugDB;
    public GameObject kolomPrefab;
    public GameObject viewPrefab;
    public Sprite darkBackground;
    public Sprite lightBackground;
    public TMP_Text statusText;
    public FileManager fileManager;
    public static BugDBManager Instance;
    //public GameObject downloadPrefab;

    //private int currentIndex = 0;
    //private FileManager fileManagerInstance;

    //private const string ButtonStatePrefix = "ButtonState_";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FungsiLoadBugDB();
    }

    public void FungsiLoadBugDB()
    {
        Debug.Log("run fungsi load bug db");

        foreach (Transform child in viewPrefab.transform)
        {
            Destroy(child.gameObject);
        }


        for (int i = 0; i < bugDB.bugs.Length; i++)
        {
            GameObject pindah = Instantiate(kolomPrefab, viewPrefab.transform);

            TMP_Text serviceNameText = pindah.transform.Find("Service").GetComponent<TMP_Text>();
            TMP_Text versionNumberText = pindah.transform.Find("Version").GetComponent<TMP_Text>();
            Button download = pindah.transform.Find("Download").GetComponent<Button>();
            Image background = pindah.GetComponent<Image>();

            // Variabel lokal untuk menangkap nilai i
            int index = i;

            // Mengatur teks untuk service name dan version number
            serviceNameText.text = bugDB.bugs[index].serviceName;
            versionNumberText.text = bugDB.bugs[index].versionNumber;

            // Menambahkan listener untuk button
            download.onClick.AddListener(() => OnDownloadButtonClick(download, bugDB.bugs[index]));
            download.interactable = GetButtonState(bugDB.bugs[index].downloadButtonText);
            Debug.Log("download interactable index " + index + download.interactable);

            // Mengatur gambar latar belakang berdasarkan indeks
            background.sprite = (index % 2 == 0) ? darkBackground : lightBackground;
        }
        //Destroy(kolomPrefab);
    }

    public void OnDownloadButtonClick(Button button, BugDBSO.BugData bugData)
    {
        // Logika ketika tombol download diklik
        Debug.Log("Downloading " + bugData.downloadButtonText);

        // Menampilkan teks "Downloading..."
        //statusText.text = "Downloading";

        StartCoroutine(SimulateDownload(button, bugData));


        //button.interactable = false;

        // menonaktifkan button
        //downloadButton.gameObject.SetActive(false);

        // Simulasi penyimpanan ke file manager
        //fileManager.AddFileToManager(bugData.downloadButtonText);
    }

    IEnumerator SimulateDownload(Button button, BugDBSO.BugData bugData)
    {
        const float dotInterval = 0.4f; // Interval antara setiap penambahan titik
        const int maxDots = 3; // Jumlah maksimal titik

        int currentDots = 0;

        while (currentDots <= maxDots)
        {
            // Tambahkan titik ke teks
            statusText.text = "Downloading" + new string('.', currentDots);

            yield return new WaitForSecondsRealtime(dotInterval);

            // Kurangi satu titik
            currentDots++;
        }


        // Mengaktifkan kembali button
        //button.interactable = false;

        // Kurangi satu titik lagi setelah mencapai jumlah maksimal
        currentDots--;


        while (currentDots >= 0)
        {
            // Tambahkan titik ke teks
            statusText.text = "Downloading" + new string('.', currentDots);

            yield return new WaitForSecondsRealtime(dotInterval);

            // Kurangi satu titik
            currentDots--;
        }

        yield return new WaitForSecondsRealtime(0.5f);


        // Menunggu selama 2 detik
        //yield return new WaitForSeconds(2);

        // Menampilkan teks "Downloaded" setelah simulasi unduhan selesai
        statusText.text = "Downloaded!";

        // Simulasi penyimpanan ke file manager
        fileManager.AddFileToManager(bugData.downloadButtonText);

        button.interactable = false;
        SetButtonState(bugData.downloadButtonText, false);

        // Menghilangkan teks setelah beberapa saat
        yield return new WaitForSecondsRealtime(1.5f);
        statusText.text = "";
    }

    private void SetButtonState(string buttonText, bool state)
    {
        PlayerPrefs.SetInt("ButtonState" + buttonText, state ? 1 : 0);
        Debug.Log("ButtonState" + buttonText);
        // Debug.Log(ButtonStatePrefix + buttonText = 0);
        
        PlayerPrefs.Save();
        Debug.Log("bug db : " + PlayerPrefs.GetInt("ButtonState"));
    }

    private bool GetButtonState(string buttonText)
    {
        return PlayerPrefs.GetInt("ButtonState" + buttonText, 1) == 1;
    }

    public void ResetAllButtonStates()
    {
        for (int i = 0; i < bugDB.bugs.Length; i++)
        {
            string buttonText = bugDB.bugs[i].downloadButtonText;
            PlayerPrefs.SetInt("ButtonState" + buttonText, 1); // Set all buttons to true (interactable)
        }
        PlayerPrefs.Save();

        FungsiLoadBugDB();
    }
}
