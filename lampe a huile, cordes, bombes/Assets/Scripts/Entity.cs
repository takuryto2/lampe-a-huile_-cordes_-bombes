using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Entity instance;

    private void Awake()
    {
        instance = this;
    }
}
