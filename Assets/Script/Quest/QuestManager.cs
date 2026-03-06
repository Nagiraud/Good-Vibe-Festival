using System;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance; // Singleton
    //[SerializeField] private PhotoQuest quest;
    [SerializeField] private PhotoQuest[] questList; // Liste de quests

    [Header("Canvas Quest")]
    [SerializeField] private GameObject canvasQuest;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference switchUI;

    [Header("Score")]
    private int score = 0;
    public int scoreToWin = 1000;
    [SerializeField] private TMP_Text textScore;




    [Header("ScollView")]
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
        canvasQuest.gameObject.SetActive(false);
        switchUI.action.Enable();
        switchUI.action.performed += QuestUi;
    }

    private void QuestUi(InputAction.CallbackContext _ctx)
    {
        Debug.Log("UI");
        IsActive = !IsActive;
        canvasQuest.gameObject.SetActive(IsActive);
    }
    void Start()
    {
        UpdateUI();
    }

    private void clearUI()
    {
        foreach (Transform child in ScrollViewQuest.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void UpdateUI()
    {
        // Quetes
        clearUI();
        for (int i = 0; i < questList.Length; i++)
        {
            Debug.Log("loop " + i);
            Debug.Log(questList[i].name);

            GameObject NewEntry = Instantiate(TemplateQuest, ScrollViewQuest.transform);
            NewEntry.transform.SetParent(ScrollViewQuest.transform);

            // Modifier le texte
            TMP_Text nomText = NewEntry.transform.Find("Title").GetComponent<TMP_Text>();
            TMP_Text descText = NewEntry.transform.Find("Description").GetComponent<TMP_Text>();
            nomText.text = questList[i].QuestName;
            descText.text = questList[i].QuestDescription;

            Toggle QuestComplet = NewEntry.transform.Find("Completed").GetComponent<Toggle>();
            QuestComplet.isOn = questList[i].IsCompleted;
        }

        // Score des quêtes
        textScore.text="Point Good Vibe : "+score.ToString();
    }

    public void addQuest(PhotoQuest NewQuest)
    {
        Debug.Log("adding quest");
    }
    public void verifyPhoto(string tag,int scoreToAdd)
    {
        for (int i = 0; i < questList.Length; i++)
        {
            if (questList[i].TagToSearch == tag && questList[i].IsCompleted==false)
            {
                score += scoreToAdd;
                if (score > scoreToWin)
                {
                    SceneManager.LoadScene("EndScene");
                }
                questList[i].IsCompleted = true;
                UpdateUI();
            }
        }
    }
}
