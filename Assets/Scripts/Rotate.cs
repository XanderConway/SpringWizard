using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField, Min(0f)] float mouseSensitivity = 100f, maxField = 70;

    Transform playerBody;
    float xRotation = 0;

    void Start () {
        playerBody = transform.parent.transform;
    }
    // Update is called once per frame
    void Update()
    {
        float scale = Time.deltaTime * mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * scale;
        float mouseY = Input.GetAxis("Mouse Y") * scale;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxField, maxField);
        transform.localEulerAngles = Vector3.right * xRotation;
        playerBody.Rotate(Vector3.up * mouseX);
        
    }
}
