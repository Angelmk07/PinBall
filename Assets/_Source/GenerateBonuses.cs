using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBonuses : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private GameObject bonusMini;
    [SerializeField] private GameObject bonus;
    [SerializeField]GameObject[] points;
    [SerializeField]List<GameObject> listMiniBonuses = new List<GameObject>();
    List<GameObject> listBonuses = new List<GameObject>();
    List<int> choice = new List<int>();
    private void Start()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {

            listMiniBonuses.Add(Instantiate(bonusMini, points[i].transform));
        }
        for (int i = 0; i < points.Length; i++)
        {
            listBonuses.Add( Instantiate(bonus, points[i].transform));
        }
        HideAll();
        ShowNewBonuses();
    }
    protected void ShowNewBonuses()
    {
        for(int i = 0;i< count; i++)
        {
            choice.Add(Random.Range(0, points.Length-1));
            int num = Random.Range(0, 1);
            if (num == 0)
            {
                listBonuses[i].SetActive(true);
            }
            else
            {
                listMiniBonuses[i].SetActive(true);
            }

        }
    }
    protected void HideAll()
    {
        for( int i = 0; i < points.Length-1; i++)
        {
            listMiniBonuses[i].SetActive(false);
            listBonuses[i].SetActive(false);
        }
    }
}
