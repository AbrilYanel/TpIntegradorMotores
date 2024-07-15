using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int damage = 25; // Da�o que inflige el proyectil

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Controller playerController = other.GetComponent<Player_Controller>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                Destroy(gameObject); // Destruir el proyectil despu�s de hacer da�o
            }
        }
    }
}
