using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform bodyTransform; // ตัวละครหลัก
    [SerializeField] private Transform gunTransform; // ปืนแยกออกจากตัวละคร

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 10f;

    private Vector2 movementInput;
    private Vector2 lookPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (!IsOwner) return; //ป้องกัน Client อื่นมาควบคุมตัวละครนี้

        inputReader.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        if (!IsOwner) return;

        inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 movement)
    {
        movementInput = movement;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Move();
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        lookPosition = inputReader.LookPosition; // รับค่าตำแหน่งเมาส์
        RotateGun();
    }

    private void Move()
    {
        if (movementInput.sqrMagnitude > 0.01f) // ถ้ากดปุ่มให้เดิน
        {
            // คำนวณมุมของการเดิน (แต่ไม่เกี่ยวกับเมาส์)
            float targetAngle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(bodyTransform.eulerAngles.z, targetAngle, 10f * Time.deltaTime);

            // หมุนตัวละครไปตามทิศทางที่กดปุ่ม
            bodyTransform.rotation = Quaternion.Euler(0f, 0f, smoothedAngle);
        }

        // เดินไปข้างหน้าตามทิศทางที่ตัวละครหัน
        rb.linearVelocity = bodyTransform.up * movementInput.magnitude * moveSpeed;
    }

    private void RotateGun()
    {
        if (gunTransform == null) return;

        Vector3 aimWorldPosition = Camera.main.ScreenToWorldPoint(lookPosition);
        aimWorldPosition.z = 0f; // ล็อกแกน Z ให้เป็น 0 (เพราะเป็นเกม 2D)

        Vector2 direction = (aimWorldPosition - gunTransform.position).normalized;
        gunTransform.up = direction; //ปืนหันไปทางเมาส์โดยไม่ส่งผลต่อตัวละคร
    }
}









/*using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform gunTransform;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 10f;

    private Vector2 movementInput;
    private Vector2 lookPosition;

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

    private void LateUpdate()
    {
        if (!IsOwner) return; // ป้องกัน Client อื่นมาควบคุม

        lookPosition = inputReader.LookPosition; // รับค่าตำแหน่งเมาส์
        RotateGun();
    }

    private void Move()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }

    private void RotateGun()
    {
        if (gunTransform == null) return;

        Vector3 aimWorldPosition = Camera.main.ScreenToWorldPoint(lookPosition);
        aimWorldPosition.z = 0f; // ล็อกแกน Z ให้เป็น 0 (เพราะเป็นเกม 2D)

        Vector2 direction = (aimWorldPosition - gunTransform.position).normalized;
        gunTransform.up = direction; // ใช้ up vector เพื่อให้ปืนชี้ไปทางเมาส์
    }
}*/