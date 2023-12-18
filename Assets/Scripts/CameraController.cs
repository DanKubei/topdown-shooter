using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class CameraController : MonoBehaviour
{
    public RaycastHitEvent OnView;
    public LayerMask ViewTargetMask;

    public Vector3 cameraOffset;
    public Transform target;

    private Camera _selfCamera;
    private Resolution _currentResolution;
    private float _rotationCof;

    void Start()
    {
        _currentResolution = Screen.currentResolution;
        _selfCamera = GetComponent<Camera>();
        _rotationCof = Mathf.Sin(_selfCamera.fieldOfView / 2 * Mathf.PI / 180);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + cameraOffset;
        DoRaycast();
    }

    private void DoRaycast()
    {
        Vector3 mousePos = Input.mousePosition;
        RaycastHit hit;
        Vector3 raycastDirection = new Vector3((Mathf.Clamp(mousePos.x, 0, _currentResolution.width) / _currentResolution.width - 0.5f) * 2f * _rotationCof,
            (Mathf.Clamp(mousePos.y, 0, _currentResolution.height) / _currentResolution.height - 0.5f) * 2f * _rotationCof,
            1);
        Physics.Raycast(transform.position, transform.TransformDirection(raycastDirection), out hit, Mathf.Infinity, ViewTargetMask);
        if (!hit.Equals(null))
        {
            OnView?.Invoke(hit);
        }
    }
}
