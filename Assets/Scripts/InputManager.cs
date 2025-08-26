using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    [SerializeField] private SoInputEvents _soInputEvents;
    
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Rotate.started += OnRotate;
    }



    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Move.started -= OnMove;
        inputActions.Player.Rotate.started -= OnRotate;
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _soInputEvents.RaiseMove(input);
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<float>();
        _soInputEvents.RaiseRotate(input);
    }
}
