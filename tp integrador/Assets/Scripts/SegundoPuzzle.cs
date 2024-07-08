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
            // Activa los cilindros
            ActivateObject(cylinder1);
            ActivateObject(cylinder2);
            ActivateObject(cylinder3);
        }
    }

    void ActivateObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(true); // Activa el objeto
        }
    }
}
