using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Vector3 maxTensionPosition;
    [SerializeField] private float tensionDuration = 1f;
    [SerializeField] private float maxForceMultiplier = 10f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int onTouchAddPoints;

    private Vector3 _startPosition;
    private bool _isLaunched = false;
    private float _tensionStartTime;
    private bool _isCharging = false;
    

    private InputAction _launch;

    private void Awake()
    {
        _launch = playerInput.actions["Launch"];
    }
    private void Start()
    {
        _startPosition = transform.position;
    }
    private void OnEnable()
    {
        _launch.performed += Charge;
        _launch.canceled += LaunchBall;
    }

    private void OnDisable()
    {
        _launch.performed -= Charge;
        _launch.canceled -= LaunchBall;
    }

    private void Update()
    {
        if (!_isLaunched)
        {
            if (_isCharging)
            {
                float timeSinceStarted = Time.time - _tensionStartTime;
                float progress = Mathf.Clamp01(timeSinceStarted / tensionDuration);
                transform.position = Vector3.Lerp(_startPosition, maxTensionPosition, progress);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        PointsHolder.Instance.AddPoints(onTouchAddPoints);
    }
    private void Charge(InputAction.CallbackContext context)
    {
        _isCharging = true;
        _tensionStartTime = Time.time;
    }
    private void LaunchBall(InputAction.CallbackContext context)
    {
        _isCharging = false;
        _isLaunched = true;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        if (rb != null)
        {
            Vector3 forceDirection = (_startPosition- maxTensionPosition).normalized;
            float forceMagnitude = Vector3.Distance(_startPosition, gameObject.transform.position) * maxForceMultiplier;

            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
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