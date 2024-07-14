using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public int damageAmount = 100;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                Debug.Log("Player damaged by pendulum: " + damageAmount);
            }
        }
    }
}
