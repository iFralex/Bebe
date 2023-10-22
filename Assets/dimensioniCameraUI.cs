using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class dimensioniCameraUI : MonoBehaviour
{
    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        if (cam.aspect < 1.77778f)
        {
            float alt = cam.aspect / 1.77778f;
            cam.rect = new Rect(cam.rect.x, (1f - alt) / 2f, cam.rect.width, alt);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
