using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sierra : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over");
            AkSoundEngine.StopAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
