using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Camera Settings")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float maxLookAngle = 80f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;

    private CharacterController controller;
    private Vector3 playerVelocity;

    // Rotation
    private float xRotation = 0f;
    private Vector2 lookInput;

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();

    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        mainCamera = GetComponentInChildren<Camera>()?.transform;
    }

    void Update()
    {

        // Gestion de la rotation de la caméra
        HandleLook();

        // Lecture du mouvement
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move = Vector3.ClampMagnitude(move, 1f);

        // Applique la gravité
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Déplacement
        Vector3 finalMove = move * playerSpeed + Vector3.up * playerVelocity.y;
        controller.Move(finalMove * Time.deltaTime);
    }

    private void HandleLook()
    {
        // Récupère l'input de la souris/stick
        lookInput = lookAction.action.ReadValue<Vector2>();

        // Rotation horizontale (personnage - axe Y)
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Rotation verticale (caméra - axe X)
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        // Applique la rotation verticale à la caméra seulement
        if (mainCamera != null)
        {
            mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}