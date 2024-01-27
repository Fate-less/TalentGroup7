using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PPCamManager : MonoBehaviour
{
    [SerializeField] float MinWidth, MinHeight;
    [SerializeField] PixelPerfectCamera ppCam; //enable crop x and y for it to work

    int width{get => (int)(MinWidth * 2f * ppCam.assetsPPU);}
    int height{get => (int)(MinHeight * 2f * ppCam.assetsPPU);}
    
    Vector2 resolution;
    void Update()
    {
        Vector2 newRes = new Vector2(Screen.width, Screen.height);
        if(resolution != newRes)
        {
            resolution = newRes;
            UpdateCam();
        }
    }

    void UpdateCam()
    {
        int yScale = Mathf.Max((int)(resolution.y / (height)), 1);
        int xScale = Mathf.Max((int)(resolution.x / (width)), 1);
        int pixelScale = Mathf.Min(xScale, yScale);

        ppCam.refResolutionY = (int)resolution.y / pixelScale;
        ppCam.refResolutionY -= ppCam.refResolutionY % 2;

        ppCam.refResolutionX = (int)resolution.x / pixelScale;
        ppCam.refResolutionX -= ppCam.refResolutionX % 2;
    }
}
