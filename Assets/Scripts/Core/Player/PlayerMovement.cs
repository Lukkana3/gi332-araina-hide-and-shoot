using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform bodyTransform; // ����Ф���ѡ
    [SerializeField] private Transform gunTransform; // �׹�¡�͡�ҡ����Ф�

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
        if (!IsOwner) return; //��ͧ�ѹ Client ����ҤǺ�������Фù��

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

        lookPosition = inputReader.LookPosition; // �Ѻ��ҵ��˹������
        RotateGun();
    }

    private void Move()
    {
        if (movementInput.sqrMagnitude > 0.01f) // ��ҡ���������Թ
        {
            // �ӹǳ����ͧ����Թ (���������ǡѺ�����)
            float targetAngle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(bodyTransform.eulerAngles.z, targetAngle, 10f * Time.deltaTime);

            // ��ع����Ф�仵����ȷҧ��衴����
            bodyTransform.rotation = Quaternion.Euler(0f, 0f, smoothedAngle);
        }

        // �Թ仢�ҧ˹�ҵ����ȷҧ������Ф��ѹ
        rb.linearVelocity = bodyTransform.up * movementInput.magnitude * moveSpeed;
    }

    private void RotateGun()
    {
        if (gunTransform == null) return;

        Vector3 aimWorldPosition = Camera.main.ScreenToWorldPoint(lookPosition);
        aimWorldPosition.z = 0f; // ��͡᡹ Z ����� 0 (�������� 2D)

        Vector2 direction = (aimWorldPosition - gunTransform.position).normalized;
        gunTransform.up = direction; //�׹�ѹ价ҧ�����������觼ŵ�͵���Ф�
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
        if (!IsOwner) return; // ��ͧ�ѹ Client ����ҤǺ���

        lookPosition = inputReader.LookPosition; // �Ѻ��ҵ��˹������
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
        aimWorldPosition.z = 0f; // ��͡᡹ Z ����� 0 (�������� 2D)

        Vector2 direction = (aimWorldPosition - gunTransform.position).normalized;
        gunTransform.up = direction; // �� up vector �������׹���价ҧ�����
    }
}*/