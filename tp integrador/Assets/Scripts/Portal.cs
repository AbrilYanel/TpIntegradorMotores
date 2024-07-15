using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 destinationPosition; // Coordenadas de destino donde se teleportará el jugador

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Teletransportar al jugador a la posición especificada
            other.transform.position = destinationPosition;
        }
    }
}
