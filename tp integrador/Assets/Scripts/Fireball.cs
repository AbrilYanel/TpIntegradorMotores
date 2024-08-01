using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damage = 10f; // Daño infligido por el proyectil
    public float lifetime = 2f; // Tiempo de vida del proyectil

    private void Start()
    {
        // Destruye el proyectil después de un tiempo para evitar acumulación
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtén el componente Player_Controller del jugador
            Player_Controller playerController = other.GetComponent<Player_Controller>();
            if (playerController != null)
            {
                playerController.TakeDamage((int)damage); // Aplica el daño al jugador
            }

            // Destruye el proyectil al impactar
            Destroy(gameObject);
        }
    }
}
