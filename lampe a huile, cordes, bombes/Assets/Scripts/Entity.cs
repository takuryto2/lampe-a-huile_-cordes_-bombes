using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public static Entity instance;

    private void Awake()
    {
        instance = this;
    }
}
