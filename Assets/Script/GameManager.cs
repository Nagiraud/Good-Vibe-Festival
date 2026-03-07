using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [Header("NPC")]
    public int NumberNpc;
    public GameObject[] ListNPC;

    [Header("Pause")]
    public GameObject pauseMenuUI;
    public InputActionReference InputPause;

    [Header("Animation")]
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Camera playerCamera;    // caméra de jeu
    [SerializeField] private CinemachineCamera[] cinematicCams; // caméras de la cinématique
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
        director.stopped += OnCinematicEnd;
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

    public void PlayCinematic()
    {
        // Désactiver la caméra joueur pendant la cinématique
        playerCamera.enabled=false;

        // Activer les caméras cinématiques
        foreach (var cam in cinematicCams)
        {
            cam.enabled = true;
            cam.Priority = 20;
        }
            

        director.Play();
    }

    private void OnCinematicEnd(PlayableDirector pd)
    {
        // Remettre la caméra joueur
        playerCamera.enabled = true;

        // Désactiver les caméras cinématiques
        foreach (var cam in cinematicCams)
        {
            cam.enabled = false;
            cam.Priority = 0;
        }
    }

    void OnDestroy()
    {
        director.stopped -= OnCinematicEnd;
    }
}
