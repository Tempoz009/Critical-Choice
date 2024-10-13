using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBot : HealthManager
{
    public static Text _enemyHPText;

    protected override void HandleDeath()
    {
        base.HandleDeath();
        _enemyHPText.text = $"Boss HP: 0";
        Player._playerHPText.color = Color.green;
        _enemyHPText.color = Color.red;
    }

    void Start()
    {
        SetHealthPoints(10, 10);
        SetParticipantName(gameObject.name);
        _enemyHPText = GameObject.FindWithTag("enemyHPText").GetComponent<Text>();
    }

    void Update()
    {
        _enemyHPText.text = $"Boss HP: {HealthPoints}";
    }
}
