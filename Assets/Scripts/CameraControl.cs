using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 sensitivity = new (200.0f, 200.0f);
    [SerializeField] private float verticalClampAngleTop = -45.0f;
    [SerializeField] private float verticalClampAngleBottom = 40.0f;
    [SerializeField] private Vector3 cameraDistance = new (0, 150f, -200.0f);
    [SerializeField] private Vector3 cameraAngle = new (30, 0, 0);
    [SerializeField] private float yaw = 0.0f; // Horizontal camera rotation (Y-axis)
    [SerializeField] private float pitch = 0.0f; // Vertical camera rotation (X-axis)

    PogoControls controls;
    private PlayerInputActions playerInputActions;
    private Vector2 lookInputVector;

    void Awake () {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Look.Enable();
    }

    void Start() {
        controls = target.GetComponentInChildren<PogoControls>();
        Vector3 initialRotation = transform.eulerAngles - target.eulerAngles;
        yaw = initialRotation.y;
        pitch = initialRotation.z;
    }

    void Update() {
        if (IsCameraControlEnabled()) {
            lookInputVector = playerInputActions.Player.Look.ReadValue<Vector2>();
            yaw += lookInputVector.x * sensitivity.x * PlayerPrefs.GetFloat("Sensitivity", 1.0f);
            pitch -= lookInputVector.y * sensitivity.y * PlayerPrefs.GetFloat("Sensitivity", 1.0f);
            pitch = Mathf.Clamp(pitch, verticalClampAngleTop, verticalClampAngleBottom);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
            transform.rotation = rotation * Quaternion.Euler(cameraAngle);
            Vector3 offset = rotation * cameraDistance;
            transform.position = target.position + offset;
            
            target.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }

    bool IsCameraControlEnabled() {
        return controls == null || controls.IsDead() == false;
    }
}
