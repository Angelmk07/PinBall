using UnityEngine;

public class PointsHolder : MonoBehaviour
{
    public static PointsHolder Instance { get; private set; }

    private int countPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this); 
        }
        else
        {
            Instance = this; 
        }
    }

    public void AddPoints(int value)
    {
        if (value > 0)
        {
            countPoints += value;
        }
    }
}