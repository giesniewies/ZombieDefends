using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAIScript : MonoBehaviour
{
    public Transform hut; // De hut die de zombie moet aanvallen
    public Transform player; // De speler
    public float detectieAfstand = 10f; // Afstand waarop de zombie de speler detecteert
    public float aanvalAfstand = 2f; // Afstand waarop de zombie aanvalt
    public float aanvalInterval = 1.5f; // Tijdsinterval tussen aanvallen
    public int schade = 10; // Hoeveel schade een aanval doet

    private NavMeshAgent agent;
    private bool valtHutAan = true;
    private float volgendeAanvalTijd = 0f;

    private StartRoundScript startRoundScript;

    public void SetManager(StartRoundScript roundManager)
    {
        startRoundScript = roundManager;
    }

    public void Die()
    {
        startRoundScript.ZombieKilled();
        Destroy(gameObject);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(hut.position);
    }

    void Update()
    {
        float afstandTotSpeler = Vector3.Distance(transform.position, player.position);
        float afstandTotHut = Vector3.Distance(transform.position, hut.position);

        if (afstandTotSpeler < detectieAfstand)
        {
            // Achtervolg speler als hij dichtbij is
            agent.SetDestination(player.position);
            valtHutAan = false;
        }
        else if (!valtHutAan)
        {
            // Als speler ontsnapt, terug naar de hut
            agent.SetDestination(hut.position);
            valtHutAan = true;
        }

        if (afstandTotSpeler < aanvalAfstand)
        {
            Aanvallen(player.gameObject);
        }
        else if (valtHutAan && afstandTotHut < aanvalAfstand)
        {
            Aanvallen(hut.gameObject);
        }
    }

    void Aanvallen(GameObject doelwit)
    {
        if (Time.time >= volgendeAanvalTijd)
        {
            if (doelwit.CompareTag("Player"))
            {
                // Roep hier de speler damage functie aan
                Debug.Log("Zombie valt speler aan!");
            }
            else if (doelwit.CompareTag("Hut"))
            {
                // Roep hier de hut damage functie aan
                Debug.Log("Zombie valt de hut aan!");
            }
            volgendeAanvalTijd = Time.time + aanvalInterval;
        }
    }
}

