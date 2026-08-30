using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class VirtualCursor : MonoBehaviour
{
    public enum DeviceType
    {
        Gamepad,
        Keyboard,
        Mouse,
        Joystick,
        Touchscreen
        // Add other device types as needed
    }



    [Header("References")]
    public RectTransform cursorTransform;
    public Canvas canvas;
    public float moveSpeed = 100f;

    [Header("Input")]
    public PlayerInput playerInput;
    public DeviceType defaultControlScheme;
    public DeviceType[] deviceTypesToWatch = new DeviceType[]
    {
        DeviceType.Gamepad,
        DeviceType.Keyboard
    };

    private Vector2 screenBounds;
    private Vector2 input;

    void Awake()
    {
        screenBounds = new Vector2(Screen.width, Screen.height);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    public void ReadCursor(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    void Update()
    {
        Vector2 currentPos = cursorTransform.anchoredPosition;

        // In case we use the mouse, we lower the sensivity
        Vector2 newPos;
        if (playerInput.currentControlScheme == "Keyboard")
            newPos = currentPos + input * moveSpeed / 10 * Time.unscaledDeltaTime;
        else
            newPos = currentPos + input * moveSpeed * Time.unscaledDeltaTime;


        // Clamp to screen bounds
        newPos.x = Mathf.Clamp(newPos.x, 0, screenBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, 0, screenBounds.y);

        cursorTransform.anchoredPosition = newPos;
        // Convert anchoredPosition to screen position
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, cursorTransform.position);

        // Update the system cursor position for UI interactions
        Mouse.current.WarpCursorPosition(screenPos);
        InputState.Change(Mouse.current.position, screenPos);

    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Disconnected || change == InputDeviceChange.Removed)
            foreach (DeviceType type in deviceTypesToWatch)
            {
                if (IsDeviceType(device, type))
                {
                    playerInput.SwitchCurrentControlScheme("Keyboard", Keyboard.current, Mouse.current);
                    break;
                }
            }
        else if (change == InputDeviceChange.Reconnected || change == InputDeviceChange.Added)
            if (device is Gamepad)
                playerInput.SwitchCurrentControlScheme("Gamepad", device);
    }

    private bool IsDeviceType(InputDevice device, DeviceType type)
    {
        switch (type)
        {
            case DeviceType.Gamepad:
                return device is Gamepad;
            case DeviceType.Keyboard:
                return device is Keyboard;
            case DeviceType.Mouse:
                return device is Mouse;
            case DeviceType.Joystick:
                return device is Joystick;
            case DeviceType.Touchscreen:
                return device is Touchscreen;
        }
        return false;
    }
}
