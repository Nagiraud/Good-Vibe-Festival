using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference SwitchMode;
    [SerializeField] private InputActionReference takePicture;

    [Header("Canvas Photo")]
    [SerializeField] private GameObject canvasCamera;

    [SerializeField] private QuestManager quest;

    private bool IsCameraActive = false ;

    private void OnEnable()
    {
        canvasCamera.gameObject.SetActive(false);
        SwitchMode.action.Enable();
        SwitchMode.action.performed += pictureMode;

        takePicture.action.Enable();
        takePicture.action.performed += picture;
    }

    private void OnDisable()
    {
        SwitchMode.action.performed -= pictureMode;
        SwitchMode.action.Disable();

        takePicture.action.performed -= picture;
        takePicture.action.Disable();
    }

    private void pictureMode(InputAction.CallbackContext _ctx)
    {
        Debug.Log("Switch Mode");
        IsCameraActive = !IsCameraActive;
        canvasCamera.gameObject.SetActive(IsCameraActive);
    }

    private void picture(InputAction.CallbackContext _ctx)
    {
        
        if(IsCameraActive)
        {
            Debug.Log("take picture");
            CheckElement();
        }
    }

    private void CheckElement()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, 10); // tout les objets à moins de DistaceVision du joueur
        
        foreach (Collider col in target)
        {

            if (col.tag == "Untagged") continue;
            
            float signedAngle = Vector3.Angle( // angle du joueur par rapport au centre de vision
            transform.forward,
            col.transform.position - transform.position);

            // + Raycast pour vérifié que rien ne bloque la vue
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (col.transform.position - transform.position), out hit, 10) && Mathf.Abs(signedAngle) < 90 / 2)
            {
                Debug.Log(col.tag);
                //TODO 
                // Vérifié les quétes
            }
            
        }
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
