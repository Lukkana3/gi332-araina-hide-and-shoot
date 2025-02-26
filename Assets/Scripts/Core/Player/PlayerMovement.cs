using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
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
        rb.linearVelocity = movementInput * moveSpeed;
    }
}
