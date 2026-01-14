using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference SwitchMode;
    [SerializeField] private InputActionReference takePicture;
    [SerializeField] private Canvas canvasCamera;

    private bool IsCameraActive = false ;
    //public InputActionReference lookAction;

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
        }
    }
}
