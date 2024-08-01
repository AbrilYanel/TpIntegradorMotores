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
        else if (other.CompareTag("Boss"))
        {
            EnemyBoss boss = other.GetComponent<EnemyBoss>();
            if (boss != null)
            {
                boss.TakeDamage(damageAmount);
                Debug.Log("Boss damaged by: " + damageAmount);
            }

            Destroy(gameObject, destroyDelay);
        } else if (other.CompareTag("Spider"))
        {
            ScriptAraña araña = other.GetComponent<ScriptAraña>();
            if (araña != null)
            {
                araña.TakeDamage(damageAmount);
                Debug.Log("Spider damaged");
            }
        }
    }
}