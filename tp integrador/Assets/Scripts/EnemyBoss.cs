using UnityEngine;

public class EnemyBoss : Enemy_Controller
{
    public GameObject chestPrefab; // Prefab del cofre a instanciar
    public Vector3 chestSpawnPosition; // Posición de spawn del cofre

    protected override void Die()
    {
        base.Die(); // Llamar al método Die de la clase base (Enemy_Controller)

        // Spawnear el cofre en la posición designada
        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, chestSpawnPosition, Quaternion.identity);
        }
    }
}