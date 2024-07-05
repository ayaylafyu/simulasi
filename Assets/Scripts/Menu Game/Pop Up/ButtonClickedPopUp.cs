using UnityEngine;
using UnityEngine.UI;

public class ButtonClickedPopUp : MonoBehaviour
{
    public Button[] buttons;
    public GameObject[] popUps;

    private PindahPopUp pindahPopUp;

    private void Start()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // Simpan nilai i ke variabel lokal untuk menggunakan dalam delegate

            // Tambahkan listener ke event onClick tombol
            buttons[i].onClick.AddListener(() => ShowPopUpAndSetLastSibling(popUps[buttonIndex]));
        }
    }

    private void ShowPopUpAndSetLastSibling(GameObject popUp)
    {
        

        if (popUp != null)
        {
            // Tampilkan popUp
            popUp.SetActive(true);

            // Jadikan popUp sebagai sibling terakhir
            popUp.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogError("popUp is null!");
        }
    }
}
