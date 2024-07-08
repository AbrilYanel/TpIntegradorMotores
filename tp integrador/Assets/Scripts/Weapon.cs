using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Disparar con click izquierdo
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        // Disparar en la dirección hacia adelante del arma
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * 10f; // Velocidad del proyectil
            Destroy(projectile, 2f);
        }
    }
}