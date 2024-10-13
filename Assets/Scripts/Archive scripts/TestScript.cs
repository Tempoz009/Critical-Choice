using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public static int player_health_points = 3;
    public int enemy_health_points = 3;
    public bool is_good_item;

    public Dictionary<string, double> items_probability = new Dictionary<string, double>()
    {
        { "Water", 0.5 },
        { "Pills", 0.4 },
        { "Apple", 0.3 },
        { "Meat Pie", 0.45 },
        { "Soap",  0.7 }
    };

    public void CheckItemQuality(Button button)
    {   
        if(button.gameObject.tag == "GoodItem")
        {
            is_good_item = true;
            Debug.Log("Этот предмет оказался ХОРОШИМ!");
            FindItemProbability(button);
            player_health_points += HPBet.hp_bet_value; 
            Debug.Log($"Очки здоровья игрока: {player_health_points}");
        }
        else if(button.gameObject.tag == "BadItem")
        {
            is_good_item = false;
            Debug.Log("Этот предмет оказался ПЛОХИМ!");
            FindItemProbability(button);
            
            Debug.Log($"Очки здоровья игрока: {player_health_points}");
        }
    }

    public void FindItemProbability(Button button)
    {
        double x = UnityEngine.Random.Range(0f, 1f);   
        Debug.Log($"Случайное значение: {x}");

        foreach(var item in items_probability)
        {
            if (button.name == item.Key)
            {
                if(item.Value <= x && button.gameObject.tag == "BadItem")
                {
                    player_health_points -= HPBet.hp_bet_value;
                    Debug.Log($"Название предмета: {item.Key}, получено урона: {HPBet.hp_bet_value}!");
                }
                else if (item.Value <= x && button.gameObject.tag == "GoodItem")
                {
                    player_health_points += HPBet.hp_bet_value;
                    Debug.Log($"Название предмета: {item.Key}, получено очков здоровья: {HPBet.hp_bet_value}!");
                }   
                else if (item.Value >= x && button.gameObject.tag == "BadItem")
                {
                    Debug.Log($"Название предмета: {item.Key}, плохое действие предмета спарировано!");
                }
                else if (item.Value >= x && button.gameObject.tag == "GoodItem")
                {
                    Debug.Log($"Название предмета: {item.Key}, плохое действие предмета спарировано!");
                }
            }
        }
    }

    void Start()
    {
        Debug.Log($"Очки здоровья игрока: {player_health_points}");
        Debug.Log($"Очки здоровья противника: {enemy_health_points}");
    }
}
