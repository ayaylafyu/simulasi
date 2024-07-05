using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    public TMP_InputField inputNama;
    public TMP_InputField inputPass;
    public Button startButton;
    public TMP_Text errorText;


    public GameObject[] tutorial1;

    public Button btnTutorial1;

    //private bool boolTutorial1 = false;

    //private const string USER_INPUT_NAME = "UserName";

    private void Start()
    {
        inputNama.text = "";
        inputPass.text = "";
        errorText.text = "";

        if (!PlayerPrefs.HasKey("MainMenuOpened"))
        {
            for (int i = 0; i < tutorial1.Length; i++)
            {
                tutorial1[i].SetActive(true);
            }
            //PlayerPrefs.SetInt("MainMenuOpened", 1);
            //PlayerPrefs.Save();

        }
        else
        {
            // Jika "GameOpened" sudah ada dan bernilai 1, tampilkan popup
            if (PlayerPrefs.GetInt("MainMenuOpened") == 1)
            {
                for (int i = 0; i < tutorial1.Length; i++)
                {
                    tutorial1[i].SetActive(false);
                }
            }
        }

            

        startButton.onClick.AddListener(ButtonStartDiklik);
        btnTutorial1.onClick.AddListener(ButtonTutorialClicked);

    }

    private void ButtonTutorialClicked()
    {
        for (int i = 0; i < tutorial1.Length; i++)
        {
            tutorial1[i].SetActive(false);
        }
    }

    private void ButtonStartDiklik()
    {
        string nama = inputNama.text;
        string pass = inputPass.text;

        // kondisi benar semua
        if (!string.IsNullOrEmpty(nama) && pass == "root")
        {
            SceneManager.LoadScene("MenuGame");
            
            
            Debug.Log("pindah menu game");
            if (!PlayerPrefs.HasKey("UserName"))
            {
                PlayerPrefs.SetString("UserName", nama);
                PlayerPrefs.Save();
            }
            
        }
        // kondisi nama dan password kosong
        else if(string.IsNullOrEmpty(nama) && pass == "")
        {
            errorText.text = "Nama dan Password Harus Diisi!";
            StartCoroutine(WrongInput(3));
        }
        // kondisi nama dan password kosong
        else if(!string.IsNullOrEmpty(nama) && pass == "")
        {
            errorText.text = "Password Harus Diisi!";
            StartCoroutine(WrongInput(3));
        }
        // kondisi password salah
        else if(!string.IsNullOrEmpty(nama) && pass != "root")
        {
            errorText.text = "Password Salah!";
            StartCoroutine(WrongInput(3));
        }
        // kondisi nama kosong dan password salah
        else if(string.IsNullOrEmpty(nama) && pass != "root")
        {
            errorText.text = "Nama dan Password Salah!";
            StartCoroutine(WrongInput(3));
        }
        // kondisi nama kosong dan password benar
        else if(string.IsNullOrEmpty(nama) && pass == "root")
        {
            errorText.text = "Nama Harus Diisi!";
            StartCoroutine(WrongInput(3));
        }

    }

    private IEnumerator WrongInput(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        errorText.text = "";
    }
}
