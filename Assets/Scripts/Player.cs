using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float gridSize = 2f;
    
    [Header("Collision Detection")]
    public LayerMask collisionMask = -1;
    public float raycastDistance => gridSize - 0.1f; // Slightly less than gridSize
    public float raycastHeight = 0f;
    
    [Header("Debug")]
    public bool showDebugRays = true;
    
    // Input cooldown to prevent spam
    private float inputCooldown = 0.1f;
    private float lastInputTime = 0f;
    
    void Start()
    {
        GridSnapper.SnapToGrid(transform, gridSize);
        
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMoveForward += () => TryMove(Vector3.forward);
            InputManager.Instance.OnMoveBackward += () => TryMove(Vector3.back);
            InputManager.Instance.OnMoveLeft += () => TryMove(Vector3.left);
            InputManager.Instance.OnMoveRight += () => TryMove(Vector3.right);
            InputManager.Instance.OnTurnLeft += () => Rotate(-90f);
            InputManager.Instance.OnTurnRight += () => Rotate(90f);
        }
    }
    
    void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMoveForward -= () => TryMove(Vector3.forward);
            InputManager.Instance.OnMoveBackward -= () => TryMove(Vector3.back);
            InputManager.Instance.OnMoveLeft -= () => TryMove(Vector3.left);
            InputManager.Instance.OnMoveRight -= () => TryMove(Vector3.right);
            InputManager.Instance.OnTurnLeft -= () => Rotate(-90f);
            InputManager.Instance.OnTurnRight -= () => Rotate(90f);
        }
    }
    
    void Update()
    {
        if (showDebugRays)
        {
            DrawDebugRays();
        }
    }
    
    void TryMove(Vector3 localDirection)
    {
        // Convert local direction to world direction
        Vector3 worldDirection = transform.TransformDirection(localDirection);
        Vector3 newPosition = transform.position + (worldDirection * gridSize);
        
        // Check for collision using raycast
        if (CanMoveTo(worldDirection))
        {
            transform.position = newPosition;
        }
        else
        {
            Debug.Log("Movement blocked!");
        }
    }
    
    bool CanMoveTo(Vector3 direction)
    {
        // Cast ray from player position + height offset
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeight;
        
        // Cast ray to check for obstacles
        if (Physics.Raycast(rayOrigin, direction, raycastDistance, collisionMask))
        {
            return false;
        }
        
        // Optional: Ground check to prevent walking off ledges
        Vector3 groundCheckPos = transform.position + (direction * gridSize);
        if (!Physics.Raycast(groundCheckPos + Vector3.up, Vector3.down, 2f, collisionMask))
        {
            // Will stop you for proceeding if there's no ground below you
            // return false;
        }
        
        return true;
    }
    
    void Rotate(float angle)
    {
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.y += angle;
        transform.eulerAngles = currentEuler;
    }
    
    void DrawDebugRays()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeight;
        
        // Draw rays in all four cardinal directions
        Vector3[] directions = {
            transform.TransformDirection(Vector3.forward),
            transform.TransformDirection(Vector3.back),
            transform.TransformDirection(Vector3.left),
            transform.TransformDirection(Vector3.right)
        };
        
        foreach (Vector3 worldDir in directions)
        {
            Color rayColor = CanMoveTo(worldDir) ? Color.green : Color.red;
            Debug.DrawRay(rayOrigin, worldDir * raycastDistance, rayColor);
        }
    }
    
    public Vector3 GetGridPosition()
    {
        return GridSnapper.GetGridPosition(transform, gridSize);
    }
    
    public Vector3 GetFacingDirection()
    {
        return GridSnapper.GetFacingDirection(transform);
    }
    
    // Force set position on grid 
    public void SetGridPosition(int x, int z)
    {
        GridSnapper.SetGridPosition(transform, x, z, gridSize);
    }
    
    // Force set rotation to cardinal direction (0, 90, 180, 270)
    public void SetCardinalRotation(int cardinalDirection)
    {
        GridSnapper.SetCardinalRotation(transform, cardinalDirection);
    }
}