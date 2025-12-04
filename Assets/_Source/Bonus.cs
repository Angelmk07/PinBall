using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private int PointsAdd;

    private void OnCollisionEnter(Collision collision)
    {
        PointsHolder.Instance?.AddPoints(PointsAdd);
    }
}