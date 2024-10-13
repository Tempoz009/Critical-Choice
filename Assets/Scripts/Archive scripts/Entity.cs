using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int maxHP;
    protected int currentHP;

    protected string _name;

    protected void Awake()
    {
        _name = gameObject.name;
        maxHP = 10;
        currentHP = maxHP;
    }

    protected virtual void Update()
    {
        if (currentHP > maxHP) 
        { 
            currentHP = maxHP;
        }
    }
    public void ChangeHealth(bool isIncrease, int hpCount)
    {
        currentHP += isIncrease ? hpCount : -hpCount;

        if (currentHP < 0)
        {
            foreach (GameObject obj1 in GameObject.FindGameObjectsWithTag("GoodItem"))
            {
                GameObject.Destroy(obj1);
            }

            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("BadItem"))
            {
                GameObject.Destroy(obj2);
            }

            Destroy(gameObject);
        }
    }

}
