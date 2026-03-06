using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PeopleController : MonoBehaviour
{
    [Header("Déplacement")]
    public GameObject[] listePosition;
    public float speed;
    NavMeshAgent navAgent;

    [Header("Comportement actuel")]
    public string currentType;
    public string[] typeNPC =
    {
        "idle",
        "dance",
        "flee"
    };
    Animator animator;

    List<List<string>> listPath = new List<List<string>>
    {
        new List<string> { "Stadium", "GasStation", "FastFood", "MusicStore", "Bar", "BookStore" },
        new List<string> { "Concert1Center", "Concert1TopLeft", "Concert1BottomLeft", "Concert1BottomRight", "FriedChicken", "Pizza", "CoffeeShop" },
        new List<string> { "Building", "ClothStore", "GiftShop", "BookStore", "Bakery", "MusicStore", "FruitCorner" },
        new List<string> { "Bar", "FastFood", "CoffeeShop", "BookStore", "StrangeCorner", "GasStation" },
        new List<string> { "Bakery", "Pizza", "FriedChicken", "FruitCorner", "FastFood", "CoffeeShop", "Bar" },
        new List<string> { "BookStore", "MusicStore", "GiftShop", "Building", "Concert2", "CoffeeShop" },
        new List<string> { "Stadium", "IntoStadium", "GasStation", "FastFood", "Bar", "StrangeCorner", "GiftShop" },
        new List<string> { "Concert1Center", "Concert1TopLeft", "Concert1BottomRight", "FriedChicken", "Pizza", "FastFood", "Bar" },
        new List<string> { "Building", "ClothStore", "MusicStore", "BookStore", "CoffeeShop", "Bakery", "FruitCorner" },
        new List<string> { "GiftShop", "ClothStore", "BookStore", "MusicStore", "FastFood", "CoffeeShop", "GasStation" }
    };

    private int pathChoosen;
    private int actualDestination = 0;

    // Stocker la référence de la coroutine pour pouvoir la stopper
    private Coroutine followPathCoroutine;
    // Flag pour mettre en pause le trajet depuis AnimateByType
    private bool isPaused = false;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        pathChoosen = Random.Range(0, listPath.Count);
        currentType = typeNPC[Random.Range(0, 3)];

        followPathCoroutine = StartCoroutine(FollowPath());
    }

    void Update()
    {
        animator.SetBool("IsMoving", navAgent.velocity.magnitude > 0.1f);
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            //  Attendre si en pause (animation en cours)
            while (isPaused)
                yield return null;

            string destinationName = listPath[pathChoosen][actualDestination];
            Transform target = GetPositionByName(destinationName);

            if (target != null)
            {
                navAgent.speed = speed;
                navAgent.SetDestination(target.position);

                while (navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance)
                {
                    // Si pause déclenchée en cours de route, stopper immédiatement
                    if (isPaused)
                    {
                        navAgent.ResetPath();
                        break;
                    }
                    yield return null;
                }

                if (!isPaused)
                {
                    navAgent.speed = 0;
                    yield return new WaitForSeconds(5f);
                }
            }

            if (!isPaused)
            {
                actualDestination++;
                if (actualDestination >= listPath[pathChoosen].Count)
                    actualDestination = 0;
            }
        }
    }

    // Appelé depuis l'extérieur pour déclencher une animation 10 secondes
    public void AnimateByType()
    {
        StartCoroutine(PlayAnimationThenResume());
    }

    IEnumerator PlayAnimationThenResume()
    {
        // 1. Mettre en pause le trajet
        isPaused = true;
        navAgent.ResetPath();
        navAgent.speed = 0;

        // 2. Jouer l'animation selon currentType
        animator.SetBool("IsMoving", false);
        animator.SetTrigger(currentType); // "idle", "dance" ou "flee"

        Debug.Log("Attente");
        // 3. Attendre 10 secondes
        yield return new WaitForSeconds(10f);

        // 4. Reprendre le trajet
        animator.ResetTrigger(currentType);
        isPaused = false;
    }

    Transform GetPositionByName(string name)
    {
        foreach (GameObject go in listePosition)
        {
            if (go.name == name)
            {
                Debug.Log(go.name + "/" + go.transform.position);
                return go.transform;
            }
        }

        Debug.LogWarning("Position non trouvée : " + name);
        return null;
    }
}
