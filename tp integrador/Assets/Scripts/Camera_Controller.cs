using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Transform player;
    public LayerMask wallLayerMask;
    public float cameraRadius = 0.5f;
    public float smoothSpeed = 10.0f;

    private Vector3 initialCameraOffset;

    void Start()
    {
        initialCameraOffset = transform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 desiredCameraPosition = player.TransformPoint(initialCameraOffset);
        Vector3 directionToCamera = (desiredCameraPosition - player.position).normalized;
        float maxDistance = initialCameraOffset.magnitude;

        RaycastHit hit;
        if (Physics.SphereCast(player.position, cameraRadius, directionToCamera, out hit, maxDistance, wallLayerMask))
        {
            Vector3 collisionPoint = player.position + directionToCamera * (hit.distance - cameraRadius);
            transform.position = Vector3.Lerp(transform.position, collisionPoint, Time.deltaTime * smoothSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, Time.deltaTime * smoothSpeed);
        }
    }
}
