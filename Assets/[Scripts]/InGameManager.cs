using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Satelites;

public class InGameManager : MonoBehaviour {

    //int initialCash = 15;
    public Text myProgressT;
    public Text myCashT;
    public Text myEarningsT;
    public Text mySateliteCostT;
    public Button buySat;


    int sateliteCost=10;
    int winCost=500;
    int myCash=15;
    int myEarnings=1;
    int sateliteEarnings = 1;

    int areaEarningsRatio = 1;

    int oppCash = 0;
    int oppEarnings = 0;
    int oppSatCost = 0;
    int oppWinCost = 0;

    private void OnEnable()
    {
        buySat.onClick.AddListener(BuySatelite);
        MainController.NextTurn+=GetPaid;
        UpdateTexts();
    }
    //eventy
    public void UpdateEarnings()
    {
        myEarnings = 1;
        //foreachsatelite
        myEarnings += SatMenager.Instance.MineSateliteCounter * sateliteEarnings;
        //forarea
        myEarnings+= SatMenager.Instance.myArea * areaEarningsRatio;

        UpdateTexts();

    }
    public void GainEarnings()
    {
        myCash += myEarnings;
        if (myCash >= winCost)
            Win();
        
    }
    public void BuySatelite()
    {
        if (myCash >= sateliteCost)
        {
            myCash -= sateliteCost;
            sateliteCost += SatMenager.Instance.MineSateliteCounter * 5;
            SatMenager.Instance.OnMineSateliteCreate.Invoke();
            UpdateEarnings();
            
        }

    }

    void GetPaid(int nrTurn)
    {
        myCash +=myEarnings;
        UpdateTexts();
    }


    void Win()
    {

        //onWin;
    }

    void UpdateTexts()
    {
        myProgressT.text = myCash+"$" +" / 1000$";
        myCashT.text = myCash + "$";
        myEarningsT.text = "+" + myEarnings + "$";
        mySateliteCostT.text = sateliteCost + "$";
    }

}
