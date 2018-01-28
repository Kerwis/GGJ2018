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
    int myArea=0;
    int sateliteEarnings = 1;

    int areaEarningsRatio = 1;

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
        myEarnings += SatMenager.mySatelliteSpawners.MineSateliteCounter * sateliteEarnings;
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
            sateliteCost += SatMenager.mySatelliteSpawners.MineSateliteCounter * 5;
            SatMenager.mySatelliteSpawners.OnMineSateliteCreate.Invoke();
            UpdateEarnings();
            
        }

    }

    void GetPaid(int nrTurn)
    {
        myCash +=myEarnings;
        UpdateTexts();
    }
    void GetArea() { }

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
