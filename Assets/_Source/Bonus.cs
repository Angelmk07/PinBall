using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private int pointsValue = 10;
    [SerializeField] private LayerMask playerLayer;

    private bool _isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;
        if ((playerLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            CollectBonus();
        }
    }
    private void CollectBonus()
    {
        _isCollected = true;
        if (PointsHolder.Instance != null)
        {
            PointsHolder.Instance.AddPoints(pointsValue);
        }
        GenerateBonuses generator = GetComponentInParent<GenerateBonuses>();
        if (generator != null)
        {
            generator.HideBonus(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}