using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBeta : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 move = new Vector2();

        if (Input.GetKey(KeyCode.I))
            move += Vector2.up;

        if (Input.GetKey(KeyCode.K))
            move += Vector2.down;

        if (Input.GetKey(KeyCode.J))
            move += Vector2.left;

        if (Input.GetKey(KeyCode.L))
            move += Vector2.right;

        move.Normalize();

        move *= speed;

        rb.position += move;
    }
}
