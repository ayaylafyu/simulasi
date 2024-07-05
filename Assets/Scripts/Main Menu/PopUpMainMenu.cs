using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMainMenu : MonoBehaviour
{
    public Button btnSetting;
    public GameObject popUpSetting;

    private void Start()
    {
        // Menambahkan fungsi untuk merespons klik tombol
        btnSetting.onClick.AddListener(btnSettingDiklik);
    }

    // Fungsi yang akan dijalankan ketika tombol Setting diklik
    private void btnSettingDiklik()
    {
        // jika saat diklik popup setting sudah aktif
        if (popUpSetting.activeSelf)
        {
            // maka nonaktifkan popup setting
            popUpSetting.SetActive(false);
        }
        // sebaliknya jika saat diklik popup setting nonaktif
        else
        {
            // maka aktifkan popup setting dan nonaktifkan popup power
            popUpSetting.SetActive(true);
            
        }
    }
}
