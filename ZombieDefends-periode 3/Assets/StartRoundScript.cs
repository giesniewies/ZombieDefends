using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoundScript : MonoBehaviour
{
    public GameObject zombiePrefab; // Assign in Inspector
    public Transform[] spawnPoints; // Assign spawn points in Inspector
    public GameObject bed; // Assign bed object in Inspector
    public int baseZombieCount = 5; // Zombies in the first round

    private int currentRound = 0;
    private int remainingZombies = 0;

    void Start()
    {
        bed.SetActive(true);
    }

    void Update()
    {
        if (remainingZombies == 0 && currentRound > 0)
        {
            bed.SetActive(true); // Allow player to start next round
        }
        if (Input.GetKey(KeyCode.P))
        {
            StartNewRound();
        }
    }

    public void StartNewRound()
    {
        currentRound++;
        int zombieCount = baseZombieCount + (currentRound * 2); // Increase zombies per round
        remainingZombies = zombieCount;
        bed.SetActive(false);
        SpawnZombies(zombieCount);
    }

    void SpawnZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
            zombie.GetComponent<ZombieAIScript>().SetManager(this); // Ensure zombie knows the manager
        }
    }

    public void ZombieKilled()
    {
        remainingZombies--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == ("Player") && remainingZombies == 0)
        {
            StartNewRound();
        }
    }
}