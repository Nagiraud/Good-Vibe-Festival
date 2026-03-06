using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PictureGallery : MonoBehaviour
{

    // Galerie
    [Header("Panels")]
    public GameObject galleryPanel;
    public GameObject fullscreenPanel;

    [Header("UI References")]
    public Transform contentParent;
    public GameObject thumbnailPrefab;
    public RawImage fullscreenImage;

    public bool isOpen = false;
    private List<Texture2D> loadedTextures = new List<Texture2D>();

    // Galerie
    public void ToggleGallery()
    {
        isOpen = !isOpen;
        galleryPanel.SetActive(isOpen);


        if (isOpen)
        {
            LoadGallery();
        }
        else
        {
            ClearGallery();
            CloseFullscreen();
        }
    }

    void LoadGallery()
    {
        string folderPath = Application.persistentDataPath + "/Photos";

        if (!Directory.Exists(folderPath))
            return;

        string[] files = Directory.GetFiles(folderPath, "*.png");

        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);

            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            loadedTextures.Add(tex);

            GameObject thumb = Instantiate(thumbnailPrefab, contentParent);
            RawImage img = thumb.GetComponent<RawImage>();
            img.texture = tex;

            Button btn = thumb.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                ShowFullscreen(tex);
            });
        }
    }

    void ClearGallery()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Texture2D tex in loadedTextures)
        {
            Destroy(tex);
        }

        loadedTextures.Clear();
    }

    void ShowFullscreen(Texture2D tex)
    {
        fullscreenPanel.SetActive(true);
        fullscreenImage.texture = tex;
    }

    public void CloseFullscreen()
    {
        fullscreenPanel.SetActive(false);
    }
}
