using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueHit : MonoBehaviour
{
    public int requiredHits =5; // N�mero de golpes necesarios para activar el suelo
    public GameObject designatedFloor; // El suelo que se activar�
    public int currentHits = 0; // Contador de golpes actuales

    // M�todo para recibir golpes
    public void TakeHit()
    {
        currentHits++;
        Debug.Log("Golpes actuales: " + currentHits);

        if (currentHits >= requiredHits)
        {
            ActivateFloor();
        }
    }

    // M�todo para activar el suelo designado
    private void ActivateFloor()
    {
        if (designatedFloor != null)
        {
            designatedFloor.SetActive(true);
            Debug.Log("El suelo ha sido activado.");
        }
    }

    // Detectar colisiones con proyectiles
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeHit();
            Destroy(other.gameObject); // Destruir el proyectil despu�s de contar el golpe
        }
    }
}
