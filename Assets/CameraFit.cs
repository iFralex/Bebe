using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        camera.rect = new Rect(Vector2.zero, Vector2.one);
        float larghezza = 6.4f / (camera.aspect * 9);
        camera.rect = new Rect(new Vector2((1f - larghezza) / 2f, 0), new Vector2(larghezza, 1));
    }
}