using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_camera_manager : MonoBehaviour
{
    Transform playerTransform;

    public float smoothTime = 0.3f;
    [Tooltip("The vertical offset between the player camera and player transform")]
    public float verticalOffset;
    [Tooltip("The Horizontal offset between the player camera and player transform")]
    public float horizontalOffset;

    private Vector3 velocity = Vector3.zero;

    void OnEnable()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        // Define a target position above and behind the target transform 
        Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + verticalOffset, playerTransform.position.z + horizontalOffset);

        // Smoothly move the camera towards that target position 
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, smoothTime);
    }
}
