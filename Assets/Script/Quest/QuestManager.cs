using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance; // Singleton

    [SerializeField] private PhotoQuest quest;

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


    void Start()
    {
        Debug.Log(quest.QuestName);
    }


    void Update()
    {
        
    }

    public void verifyPhoto()
    {

    }
}
