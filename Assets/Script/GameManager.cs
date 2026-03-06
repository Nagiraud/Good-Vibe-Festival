using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("NPC")]
    public int NumberNpc;
    public GameObject[] ListNPC;

    [Header("Pause")]
    public GameObject pauseMenuUI;
    public InputActionReference InputPause;
    public bool IsPaused { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        InputPause.action.Enable();
        InputPause.action.performed += MenuPause;
    }

    private void OnDisable()
    {
        InputPause.action.Disable();
        InputPause.action.performed -= MenuPause;
    }
    void Start()
    {
        RandomPlacementNPC();
    }

    private void RandomPlacementNPC()
    {
        for (int i = 0; i < NumberNpc; i++)
        {
            int RanNpc = Random.Range(0, ListNPC.Length);
            int RanPosX = Random.Range(13,65);
            int RanPosZ = Random.Range(72,152);
            Vector3 PosNPC = new Vector3(RanPosX, 0, RanPosZ);
            Instantiate(ListNPC[RanNpc],PosNPC,Quaternion.identity);
        }
    }

    public void MenuPause(InputAction.CallbackContext _ctx)
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;
            pauseMenuUI.SetActive(true);
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            AudioListener.pause = false;
        }
    }
}
