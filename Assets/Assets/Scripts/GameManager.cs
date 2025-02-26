using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn; // Regularly spawning object
    public GameObject specialObjectToSpawn; // Special object that spawns once and faces the player
    public GameObject arrow; // Arrow GameObject that will point towards the special object
    public Transform playerTransform; // Player's Transform
    public AudioSource specialSpawnAudioSource, startAudio; // AudioSource to play when the special object spawns
    public float spawnInterval = 3f; // Interval for regular object spawn
    private bool specialObjectSpawned = false;

    private void Start()
    {
        if (startAudio != null)
        {
            startAudio.Play();
        }

        if (arrow != null)
        {
            arrow.SetActive(false);
        }
        StartCoroutine(SpawnObjectAtIntervals());
        StartCoroutine(SpawnSpecialObjectOnce());
    }

    IEnumerator SpawnObjectAtIntervals()
    {
        while (true) // Infinite loop to keep spawning objects
        {
            SpawnObjectAroundPlayer(objectToSpawn, 500f, 1500f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    GameObject SpawnObjectAroundPlayer(GameObject objectToSpawn, float minDistance, float maxDistance)
    {
        if (playerTransform == null) return null;

        Vector3 randomDirection = Random.onUnitSphere;
        float randomDistance = Random.Range(minDistance, maxDistance);
        Vector3 spawnPosition = playerTransform.position + randomDirection * randomDistance;
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        if (objectToSpawn == specialObjectToSpawn)
        {
            spawnedObject.transform.LookAt(playerTransform.position);
            if (!specialObjectSpawned)
            {
                specialObjectSpawned = true; // Ensure the special object is only spawned once
                if (specialSpawnAudioSource != null)
                {
                    specialSpawnAudioSource.Play();
                }
                if (arrow != null)
                {
                    arrow.SetActive(true);
                    arrow.transform.LookAt(spawnedObject.transform);
                }
            }
        }

        return spawnedObject;
    }

    IEnumerator SpawnSpecialObjectOnce()
    {
        if (specialObjectSpawned) yield break;

        float waitTime = Random.Range(120f, 135f); // Random time between 5 to 10 minutes
        yield return new WaitForSeconds(waitTime);

        SpawnObjectAroundPlayer(specialObjectToSpawn, 3000f, 5000f); // Spawning the special object at a greater distance

        specialSpawnAudioSource.Play();


    }

    void Update()
    {
        if (arrow != null && specialObjectSpawned && arrow.activeSelf)
        {
            // Assuming specialSpawnedObject is the last spawned special object
            // Update to keep the arrow pointing towards the special spawned object if it moves
            GameObject specialSpawnedObject = GameObject.Find(specialObjectToSpawn.name + "(Clone)"); // Find the special object instance
            if (specialSpawnedObject != null)
            {
                arrow.transform.LookAt(specialSpawnedObject.transform);
            }
        }
    }
}
