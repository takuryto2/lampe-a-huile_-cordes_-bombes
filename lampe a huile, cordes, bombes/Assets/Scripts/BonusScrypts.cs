using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScrypts : MonoBehaviour
{
    [SerializeField] private GameGrid grid;
    [SerializeField] private GameObject BonusGameObj;

    void Start()
    {
        grid = GameGrid.instance;
        SpawnBonus();
    }

    private void Update()
    {
        
    }

    private void SpawnBonus()
    {
        
    }
}
