using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : HealthManager
{
    public static Text _playerHPText;

    protected override void HandleDeath()
    {
        base.HandleDeath();
        _playerHPText.text = $"Player HP: 0";
        _playerHPText.color = Color.red;
        EnemyBot._enemyHPText.color = Color.green;

        AudioManager.Instance.PlaySFX("Lose");
    }

    void Start()
    {
        SetHealthPoints(10, 10);
        SetParticipantName(gameObject.name);
        _playerHPText = GameObject.FindWithTag("PlayerHPText").GetComponent<Text>();
    }

    void Update()
    {
        _playerHPText.text = $"Player HP: {HealthPoints}";
    }
}
