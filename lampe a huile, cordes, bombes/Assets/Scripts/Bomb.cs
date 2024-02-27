using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float timer;
    private int radiusX;
    private int radiusY;
    [SerializeField] private float tickBoom;
    void Start()
    {
        timer = 0f;
    }


    void Update()
    {
        timer += Time.deltaTime * tickBoom;
        timer = Mathf.Clamp01(timer);
        if (timer == 1f)
        {
            BOOM();
        }
    }

    private void BOOM()
    {
        Destroy(this.gameObject);
    }

    public void SetRadius(int value)
    {
        radiusX = value;
        radiusY = value;
    }
}
