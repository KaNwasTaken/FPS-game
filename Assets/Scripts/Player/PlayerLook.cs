using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    public float ySensitivity = -30f;
    public float xSensitivity = -30f;
    float xRotation = 0f;

    public void ProcessLook(Vector2 input)
    {
        float mouseY = input.y;
        float mouseX = input.x;

        xRotation += (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(new Vector3(0, mouseX * Time.deltaTime * -xSensitivity, 0));
    }
}
