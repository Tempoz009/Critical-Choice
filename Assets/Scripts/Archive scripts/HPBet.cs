using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBet : MonoBehaviour
{
    public static int hp_bet_value;

    public void SelectBetValue(Button button)
    {
        if(button.name == "Bet1HP")
        {
            Debug.Log("Сделана ставка в 1 ОЧКО здоровья!");
            hp_bet_value = 1;
        }
        else if(button.name == "Bet2HP")
        {
            Debug.Log("Сделана ставка в 2 ОЧКА здоровья!");
            hp_bet_value = 2;
        }   
        else if(button.name == "Bet3HP")
        {
            Debug.Log("Сделана ставка в 3 ОЧКА здоровья!");
            hp_bet_value = 3;
        }
    }
}
