using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Item : MonoBehaviour
{
    void Start() 
    {
        AssignItemTag();
    }

    void AssignItemTag()
    {
        float randomValue = Random.value;

        if (randomValue < 0.5f)
        {
            gameObject.tag = "GoodItem";
        }
        else
        {
            gameObject.tag = "BadItem";
        }
    }
}
