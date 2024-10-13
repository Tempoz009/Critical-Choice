using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : Entity
{
    [SerializeField] private Text _playerText;

    protected override void Update()
    {
        base.Update();
        _playerText.text =$"{_name} : {currentHP}"; 
    }
}
