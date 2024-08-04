using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Acido : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AkSoundEngine.StopAll();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
