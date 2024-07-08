using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public Vector2 xBounds = new Vector2(-4.551966f, 4.906345f);
    public Vector2 zBounds = new Vector2(13.74371f, 21.36251f);
    public Vector3 respawnPosition = new Vector3(0.05647466f, 0f, 20.90274f);

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (IsPlayerInRange())
        {
            FollowPlayer();
        }
        CheckBounds();
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void CheckBounds()
    {
        if (transform.position.x < xBounds.x || transform.position.x > xBounds.y ||
            transform.position.z < zBounds.x || transform.position.z > zBounds.y)
        {
            rb.position = respawnPosition;
        }
    }
}
