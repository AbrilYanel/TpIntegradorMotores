using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
public class EnemyBoss : MonoBehaviour
{
    public int health = 250;
    public Slider healthBarSlider; // Referencia al Slider de la barra de vida
    public Transform player; // Referencia al jugador
    public float meleeDamage = 10f; // Daño del ataque cuerpo a cuerpo
    public float meleeRange = 2f; // Rango del ataque cuerpo a cuerpo
    public float attackCooldown = 2f; // Tiempo entre ataques
    public GameObject fireballPrefab; // Prefab de la bola de fuego
    public Transform fireballSpawnPoint; // Punto de spawn de la bola de fuego
    public float fireballCooldown = 5f; // Tiempo entre disparos de la bola de fuego
    public float teleportCooldown = 5f; // Tiempo entre teletransportaciones
    public float teleportHealthThreshold = 50f; // Umbral de salud para empezar a teletransportarse
    public Transform teleportAreaCenter; // Centro del área de teletransportación
    public float teleportAreaRadius = 10f; // Radio del área de teletransportación

    [SerializeField]
    private int currentHealth;
    private bool isDead = false; // Estado del jefe
    private bool canAttack = true; // Controla si el jefe puede atacar
    private bool canShootFireball = true; // Controla si el jefe puede disparar una bola de fuego
    private bool canTeleport = true; // Controla si el jefe puede teletransportarse

    private void Start()
    {
        currentHealth = health;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = health;
            healthBarSlider.value = currentHealth;
            healthBarSlider.gameObject.SetActive(true); // Asegúrate de que la barra de vida esté visible
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
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 3f);
        transform.LookAt(player);
    }

    private IEnumerator MeleeAttack()
    {
        canAttack = false;
        Debug.Log("Boss melee attacking!");
        // Asegúrate de que el jugador tenga el componente Player_Controller
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
        fireball.GetComponent<Rigidbody>().velocity = (player.position - fireballSpawnPoint.position).normalized * 10f; // Velocidad de la bola de fuego
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
        if (isDead) return; // No recibir daño si ya está muerto

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
        if (isDead) return; // No procesar la muerte si ya está muerto

        isDead = true;
        Debug.Log("Boss died!");

        // Puedes agregar aquí la lógica adicional para cuando el jefe muere
        gameObject.SetActive(false);
    }
}