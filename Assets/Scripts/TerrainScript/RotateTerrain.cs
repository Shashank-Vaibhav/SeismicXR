using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateTerrain : MonoBehaviour
{
    [SerializeField] private InputActionReference rotateInputButton; // Input reference
    [SerializeField] private float rotationSpeed = 5f; // Rotation speed
    [SerializeField] private float moveSpeed = 5f; // Movement speed
    [SerializeField] private TextMeshProUGUI debugText;

    private Vector2 inputVector; // To store input values

    private void OnEnable()
    {
        if (rotateInputButton != null && rotateInputButton.action != null)
        {
            rotateInputButton.action.performed += OnRotateInput;
            rotateInputButton.action.canceled += OnRotateInputCanceled;
        }
        else if (debugText != null)
        {
            debugText.text = "InputActionReference is not set or is invalid.";
        }
    }

    private void OnDisable()
    {
        if (rotateInputButton != null && rotateInputButton.action != null)
        {
            rotateInputButton.action.performed -= OnRotateInput;
            rotateInputButton.action.canceled -= OnRotateInputCanceled;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            // Rotate the object along the Y-axis
            transform.Rotate(Vector3.up, inputVector.x * rotationSpeed * Time.deltaTime, Space.World);

            if (debugText != null)
            {
                debugText.text = $"Rotating with input: {inputVector.x}";
            }
        }
        else if (Mathf.Abs(inputVector.y) > Mathf.Abs(inputVector.x))
        {
            // Move the object along the Y-axis
            transform.Translate(Vector3.up * inputVector.y * moveSpeed * Time.deltaTime, Space.World);

            if (debugText != null)
            {
                debugText.text = $"Moving with input: {inputVector.y}";
            }
        }
    }

    private void OnRotateInput(InputAction.CallbackContext context)
    {
        // Update the input vector when input is performed
        inputVector = context.ReadValue<Vector2>();

        if (debugText != null)
        {
            debugText.text = $"Input received: {inputVector}";
        }
    }

    private void OnRotateInputCanceled(InputAction.CallbackContext context)
    {
        // Reset the input vector when input is canceled
        inputVector = Vector2.zero;

        if (debugText != null)
        {
            debugText.text = "Input canceled, vector reset to zero.";
        }
    }
}
