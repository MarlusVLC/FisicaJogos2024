using System;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.transform.position = Vector3.zero;
    }
}
