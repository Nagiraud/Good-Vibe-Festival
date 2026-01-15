using UnityEngine;

[CreateAssetMenu(fileName = "PhotoQuest", menuName = "Scriptable Objects/PhotoQuest")]
public class PhotoQuest : ScriptableObject
{
    public string QuestName;
    public string QuestDescription;
    public string[] TagToSearch;
    public int TimeNeeded;
    public int Recompense;
}
