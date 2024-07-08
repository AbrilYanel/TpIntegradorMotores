using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class AbrirSuelo : MonoBehaviour
{ 
 // Referencias a los objetos que se ingresarán desde el Inspector
    public GameObject cube;         // El cubo
public GameObject plate;        // La placa
public GameObject objectToDestroy;  // El objeto a destruir

// Método para detectar colisiones
private void OnCollisionEnter(Collision collision)
{
    // Verifica si el objeto colisionado es el cubo
    if (collision.gameObject == cube)
    {
        // Verifica si el cubo también está colisionando con la placa
        if (plate.GetComponent<Collider>().bounds.Intersects(cube.GetComponent<Collider>().bounds))
        {
            // Destruye el objeto especificado
            Destroy(objectToDestroy);
        }
    }
}
}