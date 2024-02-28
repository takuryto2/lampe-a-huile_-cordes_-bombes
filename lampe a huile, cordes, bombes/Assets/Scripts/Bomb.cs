using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private int radiusX;
    private int radiusY;
    private PlayerMovement player;
    public List<Cell> explodingCells;
    private Cell cellOn;
    [SerializeField] private float tickBoom;

    private void Start()
    {
        SetBomb(player.radius, cellOn);
        explodingCells = player.grid.GetNeighbors(cellOn);
        Debug.Log(explodingCells.Count);
    }
    void Update()
    {
        tickBoom -= Time.deltaTime;
        if (tickBoom <= 0f)
        {
            BOOM();
        }
    }

    private void BOOM()
    {
        for (int i = 0; i < explodingCells.Count; i++)
        {
            Debug.Log("caca");
            player.grid.GetCell(explodingCells[i].gridPos.Item1, explodingCells[i].gridPos.Item2).ExplodeCell();
        }
        Destroy(this.gameObject);
    }

    public void SetBomb(int radius, Cell cell)
    {
        radiusX = radius;
        radiusY = radius;
        cellOn = cell;
    }
}
