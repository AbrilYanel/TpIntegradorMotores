using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Controller : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;

    public Transform player;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f; // Velocidad de rotación
    public float detectionRange = 5f;
    public Vector2 xBounds = new Vector2(-4.551966f, 4.906345f);
    public Vector2 zBounds = new Vector2(13.74371f, 21.36251f);
    public Vector3 respawnPosition = new Vector3(0.05647466f, 0f, 20.90274f);
    public int damageAmount = 10;
    public float damageRate = 1f; // Daño por segundo

    public GameObject puertaDerecha;
    public Slider healthBar;

    private Rigidbody rb;
    private Animator animator;
    private bool isPlayerInRange;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        isPlayerInRange = false;
        StartCoroutine(ApplyContinuousDamage());

        // Inicializa el Slider de la barra de salud
        healthBar = GameObject.Find("EnemyHealthBar").GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void FixedUpdate()
    {
        if (IsPlayerInRange())
        {
            FollowPlayer();
        }
        CheckBounds();

        // Actualiza el parámetro "Speed" del Animator basado en la velocidad del enemigo
        float speed = rb.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
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
    }

    void CheckBounds()
    {
        if (transform.position.x < xBounds.x || transform.position.x > xBounds.y ||
            transform.position.z < zBounds.x || transform.position.z > zBounds.y)
        {
            rb.position = respawnPosition;
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

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Actualiza el Slider de la barra de salud
        if (healthBar.value != currentHealth)
        {
            healthBar.value = currentHealth;
        }

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

        // Activar el movimiento de la puerta derecha si está asignada
        if (puertaDerecha != null)
        {
            AberturaPuerta doorOpener = puertaDerecha.GetComponent<AberturaPuerta>();
            if (doorOpener != null)
            {
                doorOpener.OpenDoor();
            }
            else
            {
                Debug.LogError("No se encontró el componente AberturaPuerta en " + puertaDerecha.name);
            }
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


