using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cofre"))
        {
            Debug.Log("�Ganaste!");
            Time.timeScale = 0f; 
            // Aqu� puedes a�adir l�gica adicional, como mostrar un mensaje de victoria o reiniciar el nivel
        }
    }
}