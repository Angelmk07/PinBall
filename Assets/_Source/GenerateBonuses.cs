using System.Collections.Generic;
using UnityEngine;

public class GenerateBonuses : MonoBehaviour
{
    [System.Serializable]
    public class BonusData
    {
        public GameObject prefab;
        public int spawnWeight = 1;
        public int pointsValue;
    }

    [Header("Bonus Settings")]
    [SerializeField] private int bonusesToSpawn = 3;
    [SerializeField] private float minDistanceBetweenBonuses = 2f;

    [Header("Bonus Types")]
    [SerializeField] private BonusData smallBonus;
    [SerializeField] private BonusData largeBonus;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private List<GameObject> _activeBonuses = new List<GameObject>();
    private List<int> _usedIndices = new List<int>();

    private void Start()
    {
        GenerateNewBonuses();
    }

    public void GenerateNewBonuses()
    {
        HideAllBonuses();
        _usedIndices.Clear();
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return;
        }

        int bonusesSpawned = 0;
        int attempts = 0;
        int maxAttempts = spawnPoints.Length * 2;

        while (bonusesSpawned < bonusesToSpawn && attempts < maxAttempts)
        {
            int randomIndex = GetRandomSpawnIndex();

            if (randomIndex != -1 && IsPositionValid(spawnPoints[randomIndex].position))
            {
                SpawnBonusAtPoint(spawnPoints[randomIndex]);
                _usedIndices.Add(randomIndex);
                bonusesSpawned++;
            }

            attempts++;
        }
    }

    private int GetRandomSpawnIndex()
    {
        List<int> availableIndices = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!_usedIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        if (availableIndices.Count == 0) return -1;

        return availableIndices[Random.Range(0, availableIndices.Count)];
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (var index in _usedIndices)
        {
            if (Vector3.Distance(position, spawnPoints[index].position) < minDistanceBetweenBonuses)
            {
                return false;
            }
        }
        return true;
    }

    private void SpawnBonusAtPoint(Transform spawnPoint)
    {
        BonusData selectedBonus = SelectRandomBonus();

        if (selectedBonus.prefab != null)
        {
            GameObject bonus = Instantiate(selectedBonus.prefab, spawnPoint.position,
                                          Quaternion.identity, spawnPoint);

            Bonus bonusComponent = bonus.GetComponent<Bonus>();
            if (bonusComponent == null)
            {
                bonusComponent = bonus.AddComponent<Bonus>();
            }

            _activeBonuses.Add(bonus);
        }
    }

    private BonusData SelectRandomBonus()
    {
        int totalWeight = smallBonus.spawnWeight + largeBonus.spawnWeight;
        int randomValue = Random.Range(0, totalWeight);

        return randomValue < smallBonus.spawnWeight ? smallBonus : largeBonus;
    }

    public void HideAllBonuses()
    {
        foreach (var bonus in _activeBonuses)
        {
            if (bonus != null)
            {
                Destroy(bonus);
            }
        }
        _activeBonuses.Clear();
    }

    public void HideBonus(GameObject bonus)
    {
        if (_activeBonuses.Contains(bonus))
        {
            _activeBonuses.Remove(bonus);
            Destroy(bonus);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (var point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.3f);
                }
            }
        }
    }
}