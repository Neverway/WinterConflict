using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera[] camera;

    public Material[] cameraMat;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < camera.Length; i++)
        {
            if (camera[i].targetTexture != null)
            {
                camera[i].targetTexture.Release();
            }
        
            camera[i].targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            cameraMat[i].mainTexture = camera[i].targetTexture;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
