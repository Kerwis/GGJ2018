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


    int sateliteCost;
    int winCost;

    int areaEarningsRatio = 10;

    public InGameManager()
    {
        
    }
    private void Awake()
    {
        myProgressT = GameObject.Find("WinText").GetComponent<Text>();
        myCashT = GameObject.Find("cash").GetComponent<Text>();
        myEarningsT = GameObject.Find("earnings").GetComponent<Text>();
        mySateliteCostT = GameObject.Find("satCost").GetComponent<Text>();
        buySat = GameObject.Find("Button").GetComponent<Button>();
        
    }
    private void OnEnable()
    {
        

        
        TP = GameObject.Find("MainController").GetComponentInChildren<TrianglePainter>();
        buySat.onClick.AddListener(BuySatelite);
        MainController.NextTurn+=GetPaid;
        //UpdateTexts();
    }
    //eventy
    public void UpdateEarnings()
    {
        //Debug.Log(TP.GetPlayerPixelCount());
        SatMenager.mySatelliteSpawners.myCash.myEarnings = 0;
        //foreachsatelite
        //SatMenager.mySatelliteSpawners.myCash.myEarnings += SatMenager.mySatelliteSpawners.MineSateliteCounter * SatMenager.mySatelliteSpawners.myCash.sateliteEarnings;
        //forarea
        //SatMenager.mySatelliteSpawners.myCash.myEarnings+= SatMenager.Instance.myArea * areaEarningsRatio;
        SatMenager.mySatelliteSpawners.myCash.myEarnings += TP.GetPlayerPixelCount()/areaEarningsRatio;
        //areaEarningsRatio *= 4;
        //Debug.Log(SatMenager.mySatelliteSpawners.myCash.myEarnings);
        UpdateTexts();

    }
    public void GainEarnings()
    {
        SatMenager.mySatelliteSpawners.myCash.myMoney += SatMenager.mySatelliteSpawners.myCash.myEarnings;
        if (SatMenager.mySatelliteSpawners.myCash.myMoney >= winCost)
            Win();
        else
            UpdateTexts();
        
    }
    public void BuySatelite()
    {
        if (SatMenager.mySatelliteSpawners.myCash.myMoney >= sateliteCost)
        {
            SatMenager.mySatelliteSpawners.OnMineSateliteCreate.Invoke();
            SatMenager.mySatelliteSpawners.myCash.myMoney -= sateliteCost;
            sateliteCost *= 2;
            ///SatMenager.mySatelliteSpawners.OnMineSateliteCreate.Invoke();
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
        Debug.Log("u win");
        //onWin;
    }

    void UpdateTexts()
    {
        myProgressT.text = SatMenager.mySatelliteSpawners.myCash.myMoney+"$" +" / 1000$";
        myCashT.text = SatMenager.mySatelliteSpawners.myCash.myMoney + "$";
        myEarningsT.text = "+" + SatMenager.mySatelliteSpawners.myCash.myEarnings + "$";
        mySateliteCostT.text = sateliteCost + "$";
    }
    public void UpdateTexts(string Prog,string cash,string earn,string cost)
    {
        myProgressT.text = cash + "$" + " / 1000$";
        myCashT.text = cash + "$";
        SatMenager.mySatelliteSpawners.myCash.myMoney = 110;
        myEarningsT.text = "+" + earn + "$";
        SatMenager.mySatelliteSpawners.myCash.myEarnings = 0;
        mySateliteCostT.text = cost + "$";
        sateliteCost = 15;
    }

}
