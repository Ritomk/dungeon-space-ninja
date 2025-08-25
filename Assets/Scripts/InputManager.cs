using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private KeyMapper keyMapperSO;
    
    [Header("Input State")]
    [field: SerializeField] public bool CanMove { get; set; } = true;
    [field: SerializeField] public bool CanRotate { get; set; } = true;
    [field: SerializeField] public bool CanInteract { get; set; } = true;
    
    [Header("Input Cooldown")]
    [SerializeField] private float inputCooldown = 0.1f;
    private float lastInputTime = 0f;
    
    // Singleton pattern for easy access
    public static InputManager Instance { get; private set; }
    
    // Input events - other scripts can subscribe to these
    public System.Action OnMoveForward;
    public System.Action OnMoveBackward;
    public System.Action OnMoveLeft;
    public System.Action OnMoveRight;
    public System.Action OnTurnLeft;
    public System.Action OnTurnRight;
    public System.Action OnInteract;
    public System.Action OnMenu;
    public System.Action OnInventory;
    
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
        // Input cooldown to prevent spam
        if (Time.time - lastInputTime < inputCooldown) return;
        
        bool inputReceived = false;
        
        // Movement inputs
        if (CanMove)
        {
            if (GetKeyDown(keyMapperSO.moveForward))
            {
                OnMoveForward?.Invoke();
                inputReceived = true;
            }
            else if (GetKeyDown(keyMapperSO.moveBackward))
            {
                OnMoveBackward?.Invoke();
                inputReceived = true;
            }
            else if (GetKeyDown(keyMapperSO.moveLeft))
            {
                OnMoveLeft?.Invoke();
                inputReceived = true;
            }
            else if (GetKeyDown(keyMapperSO.moveRight))
            {
                OnMoveRight?.Invoke();
                inputReceived = true;
            }
        }
        
        // Rotation inputs
        if (CanRotate)
        {
            if (GetKeyDown(keyMapperSO.turnLeft))
            {
                OnTurnLeft?.Invoke();
                inputReceived = true;
            }
            else if (GetKeyDown(keyMapperSO.turnRight))
            {
                OnTurnRight?.Invoke();
                inputReceived = true;
            }
        }
        
        // Other inputs (no cooldown needed)
        if (CanInteract && GetKeyDown(keyMapperSO.interact))
        {
            OnInteract?.Invoke();
        }
        
        if (GetKeyDown(keyMapperSO.menu))
        {
            OnMenu?.Invoke();
        }
        
        if (GetKeyDown(keyMapperSO.inventory))
        {
            OnInventory?.Invoke();
        }
        
        if (inputReceived)
        {
            lastInputTime = Time.time;
        }
    }
    
    // Input wrapper methods that work with both old and new input systems
    private bool GetKeyDown(KeyCode keyCode)
    {
        #if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && GetKeyboardKey(keyCode).wasPressedThisFrame;
        #else
            return Input.GetKeyDown(keyCode);
        #endif
    }
    
    private bool GetKey(KeyCode keyCode)
    {
        #if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && GetKeyboardKey(keyCode).isPressed;
        #else
            return Input.GetKey(keyCode);
        #endif
    }
    
    private bool GetKeyUp(KeyCode keyCode)
    {
        #if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && GetKeyboardKey(keyCode).wasReleasedThisFrame;
        #else
            return Input.GetKeyUp(keyCode);
        #endif
    }
    
    #if ENABLE_INPUT_SYSTEM
    private UnityEngine.InputSystem.Controls.KeyControl GetKeyboardKey(KeyCode keyCode)
    {
        return keyCode switch
        {
            KeyCode.W => Keyboard.current.wKey,
            KeyCode.S => Keyboard.current.sKey,
            KeyCode.A => Keyboard.current.aKey,
            KeyCode.D => Keyboard.current.dKey,
            KeyCode.Q => Keyboard.current.qKey,
            KeyCode.E => Keyboard.current.eKey,
            KeyCode.Space => Keyboard.current.spaceKey,
            KeyCode.Return => Keyboard.current.enterKey,
            KeyCode.Escape => Keyboard.current.escapeKey,
            KeyCode.Tab => Keyboard.current.tabKey,
            KeyCode.I => Keyboard.current.iKey,
            KeyCode.LeftShift => Keyboard.current.leftShiftKey,
            KeyCode.RightShift => Keyboard.current.rightShiftKey,
            _ => Keyboard.current.spaceKey // Fallback to a valid KeyControl
        };
    }
    #endif
    
    // Public methods for other scripts to check input states
    public bool IsMovementEnabled() => CanMove;
    public bool IsRotationEnabled() => CanRotate;
    public bool IsInteractionEnabled() => CanInteract;
    
    // Methods to temporarily disable input (useful for cutscenes, menus, etc.)
    public void DisableAllInput()
    {
        CanMove = false;
        CanRotate = false;
        CanInteract = false;
    }
    
    public void EnableAllInput()
    {
        CanMove = true;
        CanRotate = true;
        CanInteract = true;
    }
    
    public void SetMovementEnabled(bool enabled) => CanMove = enabled;
    public void SetRotationEnabled(bool enabled) => CanRotate = enabled;
    public void SetInteractionEnabled(bool enabled) => CanInteract = enabled;
    
    // Get continuous input for things that need it (like held keys)
    public bool GetInteractHeld()
    {
        return CanInteract && GetKey(keyMapperSO.interact);
    }
    
    // Debug method to show current input state
    void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 200, 150));
        GUILayout.Label("Input State:");
        GUILayout.Label($"Can Move: {CanMove}");
        GUILayout.Label($"Can Rotate: {CanRotate}");
        GUILayout.Label($"Can Interact: {CanInteract}");
        GUILayout.EndArea();
    }
}

[CreateAssetMenu(fileName = "KeyMapper", menuName = "Input Key Mapper")]
public class KeyMapper : ScriptableObject
{
    [Header("Movement")]
    public KeyCode moveForward = KeyCode.W;
    public KeyCode moveBackward = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    
    [Header("Rotation")]
    public KeyCode turnLeft = KeyCode.Q;
    public KeyCode turnRight = KeyCode.E;
    
    [Header("Actions")]
    public KeyCode interact = KeyCode.Space;
    public KeyCode menu = KeyCode.Escape;
    public KeyCode inventory = KeyCode.I;
    
    [Header("Debug")]
    public KeyCode debugToggle = KeyCode.F1;
}