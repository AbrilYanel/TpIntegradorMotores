using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public int damageAmount = 10;
    public float destroyDelay = 2f; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy_Controller enemy = other.GetComponent<Enemy_Controller>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
                Debug.Log("Enemy damaged by: " + damageAmount);
            }

            
            Destroy(gameObject, destroyDelay);
        }
    }

}