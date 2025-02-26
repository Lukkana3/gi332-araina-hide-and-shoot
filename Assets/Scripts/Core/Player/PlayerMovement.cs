using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform gunTransform; // เพิ่ม Gun ที่จะหมุนตามเมาส์

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 13f;

    private Vector2 movementInput;
    private Vector2 mousePosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.LookEvent += OnLook; //รับค่าเมาส์จาก InputReader
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.LookEvent -= OnLook;
    }

    private void OnMove(Vector2 movement)
    {
        movementInput = movement;
    }

    private void OnLook(Vector2 look)
    {
        mousePosition = look;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        RotateGun();
    }

    private void Move()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void RotateGun()
    {
        if (gunTransform == null) return;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (worldMousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}







/*using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 13f;

    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 movement)
    {
        movementInput = movement;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
}*/