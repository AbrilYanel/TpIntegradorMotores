using UnityEngine;

public class EnemyBoss : Enemy_Controller
{
    public GameObject chestPrefab; // Prefab del cofre a instanciar
    public Vector3 chestSpawnPosition; // Posici�n de spawn del cofre

    protected override void Die()
    {
        base.Die(); // Llamar al m�todo Die de la clase base (Enemy_Controller)

        // Spawnear el cofre en la posici�n designada
        if (chestPrefab != null)
        {
            Instantiate(chestPrefab, chestSpawnPosition, Quaternion.identity);
        }
    }
}