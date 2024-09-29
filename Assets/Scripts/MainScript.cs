using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;




public class MainScript : MonoBehaviour
{

    public int Coins = 0;

    public int StartAddCoins = 100;

    public TMP_Text CoinsText;

    public RectTransform BonusPanel;
    public int WednesdayBonus = 25;






    void Start()
    {
        WednesdayBonusEvent ();

        AddCoins(StartAddCoins);

        Application.targetFrameRate = 120;
    }


    void WednesdayBonusEvent ()
    {  
        DateTime currentDate = DateTime.Now;
        string currentDayOfWeek = currentDate.DayOfWeek.ToString();

        if(currentDayOfWeek == "Wednesday")
        {
            BonusPanel.gameObject.SetActive(true);
        }
    }

    public void ButtonGetBonusCoins ()
    {
        AddCoins(WednesdayBonus);
        BonusPanel.gameObject.SetActive(false);
    }


    public void AddCoins (int coins)
    {
        Coins += coins;
        CoinsText.text = Coins.ToString();
    }


}
