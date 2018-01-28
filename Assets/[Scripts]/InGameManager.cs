using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Satelites;

public class InGameManager : Singleton<InGameManager> {

    //int initialCash = 15;
    public Text myProgressT;
    public Text myCashT;
    public Text myEarningsT;
    public Text mySateliteCostT;
    public Button buySat;
    TrianglePainter TP;


    int sateliteCost=10;
    int winCost=500;

    int areaEarningsRatio = 1;

    public InGameManager()
    {
    }

    private void OnEnable()
    {
        TP = GameObject.Find("MainController").GetComponentInChildren<TrianglePainter>();
        buySat.onClick.AddListener(BuySatelite);
        MainController.NextTurn+=GetPaid;
        UpdateTexts();
    }
    //eventy
    public void UpdateEarnings()
    {
        Debug.Log(TP.GetPlayerPixelCount());
        SatMenager.mySatelliteSpawners.myCash.myEarnings = 1;
        //foreachsatelite
        //SatMenager.mySatelliteSpawners.myCash.myEarnings += SatMenager.mySatelliteSpawners.MineSateliteCounter * SatMenager.mySatelliteSpawners.myCash.sateliteEarnings;
        //forarea
        //SatMenager.mySatelliteSpawners.myCash.myEarnings+= SatMenager.Instance.myArea * areaEarningsRatio;
        SatMenager.mySatelliteSpawners.myCash.myEarnings += TP.GetPlayerPixelCount()/areaEarningsRatio;
        Debug.Log(SatMenager.mySatelliteSpawners.myCash.myEarnings);
        UpdateTexts();

    }
    public void GainEarnings()
    {
        SatMenager.mySatelliteSpawners.myCash.myMoney += SatMenager.mySatelliteSpawners.myCash.myEarnings;
        if (SatMenager.mySatelliteSpawners.myCash.myMoney >= winCost)
            Win();
        
    }
    public void BuySatelite()
    {
        if (SatMenager.mySatelliteSpawners.myCash.myMoney >= sateliteCost)
        {
            SatMenager.mySatelliteSpawners.myCash.myMoney -= sateliteCost;
            sateliteCost += SatMenager.mySatelliteSpawners.MineSateliteCounter * 5;
            SatMenager.mySatelliteSpawners.OnMineSateliteCreate.Invoke();
            UpdateEarnings();
            
        }

    }

    void GetPaid(int nrTurn)
    {
        SatMenager.mySatelliteSpawners.myCash.myMoney += SatMenager.mySatelliteSpawners.myCash.myEarnings;
        UpdateTexts();
    }
    void GetArea() { }

    void Win()
    {

        //onWin;
    }

    void UpdateTexts()
    {
        myProgressT.text = SatMenager.mySatelliteSpawners.myCash.myMoney+"$" +" / 1000$";
        myCashT.text = SatMenager.mySatelliteSpawners.myCash.myMoney + "$";
        myEarningsT.text = "+" + SatMenager.mySatelliteSpawners.myCash.myEarnings + "$";
        mySateliteCostT.text = sateliteCost + "$";
    }

}
