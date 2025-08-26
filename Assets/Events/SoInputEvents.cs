using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoInputEvents", menuName = "Scriptable Objects/SoInputEvents")]
public class SoInputEvents : ScriptableObject
{
    public Action<Vector2> OnMove;
    public Action<float> OnRotate;

    public void RaiseMove(Vector2 input) => OnMove?.Invoke(input);
    public void RaiseRotate(float input) => OnRotate?.Invoke(input);
}
