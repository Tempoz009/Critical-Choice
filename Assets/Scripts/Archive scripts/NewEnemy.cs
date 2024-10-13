using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewEnemy : Entity
{
    [SerializeField] private Text _enemyText;

    protected override void Update()
    {
        base.Update();
        _enemyText.text =$"{_name} : {currentHP}"; 
    }
}
