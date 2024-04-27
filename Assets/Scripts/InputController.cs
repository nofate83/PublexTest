using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private AppControls _appControls;
   
    public Action<Vector2> GetPoint;
    void Awake()
    {
        _appControls = new AppControls();
    }

    private void Start()
    {
        _appControls.ActionMap.TouchClick.performed += GetScreenPoint;
    }

    private void OnEnable() => _appControls.Enable();
    private void OnDisable() => _appControls.Disable();

    private void GetScreenPoint(InputAction.CallbackContext context)
    {
        GetPoint?.Invoke(context.ReadValue<Vector2>());
    }
}
