using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float timer;
    private int radiusX;
    private int radiusY;
    private GameGrid grid;
    [SerializeField] private float tickBoom;
    void Start()
    {
        timer = 0f;
    }


    void Update()
    {
        timer += Time.deltaTime * tickBoom;
        timer = Mathf.Clamp01(timer);
        if (timer == tickBoom)
        {
            BOOM();
        }
    }

    private void BOOM()
    {
        Destroy(this.gameObject);
    }

    public void SetBomb(int radius, GameGrid Gamegrid)
    {
        radiusX = radius;
        radiusY = radius;
        grid = Gamegrid;
    }
}
