using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptAraña : MonoBehaviour
{
    public int damage = 2;              // Daño que hace la araña al jugador
    public int health = 20;             // Vida de la araña
    public float movementSpeed = 10f;   // Velocidad de movimiento de la araña (aumentada a 10f)
    public Bounds movementBounds;       // Límites de movimiento de la araña
    public float jumpForce = 150f;      // Fuerza de salto de la araña
    public float jumpInterval = 2f;     // Intervalo de salto en segundos

    private GameObject player;          // Referencia al jugador
    private Rigidbody rb;
    private bool isActivated = false;   // Bandera para saber si la araña está activada

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DisableSpider();
        movementBounds = new Bounds(new Vector3(-20.77528f, 0f, 17.67536f), new Vector3(9.09007f, 0f, 10.69517f));
    }

    public void ActivateSpider(Transform playerTransform)
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            isActivated = true;
            StartCoroutine(SpiderBehavior());
        }
        else
        {
            Debug.LogWarning("No player found with tag 'Player'.");
        }
    }

    IEnumerator SpiderBehavior()
    {
        while (isActivated)
        {
            FollowPlayer();
            CheckMovementBounds();
            Jump(); // Llamar al método de salto

            yield return new WaitForSeconds(0.1f); // Ajustar el intervalo de actualización según sea necesario
        }
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            // Calcular dirección hacia el jugador
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * movementSpeed * Time.deltaTime;

            // Mover hacia la posición del jugador
            rb.MovePosition(newPosition);

            // Rotar hacia el jugador (opcional)
            Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, lookRotation, Time.deltaTime * 5f));
        }
    }

    void CheckMovementBounds()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, movementBounds.min.x, movementBounds.max.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, movementBounds.min.z, movementBounds.max.z);
        transform.position = clampedPosition;
    }

    void Jump()
    {
        // Saltar cada jumpInterval segundos
        if (Time.time % jumpInterval < 0.1f) // Aproximadamente cada jumpInterval segundos
        {
            // Añadir una fuerza hacia arriba para simular el salto
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void DisableSpider()
    {
        isActivated = false;
        StopAllCoroutines();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_Controller playerController = collision.gameObject.GetComponent<Player_Controller>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
