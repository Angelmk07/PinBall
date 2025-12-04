using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class ReLaunch : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private Vector3 maxTensionPosition;
    [SerializeField] private float tensionDuration = 1f;
    [SerializeField] private LayerMask targetLayer;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GenerateBonuses bonusGenerator;

    private Vector3 _startPosition;
    private float _tensionStartTime;
    private BoxCollider _boxCollider;
    private InputAction _reloadAction;
    private bool _isTargetInTrigger = false;
    private bool _isTensioning = false;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _startPosition = transform.position;

        if (playerInput != null)
        {
            _reloadAction = playerInput.actions["Reload"];
        }
        else
        {
            return;
        }
    }

    private void OnEnable()
    {
        if (_reloadAction != null)
        {
            _reloadAction.performed += StartReturnBall;
        }
    }

    private void OnDisable()
    {
        if (_reloadAction != null)
        {
            _reloadAction.performed -= StartReturnBall;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInTargetLayer(other.gameObject))
        {
            _isTargetInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInTargetLayer(other.gameObject))
        {
            _isTargetInTrigger = false;
        }
    }

    private void Update()
    {
        if (_isTensioning)
        {
            UpdateTension();
        }
    }

    private void StartReturnBall(InputAction.CallbackContext context)
    {
        if (!_isTargetInTrigger || _isTensioning) return;
        if (bonusGenerator != null)
        {
            bonusGenerator.HideAllBonuses();
        }
        _isTensioning = true;
        _tensionStartTime = Time.time;
    }

    private void UpdateTension()
    {
        float elapsedTime = Time.time - _tensionStartTime;
        float progress = Mathf.Clamp01(elapsedTime / tensionDuration);

        transform.position = Vector3.Lerp(_startPosition, maxTensionPosition, progress);

        if (progress >= 1f)
        {
            CompleteReturn();
        }
    }

    private void CompleteReturn()
    {
        _isTensioning = false;
        _isTargetInTrigger = false;
        if (bonusGenerator != null)
        {
            bonusGenerator.GenerateNewBonuses();
        }
        transform.position = _startPosition;
    }

    private bool IsInTargetLayer(GameObject obj)
    {
        return (targetLayer.value & (1 << obj.layer)) != 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(maxTensionPosition, 0.2f);

        Gizmos.color = Color.yellow;
        Vector3 startPos = Application.isPlaying ? _startPosition : transform.position;
        Gizmos.DrawLine(startPos, maxTensionPosition);
        Gizmos.DrawSphere(startPos, 0.15f);
    }
}