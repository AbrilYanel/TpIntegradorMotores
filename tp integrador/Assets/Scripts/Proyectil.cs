using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public int damageAmount = 10;
    public float destroyDelay = 2f; // Tiempo en segundos antes de destruir el proyectil

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy_Controller enemy = collision.gameObject.GetComponent<Enemy_Controller>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
                Debug.Log("Enemy damaged by: " + damageAmount);
            }
          
        }
    }

   
}