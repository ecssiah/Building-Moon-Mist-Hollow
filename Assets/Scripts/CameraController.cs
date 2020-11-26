﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 24f;
    public float zoomSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dx = panSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float dy = panSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Camera.main.transform.Translate(dx, dy, 0);

        float dz = zoomSpeed * Time.deltaTime * Input.GetAxis("Zoom");

        Camera.main.orthographicSize = Mathf.Clamp(
            Camera.main.orthographicSize + dz, 2f, 20f
        );
    }
}