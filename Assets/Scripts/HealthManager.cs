using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int HealthPoints { get; private set; }
    public int MaxHealthPoints { get; private set; }
    private string participantName;

    [SerializeField] private Text _deathText;

    public void ModifyHealth(int amount)
    {
        HealthPoints += amount;

        if (amount > 0)
        {
            Debug.Log(participantName + " получает " + amount + " очков здоровья.");
        }
        else if (amount < 0)
        {
            Debug.Log(participantName + " теряет " + Math.Abs(amount) + " очков здоровья.");
        }

        Debug.Log("Текущие очки здоровья у " + participantName + ": " + HealthPoints);

        if (HealthPoints > MaxHealthPoints)
        {
            HealthPoints = MaxHealthPoints;
            Debug.Log($"Превышено максимальное значение очков здоровья у {participantName}! \nУстановка максимально допустимого значения – {MaxHealthPoints}.");
        }

        if (HealthPoints <= 0)
        {
            HealthPoints = 0;
            HandleDeath();
        }
    }

    void Start()
    {
        HealthPoints = 10;
        MaxHealthPoints = 10;
    }

    public void SetHealthPoints(int HP, int maxHP)
    {
        HealthPoints = HP;
        MaxHealthPoints = maxHP;
    }

    public void SetParticipantName(string participant)
    {
        participantName = participant;
    }

    protected virtual void HandleDeath()
    {
        Debug.Log(participantName + " умирает!");
        _deathText.text = $"{participantName} dies!";
        Destroy(gameObject);

        foreach (GameObject obj1 in GameObject.FindGameObjectsWithTag("GoodItem"))
        {
            GameObject.Destroy(obj1);
        }

        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("BadItem"))
        {
            GameObject.Destroy(obj2);
        }
    }
}
