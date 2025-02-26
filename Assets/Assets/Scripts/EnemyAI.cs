using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float speed = 5f; // Speed at which the AI moves towards the player

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Check if the playerTransform has been assigned to avoid errors
        if (playerTransform != null)
        {
            // Move the AI towards the player's position on all axes
            Vector3 newPosition = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            transform.position = newPosition;

            // Make the AI face the player
            transform.LookAt(playerTransform.position);
        }
    }

    /*void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Destroy the player GameObject
            Destroy(other.gameObject);
           
        }

       
    }*/

    
}
