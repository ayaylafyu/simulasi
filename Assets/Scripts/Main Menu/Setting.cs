using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class Setting : MonoBehaviour
{
    [Header("Music")]
    public GameObject musicOnButton;
    public GameObject musicOffButton;
    public GameObject objMusic;
    private AudioSource backgroundMusic;

    private bool isMusicOn = true;

    [Header("Sound")]
    public GameObject soundOnButton;
    public GameObject soundOffButton;
    public GameObject objSound;
    public GameObject objGameOver;
    public GameObject objSuccess;
    private AudioSource backgroundSound;
    public static AudioSource backgroundGameOver;
    public static AudioSource backgroundSuccess;

    public static bool isSoundOn = true;

    [Header("Volume")]
    public Slider volumeSlider;
    public float volume;

    [Header("Button")]
    public Button[] buttonClick;


    public const string keyVolume = "VOLUME"; 
    public const string keyIsMusicOn = "ISMUSICON";
    public const string keyIsSoundOn = "ISSOUNDON";



    private void Start()
    {
        objMusic = GameObject.FindWithTag("Music");
        backgroundMusic = objMusic.GetComponent<AudioSource>();
        objSound = GameObject.FindWithTag("Sound");
        backgroundSound = objSound.GetComponent<AudioSource>();
        objGameOver = GameObject.FindWithTag("GameOver");
        backgroundGameOver = objGameOver.GetComponent<AudioSource>();
        objSuccess = GameObject.FindWithTag("Success");
        backgroundSuccess = objSuccess.GetComponent<AudioSource>();

        float totalDuration = backgroundSuccess.clip.length;
        Debug.Log("Total Duration: " + totalDuration + " seconds");

        // Check if the keys exist in PlayerPrefs, if not, initialize them
        if (!PlayerPrefs.HasKey(keyVolume))
        {
            PlayerPrefs.SetFloat(keyVolume, 1.0f);
        }
        if (!PlayerPrefs.HasKey(keyIsMusicOn))
        {
            PlayerPrefs.SetInt(keyIsMusicOn, 1);
        }
        if (!PlayerPrefs.HasKey(keyIsSoundOn))
        {
            PlayerPrefs.SetInt(keyIsSoundOn, 1);
        }

        float volumeTerakhir = PlayerPrefs.GetFloat(keyVolume);
        sliderGeser(volumeTerakhir);
        volumeSlider.value = volumeTerakhir;

        isMusicOn = PlayerPrefs.GetInt(keyIsMusicOn) == 1;
        if (isMusicOn == true)
        {
            backgroundMusic.mute = false;
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
            Debug.Log("musik pertama on");
        }
        else
        {
            backgroundMusic.mute = true;
            musicOffButton.SetActive(true);
            musicOnButton.SetActive(false);
            Debug.Log("music pertama off");
        }

        isSoundOn = PlayerPrefs.GetInt(keyIsSoundOn) == 1;
        if (isSoundOn == true)
        {
            backgroundSound.mute = false;
            backgroundGameOver.mute = false;
            backgroundSuccess.mute = false;
            soundOffButton.SetActive(false);
            soundOnButton.SetActive(true);
            Debug.Log("sound pertama on");
        }
        else
        {
            backgroundSound.mute = true;
            backgroundGameOver.mute = true;
            backgroundSuccess.mute = true;
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
            Debug.Log("sound pertama off");
        }

        for (int i = 0; i < buttonClick.Length; i++)
        {
            buttonClick[i].onClick.AddListener(ButtonDiklik);
        }
    }

    private void ButtonDiklik()
    {
        if (isSoundOn)
        {
            backgroundSound.time = 0.13f;
            backgroundSound.Play();
        }
        else
        {

        }
    }


    public void musicButton()
    {
        isMusicOn = !isMusicOn;
        
        if (isMusicOn)
        {
            backgroundMusic.mute = false;
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
            Debug.Log("music on lagi");   
        }
        else
        {
            backgroundMusic.mute = true;
            musicOffButton.SetActive(true);
            musicOnButton.SetActive(false);
            Debug.Log("music off");
        }
        PlayerPrefs.SetInt(keyIsMusicOn, (isMusicOn ? 1 : 0));
    }

    public void soundButton()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            backgroundSound.mute = false;
            backgroundGameOver.mute = false;
            backgroundSuccess.mute = false;
            soundOffButton.SetActive(false);
            soundOnButton.SetActive(true);
            Debug.Log("sound on lagi");
        }
        else
        {
            backgroundSound.mute = true;
            backgroundGameOver.mute = true;
            backgroundSuccess.mute = true;
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
            Debug.Log("sound off");
        }
        PlayerPrefs.SetInt(keyIsSoundOn, (isSoundOn ? 1 : 0));
    }

    public void sliderGeser(float nilaiSlider)
    {
        volume = nilaiSlider;
        backgroundMusic.volume = nilaiSlider;
        backgroundSound.volume = nilaiSlider;
        backgroundGameOver.volume = nilaiSlider;
        backgroundSuccess.volume = nilaiSlider;

        PlayerPrefs.SetFloat(keyVolume, nilaiSlider);
    }


}
