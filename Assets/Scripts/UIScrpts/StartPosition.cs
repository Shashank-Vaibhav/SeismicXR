using UnityEngine;

public class StartPosition : MonoBehaviour
{
    [Header("UI Placement Settings")]
    [SerializeField] private Transform followedObjectTransform; 
    [SerializeField] private Vector3 offsetFromCamera = new Vector3(0, 0, 0.5f); 

    private void Start()
    {
        InitializeFollowedObjectTransform();
        PositionUIInFrontOfCamera();
    }

    /// <summary>
    /// Initializes the followedObjectTransform. Defaults to the main camera if not assigned.
    /// </summary>
    private void InitializeFollowedObjectTransform()
    {
        if (followedObjectTransform == null)
        {
            followedObjectTransform = Camera.main != null
                ? Camera.main.transform
                : throw new MissingReferenceException("No Main Camera found, and 'followedObjectTransform' is not assigned.");
        }
    }

    /// <summary>
    /// Places the UI in front of the followed object's position with the specified offset.
    /// </summary>
    private void PositionUIInFrontOfCamera()
    {
        // Calculate the world position for the UI using the offset
        Vector3 worldOffset = followedObjectTransform.forward * offsetFromCamera.z
                            + followedObjectTransform.right * offsetFromCamera.x
                            + followedObjectTransform.up * offsetFromCamera.y;

        transform.position = followedObjectTransform.position + worldOffset;

        // Orient the UI to face the user
        transform.LookAt(followedObjectTransform.position);
        transform.Rotate(0, 180f, 0); // Flip 180° to ensure correct facing direction
    }
}
