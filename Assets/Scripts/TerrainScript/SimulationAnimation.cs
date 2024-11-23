using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationAnimation : MonoBehaviour
{
     // Array to hold the mesh frames
    public Mesh[] frameMeshes;

    // Duration for each frame (in seconds)
    public float frameDuration = 0.1f;

    // References to the Mesh Filter and Mesh Collider components
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    // Current frame index
    private int currentFrame = 0;

    void Start()
    {
        // Get the Mesh Filter and Mesh Collider components
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        // Start the frame-by-frame animation
        StartCoroutine(PlayAnimation());
    }

    // Coroutine to play the animation
    IEnumerator PlayAnimation()
    {
        while (true)
        {
            // Set the current mesh in the Mesh Filter
            meshFilter.mesh = frameMeshes[currentFrame];

            // Update the Mesh Collider to match the new mesh
            meshCollider.sharedMesh = frameMeshes[currentFrame];

            // Wait for the frame duration
            yield return new WaitForSeconds(frameDuration);

            // Move to the next frame (loop back to 0 if at the end)
            currentFrame = (currentFrame + 1) % frameMeshes.Length;
        }
    }
}
