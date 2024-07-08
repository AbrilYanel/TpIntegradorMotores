using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{

    
    public Text winText; // Referencia al Texto donde mostrar el mensaje

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cofre"))
        {
            winText.text = "¡Ganaste!";
            Debug.Log("¡Ganaste!");
           
            Time.timeScale = 0f; 
            
        }
    }
}