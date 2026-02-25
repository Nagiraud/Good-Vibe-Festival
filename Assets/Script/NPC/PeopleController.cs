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

    Animator animator;

    //private string[] ListeRoutine = ["Stadium", "Building","IntoStadium","Concert1Center","Concert1TopLeft", "Concert1BottomLeft", "Concert1BottomRight", "Concert1TopLeft", "GasStation", "ClothStore", "FriedChicken", "GiftShop", "MusicStore", "FruitCorner", "FastFood", "CoffeeShop", "Bar", "BookStore", "Bakery", "Pizza", "StrangeCorner", "Concert2"];
    // liste de tous les parcours possible
    List<List<string>> listPath = new List<List<string>>
    {
        // Parcours 1 : Visite du stade et des commerces environnants
        new List<string> { "Stadium", "GasStation", "FastFood", "MusicStore", "Bar", "BookStore" },
    
        // Parcours 2 : Quartier du concert et restauration
        new List<string> { "Concert1Center", "Concert1TopLeft", "Concert1BottomLeft", "Concert1BottomRight", "FriedChicken", "Pizza", "CoffeeShop" },
    
        // Parcours 3 : Shopping en ville
        new List<string> { "Building", "ClothStore", "GiftShop", "BookStore", "Bakery", "MusicStore", "FruitCorner" },
    
        // Parcours 4 : Soirée détente
        new List<string> { "Bar", "FastFood", "CoffeeShop", "BookStore", "StrangeCorner", "GasStation" },
    
        // Parcours 5 : Parcours gourmand
        new List<string> { "Bakery", "Pizza", "FriedChicken", "FruitCorner", "FastFood", "CoffeeShop", "Bar" },
    
        // Parcours 6 : Quartier culturel
        new List<string> { "BookStore", "MusicStore", "GiftShop", "Building", "Concert2", "CoffeeShop" },
    
        // Parcours 7 : Périple autour du stade
        new List<string> { "Stadium", "IntoStadium", "GasStation", "FastFood", "Bar", "StrangeCorner", "GiftShop" },
    
        // Parcours 8 : Avant/après concert
        new List<string> { "Concert1Center", "Concert1TopLeft", "Concert1BottomRight", "FriedChicken", "Pizza", "FastFood", "Bar" },
    
        // Parcours 9 : Balade urbaine
        new List<string> { "Building", "ClothStore", "MusicStore", "BookStore", "CoffeeShop", "Bakery", "FruitCorner" },
    
        // Parcours 10 : Circuit complet commerces
        new List<string> { "GiftShop", "ClothStore", "BookStore", "MusicStore", "FastFood", "CoffeeShop", "GasStation" }
    };
    private int pathChoosen;
    private int actualDestination = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        pathChoosen = Random.Range(0, listPath.Count);

        StartCoroutine(FollowPath());
    }

    void Update()
    {
        animator.SetBool("IsMoving", navAgent.velocity.magnitude > 0.1f);
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            // Trouver la destination actuelle
            string destinationName = listPath[pathChoosen][actualDestination];
            Transform target = GetPositionByName(destinationName);

            if (target != null)
            {
                navAgent.speed = speed;
                navAgent.SetDestination(target.position);

                // Attendre d'arriver
                while (navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance)
                {
                    yield return null;
                }

                // Attendre 5 secondes sur place
                navAgent.speed = 0;
                yield return new WaitForSeconds(5f);
            }

            // Destination suivante
            actualDestination++;
            if (actualDestination >= listPath[pathChoosen].Count)
            {
                actualDestination = 0; // recommence le parcours
            }
        }
    }

    Transform GetPositionByName(string name)
    {
        foreach (GameObject go in listePosition)
        {
            if (go.name == name) {
                Debug.Log(go.name+"/"+go.transform.position);
                return go.transform;
            }
        }

        Debug.LogWarning("Position non trouvée : " + name);
        return null;
    }
}
