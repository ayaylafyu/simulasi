using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FileManager : MonoBehaviour
{
    public TMP_Text fileManagerText;
    public GameObject filePrefab; // Assign prefab ini di Inspector
    public GameObject scrollAreaFile;
    public static List<string> downloadedFiles = new List<string>();
    public static FileManager Instance;

    private List<GameObject> clonedFiles = new List<GameObject>();


    //private const string DownloadedFilesKey = "DownloadedFiles";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: If you want this to persist between scenes
            //LoadDownloadedFiles();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FungsiLoad();
    }

    public void FungsiLoad()
    {
        LoadDownloadedFiles();
    }

    public bool IsFileDownloaded(string fileName)
    {
        //return downloadedFiles.Contains(fileName);
        return downloadedFiles.Exists(file => file.Equals(fileName, System.StringComparison.OrdinalIgnoreCase));
    }

    public void AddFileToManager(string fileName)
    {
        if (!IsFileDownloaded(fileName))
        {
            // Menambahkan file ke dalam daftar downloadedFiles
            downloadedFiles.Add(fileName);

            // Menyimpan daftar file yang telah diunduh
            SaveDownloadedFiles();

            // Membuat objek file baru dari prefab
            GameObject newFile = Instantiate(filePrefab, scrollAreaFile.transform);
            clonedFiles.Add(newFile);

            // Mendapatkan komponen teks di dalam objek file
            TMP_Text textFile = newFile.GetComponentInChildren<TMP_Text>();
            if (textFile != null)
            {
                textFile.text = fileName;
            }
            else
            {
                Debug.LogError("TMP_Text not found in the prefab or its children.");
            }

            Debug.Log("File cloned: " + fileName);

            // Mengupdate teks pada fileManagerText dengan daftar file yang sudah diunduh
            UpdateFileManagerText();
        }
        else
        {
            Debug.Log("File already downloaded: " + fileName);
        }
    }

    
    private void UpdateFileManagerText()
    {
        if (downloadedFiles.Count > 0)
        {
            // Mengonversi daftar file yang sudah diunduh menjadi string
            string filesText = string.Join("\n", downloadedFiles.ToArray());

            // Menetapkan teks pada fileManagerText
            fileManagerText.text = filesText;
            Debug.Log("FileManagerText updated:\n" + filesText);
        }
        else
        {
            fileManagerText.text = "";
        }
    }

    private void SaveDownloadedFiles()
    {
        string filesString = string.Join(",", downloadedFiles.ToArray());
        PlayerPrefs.SetString("DownloadedFiles", filesString);
        PlayerPrefs.Save();
        Debug.Log("Downloaded files saved.");
    }

    private void LoadDownloadedFiles()
    {
        if (PlayerPrefs.HasKey("DownloadedFiles"))
        {
            string filesString = PlayerPrefs.GetString("DownloadedFiles");
            downloadedFiles = new List<string>(filesString.Split(','));

            // Update UI after loading
            foreach (string fileName in downloadedFiles)
            {
                GameObject newFile = Instantiate(filePrefab, scrollAreaFile.transform);
                clonedFiles.Add(newFile);
                TMP_Text textFile = newFile.GetComponentInChildren<TMP_Text>();
                if (textFile != null)
                {
                    textFile.text = fileName;
                }
                else
                {
                    Debug.LogError("TMP_Text not found in the prefab or its children.");
                }
            }

            UpdateFileManagerText();
            Debug.Log("Downloaded files loaded.");
        }
        else
        {
            Debug.Log("No downloaded files found.");
            DestroyClonedFiles();
            //DestroyObject(newfile);
        }
    }

    private void DestroyClonedFiles()
    {
        foreach (GameObject file in clonedFiles)
        {
            Destroy(file);
        }
        clonedFiles.Clear();
    }
}
