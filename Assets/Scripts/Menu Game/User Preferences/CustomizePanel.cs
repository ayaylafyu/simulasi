using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CustomizePanel : MonoBehaviour
{
    public enum SizeOption
    {
        Small,
        Medium,
        Large
    }

    public SizeOption currentSizeOption = SizeOption.Medium; // Default size option
    public Button smallButton;
    public Button mediumButton;
    public Button largeButton;

    public Sprite[] selected;
    public Sprite[] normal;

    // Daftar elemen yang akan diubah ukurannya
    public RectTransform[] resizablePanels;
    //public TMP_Text[] resizableTexts;

    private void Start()
    {
        // Menambahkan event listener pada tombol-tombol
        smallButton.onClick.AddListener(() => SetSizeOption(SizeOption.Small));
        mediumButton.onClick.AddListener(() => SetSizeOption(SizeOption.Medium));
        largeButton.onClick.AddListener(() => SetSizeOption(SizeOption.Large));

        // Inisialisasi ukuran sesuai dengan opsi yang dipilih
        SetSizeOption(currentSizeOption);
    }

    private void SetSizeOption(SizeOption sizeOption)
    {
        // Mengatur ukuran opsi saat ini
        currentSizeOption = sizeOption;

        // Menyesuaikan ukuran elemen-elemen berdasarkan opsi yang dipilih
        switch (sizeOption)
        {
            case SizeOption.Small:
                SetSizeForElements(1f, 0f); // Ukuran default
                smallButton.image.sprite = selected[0];
                mediumButton.image.sprite = normal[1];
                largeButton.image.sprite = normal[2];
                break;

            case SizeOption.Medium:
                SetSizeForElements(1.25f, 20f); // Menambah ukuran font sebesar 20%
                smallButton.image.sprite = normal[0];
                mediumButton.image.sprite = selected[1];
                largeButton.image.sprite = normal[2];
                break;

            case SizeOption.Large:
                SetSizeForElements(1.5f, 40f); // Menambah ukuran font sebesar 40%
                smallButton.image.sprite = normal[0];
                mediumButton.image.sprite = normal[1];
                largeButton.image.sprite = selected[2];
                break;
        }
    }

    private void SetSizeForElements(float scaleFactor, float fontSizePercentage)
    {
        // Menyesuaikan ukuran panel berdasarkan faktor skala
        foreach (RectTransform panel in resizablePanels)
        {
            panel.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }

    /*public void OnSmallButtonPointerEnter()
    {
        // Saat pointer masuk ke tombol small
        HighlightButton(smallButton, 0);
    }

    public void OnMediumButtonPointerEnter()
    {
        // Saat pointer masuk ke tombol medium
        HighlightButton(mediumButton, 1);
    }

    public void OnLargeButtonPointerEnter()
    {
        // Saat pointer masuk ke tombol large
        HighlightButton(largeButton, 2);
    }

    public void OnButtonPointerExit()
    {
        // Saat pointer keluar dari tombol
        if (currentHighlightedButton != null)
        {
            // Kembalikan gambar tombol ke gambar normal
            currentHighlightedButton.image.sprite = normal[(int)currentSizeOption];
            currentHighlightedButton = null;
        }
    }

    private void HighlightButton(Button button, int index)
    {
        // Ubah gambar tombol menjadi gambar highlight
        button.image.sprite = highlight[index];
        currentHighlightedButton = button;
    }

    private void OnButtonClick(Button button)
    {
        // Tombol yang diklik akan menjadi tombol terpilih
        int index = GetButtonIndex(button);
        button.image.sprite = selected[index];
    }

    private int GetButtonIndex(Button button)
    {
        if (button == smallButton)
        {
            return 0;
        }
        else if (button == mediumButton)
        {
            return 1;
        }
        else if (button == largeButton)
        {
            return 2;
        }
        else
        {
            return -1;
        }
    }*/

}
