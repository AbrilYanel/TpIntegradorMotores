using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
public class EnemyBoss : MonoBehaviour
{
    public int health = 250;
    public float moveSpeed = 1f;
    public float leftLimit = -10f;
    public float rightLimit = 10f;

    public Slider healthBarSlider; // Referencia al Slider de la barra de vida

    // Variable para visualizar la vida actual en el Inspector
    [SerializeField]
    private int currentHealth;

    private Vector3 startPosition;
    private bool movingRight = true;
    private bool canMove = false; // Controla si el jefe puede moverse
    private bool isDead = false; // Estado del jefe

    private void Start()
    {
        startPosition = transform.position;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = health;
            healthBarSlider.value = health;
            healthBarSlider.gameObject.SetActive(false); // Ocultar la barra de vida al inicio
        }

        // Inicializa la vida actual
        currentHealth = health;
    }

    private void Update()
    {
        if (canMove && !isDead)
        {
            Move();
        }
    }

    private void Move()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

            if (transform.position.x >= rightLimit)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            if (transform.position.x <= leftLimit)
            {
                movingRight = true;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // No recibir daño si ya está muerto

        currentHealth -= damageAmount;

        // Actualiza el Slider de la barra de salud
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // No procesar la muerte si ya está muerto

        isDead = true;
        Debug.Log("Boss died!");

        // Puedes agregar aquí la lógica adicional para cuando el jefe muere
        // Por ejemplo, desactivar el jefe en lugar de destruirlo
        gameObject.SetActive(false);
    }

    public void StartMoving()
    {
        canMove = true; // Permite que el jefe comience a moverse
    }
}