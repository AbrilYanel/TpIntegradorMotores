using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegundoPuzzle : MonoBehaviour
{
    public GameObject cube;                 // El cubo que colisionará con la placa
    public GameObject plate;                // La placa con la que colisionará el cubo
    public GameObject cylinder1;            // Primer cilindro a activar
    public GameObject cylinder2;            // Segundo cilindro a activar
    public GameObject cylinder3;            // Tercer cilindro a activar

    void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisiona con la placa es el cubo
        if (collision.gameObject == cube)
        {
            Debug.Log("Colisionaron");
            // Activa los cilindros y hace sus renderers visibles
            ActivateRenderer(cylinder1);
            ActivateRenderer(cylinder2);
            ActivateRenderer(cylinder3);
        }
    }

    void ActivateRenderer(GameObject cylinder)
    {
        if (cylinder != null)
        {
            cylinder.SetActive(true); // Activa el objeto
            Renderer cylinderRenderer = cylinder.GetComponent<Renderer>();
            if (cylinderRenderer != null)
            {
                cylinderRenderer.enabled = true; // Activa el renderer
            }
        }
    }
}
