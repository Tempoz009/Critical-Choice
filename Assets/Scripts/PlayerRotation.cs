using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public Transform playerBody;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float maxXRotation = 20f;
    private float maxYRotation = 15f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Поворот вокруг оси X (вертикальный поворот)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxXRotation, maxXRotation);

        // Поворот вокруг оси Y (горизонтальный поворот)
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -maxYRotation, maxYRotation);

        // Применяем повороты к камере и телу игрока
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Вращение камеры
        playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f); // Вращение тела игрока
    }
}
