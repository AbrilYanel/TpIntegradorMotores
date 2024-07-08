using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    public Transform flowerPosition; // Punto hacia donde se atrae el jugador
    public Transform groundObject; // Objeto sobre el cual debe estar el jugador para activar el gancho
    public float attractionForce = 20f; // Fuerza de atracci�n hacia la flor
    public float maxAttractionDistance = 10f; // Distancia m�xima de atracci�n
    public KeyCode hookKey = KeyCode.T; // Tecla para activar el gancho

    private Rigidbody rb;
    private bool isHooking = false;
    private bool isBeingAttracted = false;
    private Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(hookKey) && !isHooking && IsPlayerOnGround())
        {
            TryHook();
        }

        if (isBeingAttracted)
        {
            AttractPlayer();
        }
    }

    bool IsPlayerOnGround()
    {
        // Calcula la distancia entre el jugador y el objeto espec�fico
        float distance = Vector3.Distance(transform.position, groundObject.position);

        // Verifica si el jugador est� encima del objeto espec�fico y dentro del rango
        return distance <= maxAttractionDistance && IsPlayerAboveGroundObject();
    }

    bool IsPlayerAboveGroundObject()
    {
        // Raycast hacia abajo desde la posici�n del jugador para detectar si est� sobre el objeto requerido
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            // Verificar si el objeto sobre el cual est� el jugador coincide con el objeto requerido
            return hit.collider.transform == groundObject;
        }
        return false;
    }

    void TryHook()
    {
        // Comprobar si el jugador est� cerca del punto de la flor y dentro del rango
        if (Vector3.Distance(transform.position, flowerPosition.position) <= maxAttractionDistance)
        {
            isHooking = true;
            isBeingAttracted = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true; // Desactivar la f�sica del Rigidbody para controlar el movimiento manualmente
        }
    }

    void AttractPlayer()
    {
        // Calcular la direcci�n hacia la flor
        Vector3 direction = (flowerPosition.position - transform.position).normalized;

        // Aplicar la fuerza de atracci�n
        rb.MovePosition(transform.position + direction * attractionForce * Time.deltaTime);

        // Verificar si el jugador ha llegado a la posici�n de la flor
        if (Vector3.Distance(transform.position, flowerPosition.position) < 0.1f)
        {
            FinishAttraction();
        }
    }

    void FinishAttraction()
    {
        isBeingAttracted = false;
        rb.isKinematic = false; // Reactivar la f�sica del Rigidbody
        rb.velocity = Vector3.zero;
        transform.position = flowerPosition.position; // Asegurar que el jugador est� exactamente en la posici�n de la flor
        isHooking = false;
    }
}
