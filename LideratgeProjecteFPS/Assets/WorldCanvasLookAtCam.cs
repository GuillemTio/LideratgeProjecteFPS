using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasLookAtCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
 
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
        this.transform.Rotate(0,180,0);
    }
}
