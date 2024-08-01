using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder;
public class EnemyBoss : MonoBehaviour
{
    public Transform player; // Referencia al transform del jugador
    public float followSpeed = 5f; // Velocidad de seguimiento
    public float rotationSpeed = 2f; // Velocidad de rotaci�n
    public float detectionRange = 20f; // Rango de detecci�n

    public int maxHealth = 500; // Vida m�xima del jefe
    public int currentHealth; // Vida actual del jefe

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa la vida actual
    }

    private void Update()
    {
        // Verificar la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Mover al jefe hacia el jugador
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * followSpeed * Time.deltaTime;

            // Rotar el jefe para mirar hacia el jugador
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // M�todo para recibir da�o
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo que se llama cuando el jefe muere
    void Die()
    {
        // L�gica para manejar la muerte del jefe, como eliminar el objeto o reproducir animaci�n
        Destroy(gameObject); // Ejemplo: eliminar el jefe del juego
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto colisionado es un proyectil
        if (other.CompareTag("Projectile"))
        {
            // Asumiendo que el proyectil tiene un componente Proyectil que define el da�o
            Proyectil projectile = other.GetComponent<Proyectil>();
            if (projectile != null)
            {
                TakeDamage(projectile.damageAmount); // Aplica el da�o al jefe
                Destroy(other.gameObject); // Destruye el proyectil
            }
        }
    }
}