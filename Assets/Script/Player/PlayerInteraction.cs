using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference switchMode;
    [SerializeField] private InputActionReference takePicture;
    [SerializeField] private InputActionReference openGallery;

    // Prise Photo
    [Header("Canvas Photo")]
    [SerializeField] private GameObject canvasCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera photoCamera;
    [SerializeField] private RenderTexture renderTexture;

    [Header("Canvas Gallerie Photo")]
    public PictureGallery gallery;

    [Header("Quêtes")]
    [SerializeField] private QuestManager quest;

    private bool isCameraActive = false ;


    public MonoBehaviour playerCameraScript;

    private void OnEnable()
    {
        mainCamera.enabled = true;
        canvasCamera.gameObject.SetActive(false);
        switchMode.action.Enable();
        switchMode.action.performed += PictureMode;

        takePicture.action.Enable();
        takePicture.action.performed += Picture;

        openGallery.action.Enable();
        openGallery.action.performed += ToggleGallery;

        

    }

    private void OnDisable()
    {
        switchMode.action.performed -= PictureMode;
        switchMode.action.Disable();

        takePicture.action.performed -= Picture;
        takePicture.action.Disable();
    }

    private void PictureMode(InputAction.CallbackContext _ctx)
    {
        Debug.Log("Switch Mode");
        isCameraActive = !isCameraActive;
        canvasCamera.gameObject.SetActive(isCameraActive);
    }

    private void Picture(InputAction.CallbackContext _ctx)
    {
        
        if(isCameraActive)
        {
            SavePhoto();
            CheckElement();
        }
    }

    private void CheckElement()
    {
        int maxDistance = 10;
        Collider[] target = Physics.OverlapSphere(transform.position, 10); // tout les objets à moins de DistaceVision du joueur
        
        foreach (Collider col in target)
        {

            if (col.tag == "Untagged") continue;
            
            float signedAngle = Vector3.Angle( // angle du joueur par rapport au centre de vision
            transform.forward,
            col.transform.position - transform.position);

            // + Raycast pour vérifié que rien ne bloque la vue
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (col.transform.position - transform.position), out hit, maxDistance) && Mathf.Abs(signedAngle) < 90 / 2)
            {
                Debug.Log(col.tag);

                // Calcul du score basé sur la distance
                Vector3 dirToTarget = col.transform.position - transform.position;
                float distance = dirToTarget.magnitude;

                float normalized = 1 - (distance / maxDistance);
                int score = Mathf.RoundToInt(normalized * 100);

                // Vérification si le tag apparait dans une quetes
                quest.verifyPhoto(col.tag,score);

                //si un pnj est detecté
                if (col.tag == "NPC")
                {
                    col.GetComponent<PeopleController>().AnimateByType();
                }
            }
        }
    }

    public void SavePhoto()
    {
        // Rendre la caméra dans la RenderTexture
        photoCamera.targetTexture = renderTexture;
        photoCamera.Render();

        // Activer la RenderTexture
        RenderTexture.active = renderTexture;

        // Copier vers Texture2D
        Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photo.Apply();

        // Nettoyage
        RenderTexture.active = null;

        // Encoder en PNG
        byte[] bytes = photo.EncodeToPNG();

        // Dossier Photos
        string folderPath = Application.persistentDataPath + "/Photos";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fileName = "Photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(folderPath, fileName);

        File.WriteAllBytes(fullPath, bytes);

        Debug.Log("Photo sauvegardée : " + fullPath);
        mainCamera.enabled = true;
    }



    // Galerie
    void ToggleGallery(InputAction.CallbackContext _ctx)
    {
        gallery.ToggleGallery();

        // Bloque le joueur
        playerCameraScript.enabled = !gallery.isOpen;

    }

    // Debug : affiche la zone de détéction du joueur
    private void OnDrawGizmos()
     {
         Handles.color = new Color(0, 1, 0, 0.3f);
         Handles.DrawSolidArc(transform.position,
             transform.up,
             Quaternion.AngleAxis(-90/2f,transform.up)*transform.forward,// orientation de l'angle de vue devant le professeur (autant à gauche et à droite)
             90,
             10);
     }
}
