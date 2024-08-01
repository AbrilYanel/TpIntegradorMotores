using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBall : MonoBehaviour
{
    public GameObject player; // Referencia al jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Player_Controller playerController = player.GetComponent<Player_Controller>();
            if (playerController != null)
            {
                playerController.RestoreHealthToFull();
                Destroy(gameObject); // Destruir la esfera después de restaurar la salud del jugador
            }
        }
    }
}
