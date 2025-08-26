using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private SoInputEvents _soInputEvents;
    
    public void OnRotateLeft()
    {
        float input = -1f;
        _soInputEvents.RaiseRotate(input);
    }

    public void OnRotateRight()
    {
        float input = 1f;
        _soInputEvents.RaiseRotate(input);
    }

    public void OnWalkForward()
    {
        var input = new Vector2(0f, 1f);
        _soInputEvents.RaiseMove(input);
    }

    public void OnWalkBackwards()
    {
        var input = new Vector2(0f, -1f);
        _soInputEvents.RaiseMove(input);
    }

    public void OnWalkLeft()
    {
        var input = new Vector2(-1f, 0f);
        _soInputEvents.RaiseMove(input);
    }

    public void OnWalkRight()
    {
        var input = new Vector2(1f, 0f);
        _soInputEvents.RaiseMove(input);
    }
}
