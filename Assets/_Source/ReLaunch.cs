using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class ReLaunch : GenerateBonuses
{
    [SerializeField] private Vector3 maxTensionPosition;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float tensionDuration = 1f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private PlayerInput playerInput;

    private float _tensionStartTime;
    private BoxCollider m_BoxCollider;
    private InputAction _reload;
   [SerializeField] private bool isTargetInTrigger = false; 

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = true;
        _reload = playerInput.actions["Reload"];
    }

    private void OnEnable()
    {
        _reload.performed += ReturnBall;
    }

    private void OnDisable()
    {
        _reload.performed -= ReturnBall; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            isTargetInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            isTargetInTrigger = false;
        }
    }

    private void ReturnBall(InputAction.CallbackContext context)
    {
        if (!isTargetInTrigger) return;
        HideAll();
        ShowNewBonuses();
        float timeSinceStarted = Time.time - _tensionStartTime;
        float progress = Mathf.Clamp01(timeSinceStarted / tensionDuration);
        transform.position = Vector3.Lerp(_startPosition, maxTensionPosition, progress);

        isTargetInTrigger = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(maxTensionPosition, 0.1f);

        if (Application.isPlaying)
        {
            Gizmos.DrawLine(_startPosition, maxTensionPosition);
        }
        else
        {
            Gizmos.DrawLine(transform.position, maxTensionPosition);
        }
    }
}