using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject helpMenuUI;
    public GameObject listHelp;
    public GameObject setting;

    [Header("Button")]
    public Button[] buttonPause;
    public Sprite selectedPause;
    public Sprite normalPause;
    public TMP_Text[] textPause;

    /*private void Awake()
    {

        pauseMenuUI.SetActive(false);
        //helpMenuUI.SetActive(false);
        listHelp.SetActive(false);
        setting.SetActive(false);
    }*/
    void Start()
    {
        pauseMenuUI.SetActive(false);
        //helpMenuUI.SetActive(false);
        listHelp.SetActive(false);
        setting.SetActive(false);

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //Time.timeScale = 0; // Jeda waktu
            pauseMenuUI.SetActive(true);
            helpMenuUI.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                buttonPause[i].image.sprite = normalPause;
                textPause[i].color = Color.white;
            }
        }
        else if (helpMenuUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Time.timeScale = 0;
                helpMenuUI.SetActive(false);
            }
        }
        
    }
    
    public void ButtonContinue()
    {
        pauseMenuUI.SetActive(false);
        helpMenuUI.SetActive(false);
        listHelp.SetActive(false);
        setting.SetActive(false);

        buttonPause[0].image.sprite = selectedPause;
        textPause[0].color = Color.black;

        for (int i = 1; i < 4; i++)
        {
            buttonPause[i].image.sprite = normalPause;
            textPause[i].color = Color.white;
        }
    }

    public void ButtonSetting()
    {
        if (!setting.activeSelf)
        {
            setting.SetActive(true);
            listHelp.SetActive(false);
        }
        else
        {
            setting.SetActive(false);
            listHelp.SetActive(false);
        }

        buttonPause[0].image.sprite = normalPause;
        textPause[0].color = Color.white;

        buttonPause[1].image.sprite = selectedPause;
        textPause[1].color = Color.black;

        for (int i = 2; i < 4; i++)
        {
            buttonPause[i].image.sprite = normalPause;
            textPause[i].color = Color.white;
        }
    }

    public void ButtonHelp()
    {
        if (!listHelp.activeSelf)
        {
            setting.SetActive(false);
            listHelp.SetActive(true);
        }
        else
        {
            setting.SetActive(false);
            listHelp.SetActive(false);
        }
        for (int i = 0; i < 2; i++)
        {
            buttonPause[i].image.sprite = normalPause;
            textPause[i].color = Color.white;
        }

        buttonPause[2].image.sprite = selectedPause;
        textPause[2].color = Color.black;
       
        buttonPause[3].image.sprite = normalPause;
        textPause[3].color = Color.white;
    }

    public void ButtonExit()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("pindah main menu");

        for (int i = 0; i < 3; i++)
        {
            buttonPause[i].image.sprite = normalPause;
            textPause[i].color = Color.white;
        }

        buttonPause[3].image.sprite = selectedPause;
        textPause[3].color = Color.black;
    }
}
