using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance; // Singleton

    [SerializeField] private PhotoQuest QuestTest; // Liste de quests
    //[SerializeField] private PhotoQuest quest;
    [SerializeField] private PhotoQuest[] QuestList; // Liste de quests

    [Header("Canvas Quest")]
    [SerializeField] private GameObject CanvasQuest;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference SwitchUI;
    


    public GameObject ScrollViewQuest;
    public GameObject TemplateQuest;

    private bool IsActive;

    public int intTest;

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
        CanvasQuest.gameObject.SetActive(false);
        SwitchUI.action.Enable();
        SwitchUI.action.performed += QuestUi;
        addQuest(QuestTest);
    }

    private void QuestUi(InputAction.CallbackContext _ctx)
    {
        Debug.Log("UI");
        IsActive = !IsActive;
        CanvasQuest.gameObject.SetActive(IsActive);
    }
    void Start()
    {
        UpdateUI();
    }


    void Update()
    {
        
    }

    private void UpdateUI()
    {

        Debug.Log(QuestList.Length);
        for (int i = 0; i <= QuestList.Length; i++)
        {
            Debug.Log("loop " + i);
            Debug.Log(QuestList[i].name);

            GameObject NewEntry = Instantiate(TemplateQuest, ScrollViewQuest.transform);
            NewEntry.transform.SetParent(ScrollViewQuest.transform);

            // Modifier le texte
            TMP_Text nomText = NewEntry.transform.Find("Title").GetComponent<TMP_Text>();
            TMP_Text descText = NewEntry.transform.Find("Description").GetComponent<TMP_Text>();
            nomText.text = QuestList[i].QuestName;
            descText.text = QuestList[i].QuestDescription;
        }
    }

    public void addQuest(PhotoQuest NewQuest)
    {
        Debug.Log("adding quest");
    }
    public void verifyPhoto()
    {

    }
}
