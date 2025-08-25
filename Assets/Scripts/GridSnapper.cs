using UnityEngine;

public static class GridSnapper
{
    /// <summary>
    /// Snaps a transform to the nearest grid position
    /// </summary>
    /// <param name="transform">Transform to snap</param>
    /// <param name="gridSize">Size of each grid cell</param>
    /// <param name="snapRotation">Whether to also snap rotation to 90-degree increments</param>
    public static void SnapToGrid(Transform transform, float gridSize, bool snapRotation = true)
    {
        // Snap position to grid
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.z = Mathf.Round(pos.z / gridSize) * gridSize;
        transform.position = pos;
        
        if (snapRotation)
        {
            SnapRotationToCardinal(transform);
        }
    }
    
    /// <summary>
    /// Snaps rotation to nearest 90-degree increment
    /// </summary>
    /// <param name="transform">Transform to snap</param>
    public static void SnapRotationToCardinal(Transform transform)
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = Mathf.Round(euler.y / 90f) * 90f;
        transform.eulerAngles = euler;
    }
    
    /// <summary>
    /// Gets the grid position of a transform
    /// </summary>
    /// <param name="transform">Transform to get grid position from</param>
    /// <param name="gridSize">Size of each grid cell</param>
    /// <returns>Grid coordinates as Vector3 (y is always 0)</returns>
    public static Vector3 GetGridPosition(Transform transform, float gridSize)
    {
        return new Vector3(
            Mathf.Round(transform.position.x / gridSize),
            0,
            Mathf.Round(transform.position.z / gridSize)
        );
    }
    
    /// <summary>
    /// Gets the grid position from a world position
    /// </summary>
    /// <param name="worldPosition">World position to convert</param>
    /// <param name="gridSize">Size of each grid cell</param>
    /// <returns>Grid coordinates as Vector3 (y is always 0)</returns>
    public static Vector3 WorldToGrid(Vector3 worldPosition, float gridSize)
    {
        return new Vector3(
            Mathf.Round(worldPosition.x / gridSize),
            0,
            Mathf.Round(worldPosition.z / gridSize)
        );
    }
    
    /// <summary>
    /// Converts grid coordinates to world position
    /// </summary>
    /// <param name="gridPosition">Grid coordinates</param>
    /// <param name="gridSize">Size of each grid cell</param>
    /// <param name="yPosition">Y position in world space (default 0)</param>
    /// <returns>World position</returns>
    public static Vector3 GridToWorld(Vector3 gridPosition, float gridSize, float yPosition = 0)
    {
        return new Vector3(
            gridPosition.x * gridSize,
            yPosition,
            gridPosition.z * gridSize
        );
    }
    
    /// <summary>
    /// Sets a transform to a specific grid position
    /// </summary>
    /// <param name="transform">Transform to move</param>
    /// <param name="gridX">Grid X coordinate</param>
    /// <param name="gridZ">Grid Z coordinate</param>
    /// <param name="gridSize">Size of each grid cell</param>
    public static void SetGridPosition(Transform transform, int gridX, int gridZ, float gridSize)
    {
        Vector3 newPos = new Vector3(
            gridX * gridSize, 
            transform.position.y, 
            gridZ * gridSize
        );
        transform.position = newPos;
    }
    
    /// <summary>
    /// Sets rotation to a cardinal direction (0, 90, 180, 270 degrees)
    /// </summary>
    /// <param name="transform">Transform to rotate</param>
    /// <param name="cardinalDirection">Direction (0=North, 1=East, 2=South, 3=West)</param>
    public static void SetCardinalRotation(Transform transform, int cardinalDirection)
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = cardinalDirection * 90f;
        transform.eulerAngles = euler;
    }
    
    /// <summary>
    /// Gets the current cardinal direction the transform is facing
    /// </summary>
    /// <param name="transform">Transform to check</param>
    /// <returns>Cardinal direction (0=North, 1=East, 2=South, 3=West)</returns>
    public static int GetCardinalDirection(Transform transform)
    {
        float yRotation = transform.eulerAngles.y;
        return Mathf.RoundToInt(yRotation / 90f) % 4;
    }
    
    /// <summary>
    /// Gets the facing direction as a grid vector
    /// </summary>
    /// <param name="transform">Transform to check</param>
    /// <returns>Direction vector on the grid (like Vector3.forward, Vector3.right, etc.)</returns>
    public static Vector3 GetFacingDirection(Transform transform)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        
        // Round to nearest cardinal direction
        if (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
        {
            return new Vector3(Mathf.Sign(forward.x), 0, 0);
        }
        else
        {
            return new Vector3(0, 0, Mathf.Sign(forward.z));
        }
    }
}