using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPendulo : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit = 75f;
    public bool randomStart = false;
    public float initialAngle = 0f;
    public bool invertDirection = false;

    private float random = 0;

    private void Awake()
    {
        if (randomStart)
        {
            random = Random.Range(0, 1f);
        }
    }

    private void Update()
    {
        float directionMultiplier = invertDirection ? -1f : 1f;
        float angle = limit * Mathf.Sin((Time.time + random) * speed) * directionMultiplier + initialAngle;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
