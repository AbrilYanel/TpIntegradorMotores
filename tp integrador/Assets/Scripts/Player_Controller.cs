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

    public int maxHealth = 100;
    private int currentHealth;

    private Rigidbody rb;
    private Animator animator;

    public Text healthText;
    public Slider healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        healthText = GameObject.Find("Vida").GetComponent<Text>();
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        // Inicializa el Slider de la barra de salud
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        UpdateHealthText();
    }

    void FixedUpdate()
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
        }

        // Update the animator parameters
        animator.SetFloat("speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
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
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth.ToString();
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