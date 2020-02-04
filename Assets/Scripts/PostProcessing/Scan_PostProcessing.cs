using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Scan_PostProcessing : MonoBehaviour
{

    // This is for the Depth Scan done via Post Processing
    [SerializeField]
    private Material Scan = null;

    [SerializeField]
    private float waveSpeed = 0;

    [SerializeField]
    private bool waveActive = false;

    [SerializeField]
    private float maxDistance = 20;

    [SerializeField]
    private float startDistance = 0;

    [SerializeField]
    private float pulseDelay = 0;
    
    private float pulseTimer = 0;

    private float waveDistance = 0;


    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Scan.SetFloat("_WaveDistance", waveDistance);
        Graphics.Blit(src, dst, Scan);
    }

    private void Start()
    {
        // Set the camera's depth mode.
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;

        // Set the _MidPoint variable in "sh_Scan"
        Scan.SetFloat("_MidPoint", maxDistance/2);
    }

    private void Update()
    {
        // Repeatedly send out a scan wave.
        // Located in this method because of the use of Time.
        StartWave();
    }

    // Send out a scan wave in pulses.
    // Uses Time to move away from the camera.
    private void StartWave()
    {
        // Timer used to send out pulses of scan waves.
        if (pulseTimer >= 1)
        {
            waveActive = true;
            pulseTimer = 0;
        }
        pulseTimer += Time.deltaTime * pulseDelay;


        // Send the Wave
        if (waveActive){
            // Set the wave to the starting distance at the beginning of the scan
            if (waveDistance < startDistance)
                waveDistance = startDistance;
            // Move the wave down camera depth
            waveDistance = waveDistance + waveSpeed * Time.deltaTime;
            // Once the wave reaches the maximum depth, stop the scan
            if (waveDistance >= maxDistance)
                waveActive = false;
        } else {
            // If not scanning, then keep the scan line at the camera
            waveDistance = 0;
        }
    }
}
