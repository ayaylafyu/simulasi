using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonActiveQuest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Active Quest")]
    public Button hideButton;
    public Button showButton;
    public GameObject hidePanel;
    public GameObject showPanel;

    [Header("Log Misi")]
    public Button hideButtonMisi;
    public Button showButtonMisi;
    public GameObject hidePanelMisi;
    public GameObject showPanelMisi;

    [Header("Misi")]
    public GameObject[] misi;
    public GameObject[] keterangan;

    [Header("Relasi")]
    public QuestManager questManager;

    private void Start()
    {
        // Menambahkan fungsi untuk merespons klik tombol
        hideButton.onClick.AddListener(BtnHideDiklik);
        showButton.onClick.AddListener(BtnShowDiklik);

        hideButtonMisi.onClick.AddListener(BtnHideMisiDiklik);
        showButtonMisi.onClick.AddListener(BtnShowMisiDiklik);

        // Menambahkan EventTrigger ke setiap elemen dalam array misi
        foreach (GameObject misiObj in misi)
        {
            EventTrigger trigger = misiObj.AddComponent<EventTrigger>();
            // Event Pointer Enter
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((eventData) => { OnPointerEnter((PointerEventData)eventData); });
            trigger.triggers.Add(entryEnter);
            // Event Pointer Exit
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((eventData) => { OnPointerExit((PointerEventData)eventData); });
            trigger.triggers.Add(entryExit);
        }
    }

    // Fungsi yang akan dijalankan ketika tombol hide diklik
    private void BtnHideDiklik()
    {
        if (showPanel.activeSelf)
        {
            showPanel.SetActive(false);
            hidePanel.SetActive(true);
        }
    }

    // Fungsi yang akan dijalankan ketika tombol show diklik
    private void BtnShowDiklik()
    {
        if (hidePanel.activeSelf)
        {
            showPanel.SetActive(true);
            hidePanel.SetActive(false);
        }
    }

    private void BtnHideMisiDiklik()
    {
        if (showPanelMisi.activeSelf)
        {
            showPanelMisi.SetActive(false);
            hidePanelMisi.SetActive(true);
        }
    }

    // Fungsi yang akan dijalankan ketika tombol show diklik
    private void BtnShowMisiDiklik()
    {
        if (hidePanelMisi.activeSelf)
        {
            showPanelMisi.SetActive(true);
            hidePanelMisi.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Menampilkan keterangan sesuai dengan objek yang dihover
        GameObject hoveredObject = eventData.pointerEnter;
        int index = Array.IndexOf(misi, hoveredObject);
        if (index >= 0 && index < keterangan.Length)
        {
            keterangan[index].SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Menyembunyikan keterangan ketika pointer keluar dari objek
        foreach (GameObject keteranganObj in keterangan)
        {
            keteranganObj.SetActive(false);
        }
    }
}
