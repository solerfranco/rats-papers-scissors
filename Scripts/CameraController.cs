using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objetivo; // El objeto que la cámara debe seguir
    public float suavizado = 0.125f; // Qué tan suave es el seguimiento
    public Vector3 offset; // Posición relativa de la cámara

    void LateUpdate()
    {
        if (objetivo != null)
        {
            Vector3 posicionDeseada = objetivo.position + offset;
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
            transform.position = new Vector3(posicionSuavizada.x, posicionSuavizada.y, transform.position.z);
        }
    }
}
