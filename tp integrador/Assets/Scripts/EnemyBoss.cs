using UnityEngine;
using System.Collections;
public class EnemyBoss : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;

    public Transform player;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float detectionRange = 5f;

    public Vector2 xBounds = new Vector2(-4.551966f, 4.906345f);
    public Vector2 zBounds = new Vector2(13.74371f, 21.36251f);
    public Vector3 respawnPosition = new Vector3(0.05647466f, 0f, 20.90274f);

    public GameObject chestPrefab; // Prefab del cofre a instanciar
    public Vector3 chestSpawnPosition; // Posición de spawn del cofre

    public GameObject projectilePrefab; // Prefab del proyectil a disparar
    public Transform firePoint; // Punto desde el cual se disparan los proyectiles
    public float fireRate = 2f; // Tiempo entre disparos
    public float projectileSpeed = 10f; // Velocidad del proyectil

    public int damageAmount = 10;
    public float damageRate = 1f; // Daño por segundo

    private Rigidbody rb;
    private bool isPlayerInRange;
    private bool isDead = false;
    private float nextFireTime = 0f; // Tiempo para el próximo disparo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        isPlayerInRange = false;
        StartCoroutine(ApplyContinuousDamage());

        // Asegúrate de que Use Gravity está activado
        if (rb != null)
        {
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        Debug.Log("EnemyBoss initialized.");
    }

    void Update()
    {
        isPlayerInRange = IsPlayerInRange();

        // Solo dispara si el jugador está en rango
        if (isPlayerInRange && Time.time >= nextFireTime && !isDead && player != null)
        {
            ShootProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        if (isPlayerInRange)
        {
            FollowPlayer();
        }
        CheckBounds();

        // Asegúrate de que el Rigidbody del Boss no se esté moviendo hacia abajo
        if (rb != null && rb.velocity.y < 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    bool IsPlayerInRange()
    {
        bool inRange = Vector3.Distance(transform.position, player.position) <= detectionRange;
        Debug.Log("IsPlayerInRange: " + inRange);
        return inRange;
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;

        // Calcular la rotación hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime);

        // Aplicar la nueva posición y rotación al enemigo
        rb.MovePosition(newPosition);
        rb.MoveRotation(rotation);

        Debug.Log("Following player to position: " + newPosition);
    }

    void CheckBounds()
    {
        if (transform.position.x < xBounds.x || transform.position.x > xBounds.y ||
            transform.position.z < zBounds.x || transform.position.z > zBounds.y)
        {
            rb.position = respawnPosition;
            Debug.Log("EnemyBoss out of bounds, respawning at: " + respawnPosition);
        }
    }

    IEnumerator ApplyContinuousDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageRate);
            if (isPlayerInRange && !isDead)
            {
                Player_Controller playerController = player.GetComponent<Player_Controller>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damageAmount);
                    Debug.Log("Player damaged by: " + damageAmount);
                }
            }
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (player.position - firePoint.position).normalized;
                rb.velocity = direction * projectileSpeed; // Ajusta la velocidad del proyectil
                Debug.Log("Projectile shot at speed: " + projectileSpeed);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Enemy died!");

        // Detener la corrutina ApplyContinuousDamage
        StopCoroutine(ApplyContinuousDamage());

        // Spawnear el cofre en la posición designada
        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, chestSpawnPosition, Quaternion.identity);
        }

        // Desactivar el GameObject del enemigo en lugar de destruirlo
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

}