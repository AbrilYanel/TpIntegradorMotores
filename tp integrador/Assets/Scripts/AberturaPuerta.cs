using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AberturaPuerta : MonoBehaviour
{
    public float moveDistance = 6f;
    public float moveSpeed = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }

    public void OpenDoor()
    {
        if (!isOpening)
        {
            isOpening = true;
            StartCoroutine(MoveDoor());
        }
    }

    IEnumerator MoveDoor()
    {
        float elapsedTime = 0f;

        while (transform.position.y < targetPosition.y)
        {
            float newY = Mathf.Lerp(initialPosition.y, targetPosition.y, elapsedTime / moveSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
