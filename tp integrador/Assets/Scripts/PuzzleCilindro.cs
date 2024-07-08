using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCilindro : MonoBehaviour
{
    public Color targetColor = Color.yellow; // El color al que cambiará el cilindro
    public GameObject objectToDestroy1;   // Primer objeto a destruir
    public GameObject objectToDestroy2;   // Segundo objeto a destruir

    private Renderer cylinderRenderer;    // Renderer del cilindro
    private bool colorChanged = false;    // Indicador de cambio de color

    void Start()
    {
        cylinderRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisionó es un proyectil (puedes ajustar la lógica según tu sistema de disparo)
        if (collision.gameObject.CompareTag("Projectile") && !colorChanged)
        {
            // Cambia el color del cilindro
            cylinderRenderer.material.color = targetColor;
            colorChanged = true;

            // Destruye los objetos especificados
            if (objectToDestroy1 != null)
            {
                Destroy(objectToDestroy1);
            }
            if (objectToDestroy2 != null)
            {
                Destroy(objectToDestroy2);
            }
        }
    }
}