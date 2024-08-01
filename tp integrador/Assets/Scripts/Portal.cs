using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 destinationPosition; // Coordenadas de destino donde se teleportará el jugador
    public AK.Wwise.Event Event;

    private bool hasPlayed = false; // Variable para controlar si el evento ya ha sido reproducido

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Verificar si el evento ya ha sido reproducido
            if (!hasPlayed)
            {
                Event.Post(gameObject); // Reproducir el evento
                hasPlayed = true; // Marcar el evento como reproducido
            }

            // Teletransportar al jugador a la posición especificada
            other.transform.position = destinationPosition;
        }
    }
}
