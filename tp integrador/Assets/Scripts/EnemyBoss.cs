using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
public class EnemyBoss : MonoBehaviour
{
    public int health = 250;
    public Slider healthBarSlider; // Referencia al Slider de la barra de vida
    public Transform player; // Referencia al jugador
    public float meleeDamage = 10f; // Da�o del ataque cuerpo a cuerpo
    public float meleeRange = 2f; // Rango del ataque cuerpo a cuerpo
    public float attackCooldown = 2f; // Tiempo entre ataques
    public GameObject fireballPrefab; // Prefab de la bola de fuego
    public Transform fireballSpawnPoint; // Punto de spawn de la bola de fuego
    public float fireballCooldown = 5f; // Tiempo entre disparos de la bola de fuego
    public float fireballLaunchForce = 10f; // Fuerza de lanzamiento de la bola de fuego
    public float teleportCooldown = 5f; // Tiempo entre teletransportaciones
    public float teleportHealthThreshold = 50f; // Umbral de salud para empezar a teletransportarse
    public Transform teleportAreaCenter; // Centro del �rea de teletransportaci�n
    public float teleportAreaRadius = 10f; // Radio del �rea de teletransportaci�n
    public GameObject dropOnDeath; // GameObject que se dejar� en una posici�n designada al morir
    public Transform dropPosition; // Posici�n donde se dejar� el GameObject al morir
    public Animator animator; // Referencia al componente Animator

    [SerializeField]
    private int currentHealth;
    private bool isDead = false; // Estado del jefe
    private bool canAttack = true; // Controla si el jefe puede atacar
    private bool canShootFireball = true; // Controla si el jefe puede disparar una bola de fuego
    private bool canTeleport = true; // Controla si el jefe puede teletransportarse
    public float moveSpeed = 3f; // Velocidad de movimiento del jefe

    private void Start()
    {
        currentHealth = health;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = health;
            healthBarSlider.value = currentHealth;
            healthBarSlider.gameObject.SetActive(true); // Aseg�rate de que la barra de vida est� visible
        }
    }

    private void Update()
    {
        if (isDead) return;

        FollowAndLookAtPlayer();

        if (Vector3.Distance(transform.position, player.position) <= meleeRange && canAttack)
        {
            StartCoroutine(MeleeAttack());
        }
        else if (Vector3.Distance(transform.position, player.position) > 5f && canShootFireball)
        {
            StartCoroutine(ShootFireball());
        }

        if (currentHealth <= teleportHealthThreshold && canTeleport)
        {
            StartCoroutine(Teleport());
        }
    }

    private void FollowAndLookAtPlayer()
    {
        // Activar animaci�n de movimiento
        if (animator != null)
        {
            animator.SetBool("IsMoving", true);
        }

        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);
        transform.LookAt(player);
    }

    private IEnumerator MeleeAttack()
    {
        canAttack = false;
        Debug.Log("Boss melee attacking!");
        // Aseg�rate de que el jugador tenga el componente Player_Controller
        Player_Controller playerController = player.GetComponent<Player_Controller>();
        if (playerController != null)
        {
            playerController.TakeDamage((int)meleeDamage);
        }
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ShootFireball()
    {
        canShootFireball = false;
        Debug.Log("Boss shooting fireball!");
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = (player.position - fireballSpawnPoint.position).normalized * fireballLaunchForce; // Usar la fuerza de lanzamiento especificada
        }
        yield return new WaitForSeconds(fireballCooldown);
        canShootFireball = true;
    }

    private IEnumerator Teleport()
    {
        canTeleport = false;
        Debug.Log("Boss teleporting!");
        Vector3 randomPosition = teleportAreaCenter.position + Random.insideUnitSphere * teleportAreaRadius;
        randomPosition.y = transform.position.y; // Mantener la misma altura
        transform.position = randomPosition;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // No recibir da�o si ya est� muerto

        currentHealth -= damageAmount;

        // Actualiza el Slider de la barra de salud
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // No procesar la muerte si ya est� muerto

        isDead = true;
        Debug.Log("Boss died!");

        // Desactivar el GameObject del jefe
        gameObject.SetActive(false);

        // Ocultar la barra de salud
        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(false);
        }

        // Detener animaci�n de movimiento
        if (animator != null)
        {
            animator.SetBool("IsMoving", false);
            // Iniciar animaci�n de muerte aqu� (si se tiene)
            // animator.SetTrigger("Die");
        }

        // Dejar el GameObject en la posici�n designada
        if (dropOnDeath != null && dropPosition != null)
        {
            Instantiate(dropOnDeath, dropPosition.position, dropPosition.rotation);
        }
    }
}