using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DTester : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
}
