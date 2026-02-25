using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton
    public int NumberNpc;
    public GameObject[] ListNPC;

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
}
