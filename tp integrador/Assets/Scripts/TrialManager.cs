using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialManager : MonoBehaviour
{
    public Transform[] frontSpheres;
    public Transform[] leftSpheres;
    public Transform[] rightSpheres;

    public Image frontArrow; // Referencia a la imagen de la flecha para el frente
    public Image leftArrow;  // Referencia a la imagen de la flecha para la izquierda
    public Image rightArrow; // Referencia a la imagen de la flecha para la derecha

    public float sphereSpeed = 1f; // Velocidad de las esferas
    public float travelDistance = 5f; // Distancia de desplazamiento
    public float challengeDuration = 15f; // Duración total del desafío

    public Slider healthBar; // Barra de vida del jefe en el canvas
    public EnemyBoss boss; // Referencia al jefe

    private Vector3[] frontStartPositions;
    private Vector3[] leftStartPositions;
    private Vector3[] rightStartPositions;

    private void Start()
    {
        frontStartPositions = new Vector3[frontSpheres.Length];
        leftStartPositions = new Vector3[leftSpheres.Length];
        rightStartPositions = new Vector3[rightSpheres.Length];

        for (int i = 0; i < frontSpheres.Length; i++)
        {
            frontStartPositions[i] = frontSpheres[i].position;
        }
        for (int i = 0; i < leftSpheres.Length; i++)
        {
            leftStartPositions[i] = leftSpheres[i].position;
        }
        for (int i = 0; i < rightSpheres.Length; i++)
        {
            rightStartPositions[i] = rightSpheres[i].position;
        }

        // Desactivar flechas y barra de vida al inicio
        frontArrow.enabled = false;
        leftArrow.enabled = false;
        rightArrow.enabled = false;

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false); // Asegúrate de que la barra de vida esté oculta al inicio
        }
    }

    public void ActivateChallenge()
    {
        StartCoroutine(MoveSpheres());
    }

    private IEnumerator MoveSpheres()
    {
        float startTime = Time.time;

        // Secuencial: front -> right -> left
        yield return MoveGroupSequential(frontSpheres, frontStartPositions, Vector3.back, frontArrow); // Cambiado a Vector3.back para Z negativo
        yield return MoveGroupSequential(rightSpheres, rightStartPositions, Vector3.left, rightArrow);
        yield return MoveGroupSequential(leftSpheres, leftStartPositions, Vector3.right, leftArrow);

        // Mostrar la barra de vida del jefe
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }

        // Activar el movimiento del jefe
        if (boss != null)
        {
            boss.StartMoving();
        }

        // Movimiento aleatorio hasta que pasen 15 segundos
        while (Time.time - startTime < challengeDuration)
        {
            yield return MoveGroupRandomly(frontSpheres, frontStartPositions, Vector3.back, frontArrow);
            yield return MoveGroupRandomly(leftSpheres, leftStartPositions, Vector3.right, leftArrow);
            yield return MoveGroupRandomly(rightSpheres, rightStartPositions, Vector3.left, rightArrow);
        }

        // Desactivar todas las esferas y flechas
        DeactivateAllSpheres(frontSpheres, frontArrow);
        DeactivateAllSpheres(leftSpheres, leftArrow);
        DeactivateAllSpheres(rightSpheres, rightArrow);
    }

    private IEnumerator MoveGroupSequential(Transform[] spheres, Vector3[] startPositions, Vector3 direction, Image arrow)
    {
        for (int i = 0; i < spheres.Length; i += 4)
        {
            // Titilar la flecha
            yield return StartCoroutine(FlashArrow(arrow));

            List<IEnumerator> coroutines = new List<IEnumerator>();
            for (int j = 0; j < 4 && (i + j) < spheres.Length; j++)
            {
                coroutines.Add(MoveSphere(spheres[i + j], startPositions[i + j], direction));
            }

            foreach (var coroutine in coroutines)
            {
                StartCoroutine(coroutine);
            }

            // Esperar a que todas las esferas del grupo terminen su movimiento
            yield return new WaitForSeconds(travelDistance / sphereSpeed);
        }
    }

    private IEnumerator MoveGroupRandomly(Transform[] spheres, Vector3[] startPositions, Vector3 direction, Image arrow)
    {
        for (int i = 0; i < spheres.Length; i += 4)
        {
            // Titilar la flecha
            yield return StartCoroutine(FlashArrow(arrow));

            List<IEnumerator> coroutines = new List<IEnumerator>();
            for (int j = 0; j < 4 && (i + j) < spheres.Length; j++)
            {
                coroutines.Add(MoveSphere(spheres[i + j], startPositions[i + j], direction));
            }

            foreach (var coroutine in coroutines)
            {
                StartCoroutine(coroutine);
            }

            // Esperar a que todas las esferas del grupo terminen su movimiento
            yield return new WaitForSeconds(travelDistance / sphereSpeed);
        }
    }

    private IEnumerator FlashArrow(Image arrow)
    {
        for (int i = 0; i < 2; i++)
        {
            arrow.enabled = true;  // Mostrar la flecha
            yield return new WaitForSeconds(0.25f);
            arrow.enabled = false; // Ocultar la flecha
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator MoveSphere(Transform sphere, Vector3 startPosition, Vector3 direction)
    {
        Vector3 endPosition = startPosition + direction * travelDistance;
        float travelTime = travelDistance / sphereSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < travelTime)
        {
            sphere.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sphere.position = startPosition;
    }

    private void DeactivateAllSpheres(Transform[] spheres, Image arrow)
    {
        foreach (Transform sphere in spheres)
        {
            sphere.gameObject.SetActive(false); // Desactivar en lugar de destruir
        }

        // Desactivar la flecha
        if (arrow != null)
        {
            arrow.gameObject.SetActive(false);
        }
    }
}
