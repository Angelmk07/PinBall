using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperController : MonoBehaviour
{
    [SerializeField] private GameObject FlipperL;
    [SerializeField] private GameObject FlipperR;
    [SerializeField] private float rotate = 30f;
    [SerializeField] private float lerpt = 5;
    private PlayerInput _playerInput;

    private InputAction _leftFlipperAction;
    private InputAction _rightFlipperAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _leftFlipperAction = _playerInput.actions["Flipper_L"];
        _rightFlipperAction = _playerInput.actions["Flipper_R"];
    }

    private void OnEnable()
    {
        _leftFlipperAction.performed += OnLeftFlipper;
        _rightFlipperAction.performed += OnRightFlipper;
        _leftFlipperAction.canceled += OffLeftFlipper;
        _rightFlipperAction.canceled += OffRightFlipper;
    }

    private void OnDisable()
    {
        _leftFlipperAction.performed -= OnLeftFlipper;
        _leftFlipperAction.canceled -= OffLeftFlipper;
        _rightFlipperAction.performed -= OnRightFlipper;
        _rightFlipperAction.canceled -= OffRightFlipper;
    }

    private void OnRightFlipper(InputAction.CallbackContext context)
    {
        FlipperR.transform.Rotate( new Vector3(0,0, rotate));
    }

    private void OnLeftFlipper(InputAction.CallbackContext context)
    {
        FlipperL.transform.Rotate(new Vector3(0, 0, -rotate));
    }

    private void OffRightFlipper(InputAction.CallbackContext context)
    {
        FlipperR.transform.Rotate(new Vector3(0, 0, -rotate));
    }

    private void OffLeftFlipper(InputAction.CallbackContext context)
    {
        FlipperL.transform.Rotate(new Vector3(0, 0, rotate));
    }

}


