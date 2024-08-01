using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player_Controller : MonoBehaviour
{
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float dodgeSpeed = 10f; // Velocidad del esquivar
    public float dodgeDuration = 0.2f; // Duración del esquivar

    public int maxHealth = 100;
    private int currentHealth;

    private Rigidbody rb;
    private Animator animator;

    public GameObject projectilePrefab;
    public Transform shootPoint;

    public Slider healthBar;

    private bool isDodging = false;
    private float lastStepTime = 0f; // Tiempo de la última reproducción del sonido de pasos
    public float stepInterval = 0.2f; // Intervalo entre sonidos de pasos

    public AK.Wwise.Event attackSoundEvent;
    public AK.Wwise.Event walkingSoundEvent; // Evento para el sonido de caminar
    public AK.Wwise.Event rollingSoundEvent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        // Inicializa el Slider de la barra de salud
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        UpdateHealthText();
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Rotate the player based on horizontal input
            if (horizontal != 0)
            {
                float rotation = horizontal * rotationSpeed * Time.fixedDeltaTime;
                Quaternion turnOffset = Quaternion.Euler(0, rotation, 0);
                rb.MoveRotation(rb.rotation * turnOffset);
            }

            // Move the player forward or backward
            if (vertical != 0)
            {
                Vector3 moveDirection = transform.forward * vertical * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + moveDirection);

                // Reproduce el sonido de pasos si ha pasado el intervalo
                if (Time.time - lastStepTime > stepInterval)
                {
                    lastStepTime = Time.time;
                    walkingSoundEvent.Post(gameObject);
                }
            }

            // Update the animator parameters
            animator.SetFloat("speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            if (Input.GetMouseButtonDown(0)) // Disparar con click izquierdo
            {
                animator.SetTrigger("isAttacking");
                attackSoundEvent.Post(gameObject);

                ShootProjectile();
            }

            if (Input.GetMouseButtonDown(1)) // Esquivar con click derecho
            {
                StartCoroutine(Dodge());
                rollingSoundEvent.Post(gameObject);
            }
        }
    }

    IEnumerator Dodge()
    {
        isDodging = true;
        animator.SetTrigger("isDodging");

        Vector3 dodgeDirection = transform.forward;
        float dodgeEndTime = Time.time + dodgeDuration;

        while (Time.time < dodgeEndTime)
        {
            rb.MovePosition(rb.position + dodgeDirection * dodgeSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        isDodging = false;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        UpdateHealthText();
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthText()
    {
        if (healthBar.value != currentHealth)
        {
            healthBar.value = currentHealth;
        }
    }

    void ShootProjectile()
    {
        if (shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            // Disparar en la dirección hacia adelante del shootPoint
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootPoint.forward * 10f; // Velocidad del proyectil
                Destroy(projectile, 1f);
            }
        }
    }

    void GameOver()
    {
        // Aquí puedes añadir lógica para reiniciar el juego, mostrar un mensaje de game over, etc.
        Debug.Log("Game Over");
        // Ejemplo: reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}