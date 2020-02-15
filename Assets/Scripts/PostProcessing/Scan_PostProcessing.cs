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

    private float waveOpacity = 0;

    [SerializeField]
    private float opacityScalar = 1;


    //[ImageEffectOpaque]
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
        //Scan.SetFloat("_MidPoint", maxDistance/2);
    }

    private void Update()
    {
        // Repeatedly send out a scan wave.
        // Located in this method because of the use of Time.
        StartWave();

        // Always update the wave opacity.
        // The longer the scan has been sent out, the less opaque the whole scan becomes.
        Scan.SetFloat("_Opacity", waveOpacity);
    }

    // Send out a scan wave in pulses.
    // Uses Time to move away from the camera.
    private void StartWave()
    {
        // Timer used to send out pulses of scan waves.
        // If the timer is greater than the threshold (hardcoded to 1 in this case),
        // Then reset the following variables:
        if (pulseTimer >= 1)
        {
            // This restarts the scan wave entirely, clearing the screen of the effect.
            waveActive = true;
            // This resets the timer itself, to begin again.
            pulseTimer = 0;
        }
        // Increase the timer multiplied by a scalar.
        pulseTimer += Time.deltaTime * pulseDelay;
        // Decrease the opacity variable over time, 
        // scaled by the max distance so it's completely black before the next scan.
        if (waveOpacity >= 0)
            waveOpacity -= Time.deltaTime * opacityScalar;



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
            // This makes the wave opaque again, so it's brighter at the start of the scan.
            waveOpacity = 1;
        }
    }
}
