using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private SoInputEvents _soInputEvents;

    [SerializeField] private float _inputCooldown = 1f;
    private float _timer = 0f;
    private void OnEnable()
    {
        _soInputEvents.OnMove += Move;
        _soInputEvents.OnRotate += Rotate;
    }
    

    private void OnDisable()
    {
        _soInputEvents.OnMove -= Move;
        _soInputEvents.OnRotate -= Rotate;
        
    }

    private void Update()
    {
        if(_timer > 0) _timer -= Time.deltaTime;
    }

    private void Rotate(float input)
    {
        if(_timer > 0) return;
        _timer = _inputCooldown;
        
        switch (input)
        {
            case > 0:
                transform.rotation *= Quaternion.Euler(0, 90f, 0);
                break;
            case < 0:
                transform.rotation *= Quaternion.Euler(0, -90f, 0);
                break;
        }
    }

    private void Move(Vector2 input)
    {
        if(_timer > 0) return;
        _timer = _inputCooldown;
        
        var moveToNextPoint = 1f;
        var distance = 1.1f;
        RaycastHit hit;
        
        switch (input.x)
        {
            case > 0:
                if (Physics.Raycast(transform.position, transform.right, out hit, distance))
                    return;
                transform.position += transform.right * moveToNextPoint;
                break;
            case < 0:
                if (Physics.Raycast(transform.position, -transform.right, out hit, distance))
                    return;
                transform.position += transform.right * -moveToNextPoint;
                break;
        }

        switch (input.y)
        {
            case > 0:
                if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
                    return;
                transform.position += transform.forward * moveToNextPoint;
                break;
            case < 0:
                if (Physics.Raycast(transform.position, -transform.forward, out hit, distance))
                    return;
                transform.position += transform.forward * -moveToNextPoint;
                break;
        }
    }
}