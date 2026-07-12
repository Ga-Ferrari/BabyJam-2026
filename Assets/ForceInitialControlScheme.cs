using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ForceInitialControlScheme : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void Start()
    {
        if (Keyboard.current != null)
            InputUser.PerformPairingWithDevice(Keyboard.current, playerInput.user);

        if (Mouse.current != null)
            InputUser.PerformPairingWithDevice(Mouse.current, playerInput.user);

        foreach (var gamepad in Gamepad.all)
            InputUser.PerformPairingWithDevice(gamepad, playerInput.user);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added && device is Gamepad)
            InputUser.PerformPairingWithDevice(device, playerInput.user);
    }
}