using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceJump : MonoBehaviour
{
    public GameObject winText;


    public void Start()
    {

        winText = GameObject.FindGameObjectWithTag("Win");
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Start the scale and destroy coroutine
            StartCoroutine(ScaleAndDestroyPlayer(other.gameObject));
        }
    }

    IEnumerator ScaleAndDestroyPlayer(GameObject player)
    {
        float duration = 0.7f; // Duration in seconds
        float currentTime = 0f; // Track the current time
        Vector3 originalScale = player.transform.localScale; // Store the original scale
        Vector3 targetScale = new Vector3(originalScale.x, originalScale.y, 10); // Target scale

        // Smoothly scale the player over the duration
        while (currentTime <= duration)
        {
            // Calculate the current time ratio
            float t = currentTime / duration;
            // Interpolate the scale based on the current time ratio
            player.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            // Increment the current time
            currentTime += Time.deltaTime;
            // Wait for the next frame
            yield return null;
        }

        // Ensure the player is scaled exactly to the target scale
        player.transform.localScale = targetScale;

        // Destroy the player GameObject
        Destroy(player);
        //Win Text

        Invoke("WinScene",1f);
        
    }

    void WinScene()
    {
        SceneManager.LoadScene("Win scene");
    }
}
