using UnityEngine;
using UnityEngine.Events;

public class PointsHolder : MonoBehaviour
{
    public static PointsHolder Instance { get; private set; }

    public UnityEvent<int> OnPointsChanged;

    private int _currentPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddPoints(int points)
    {
        if (points <= 0) return;

        _currentPoints += points;
        OnPointsChanged?.Invoke(_currentPoints);
    }

    public int GetCurrentPoints() => _currentPoints;

    public void ResetPoints()
    {
        _currentPoints = 0;
        OnPointsChanged?.Invoke(_currentPoints);
    }
}