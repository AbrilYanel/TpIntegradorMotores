using UnityEngine;
using System.Collections;
public class EnemyBoss : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;

    public float moveSpeed = 3f;
    public float rotationSpeed = 2f;
    public float detectionRange = 15f;
    public float shootingRange = 10f;
    public float shootingRate = 1f; // Proyectiles por segundo
    public int damageAmount = 20;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public GameObject dropObject; // Objeto que se soltará al morir

    public Vector2 boundsX = new Vector2(-10, 10);
    public Vector2 boundsZ = new Vector2(-10, 10);

    private Transform player;
    private float lastShotTime;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();

            if (distanceToPlayer <= shootingRange && Time.time > lastShotTime + 1f / shootingRate)
            {
                ShootProjectile();
                lastShotTime = Time.time;
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, boundsX.x, boundsX.y);
        newPosition.z = Mathf.Clamp(newPosition.z, boundsZ.x, boundsZ.y);

        transform.position = newPosition;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * 10f; // Velocidad del proyectil
                Destroy(projectile, 2f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (dropObject != null)
        {
            Instantiate(dropObject, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}