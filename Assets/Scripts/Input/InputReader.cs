using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MoveEvent;
    public Vector2 LookPosition { get; private set; } //เพิ่มตัวแปรนี้
    public event Action<bool> PrimaryFireEvent;

    private PlayerInputActions controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerInputActions();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookPosition = context.ReadValue<Vector2>(); //บันทึกค่าตำแหน่งเมาส์
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
    }
}
