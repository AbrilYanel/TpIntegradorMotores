using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damage = 10f; // Da�o infligido por el proyectil
    public float lifetime = 2f; // Tiempo de vida del proyectil

    private void Start()
    {
        // Destruye el proyectil despu�s de un tiempo para evitar acumulaci�n
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obt�n el componente Player_Controller del jugador
            Player_Controller playerController = other.GetComponent<Player_Controller>();
            if (playerController != null)
            {
                playerController.TakeDamage((int)damage); // Aplica el da�o al jugador
            }

            // Destruye el proyectil al impactar
            Destroy(gameObject);
        }
    }
}
