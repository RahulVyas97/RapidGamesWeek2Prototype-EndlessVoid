using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn; // Assign the GameObject you want to spawn in the Inspector
    public Transform playerTransform; // Assign the player's Transform in the Inspector
    public float spawnInterval = 5f; // Time between each spawn

    private bool isPlayerInside = false;
    private Coroutine spawnCoroutine;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the colliding object is tagged as "Player"
        {
            if (!isPlayerInside)
            {
                isPlayerInside = true;
                // Start spawning objects
                spawnCoroutine = StartCoroutine(SpawnObjectAtIntervals());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerInside)
        {
            isPlayerInside = false;
            // Stop spawning objects
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
        }
    }

    IEnumerator SpawnObjectAtIntervals()
    {
        while (isPlayerInside)
        {
            SpawnFacingPlayer();
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    void SpawnFacingPlayer()
    {
        // Calculate a random position inside the trigger box
        Vector3 randomPosition = new Vector3(
            Random.Range(GetComponent<Collider>().bounds.min.x, GetComponent<Collider>().bounds.max.x),
            Random.Range(GetComponent<Collider>().bounds.min.y, GetComponent<Collider>().bounds.max.y),
            Random.Range(GetComponent<Collider>().bounds.min.z, GetComponent<Collider>().bounds.max.z));

        // Instantiate the object at the random position
        GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);

        // Calculate the direction from the object to the player
        Vector3 directionToPlayer = playerTransform.position - spawnedObject.transform.position;

        // Set the object's rotation to face the player
        spawnedObject.transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
}
