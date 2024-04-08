using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 _offset;
    private Transform target;
    [SerializeField] private float smoothTime;
    [SerializeField] private float smoothTimeZoom;
    [SerializeField] private Transform initialPosition;
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float zoomInFOV;
    [SerializeField] private float zoomOutFOV;
    [SerializeField] private float leftThreshold;
    [SerializeField] private Camera cameraControl; // X position threshold for static camera
    private Vector3 targetPosition;

    private void Start()
    {
        target = Player.Instance.transform;
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // Calculate target position based on player position
        float currentFOV = cameraControl.fieldOfView;
        float targetFOV;

        if (target.position.x < leftThreshold) // Player is right of threshold
        {
            targetPosition = target.position + _offset;
            targetFOV = zoomInFOV;
        }
        else // Player is left of threshold
        {
            // Use a static position for left side
            targetPosition = initialPosition.position;
            targetFOV = zoomOutFOV;
        }

        // Smoothly follow target
        cameraControl.fieldOfView = Mathf.Lerp(currentFOV, targetFOV, smoothTimeZoom * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}
