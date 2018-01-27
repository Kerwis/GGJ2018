using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Satelites;

public class InGameManager : MonoBehaviour {

    //int initialCash = 15;
    
    int sateliteCost=10;
    int winCost=500;
    int myCash=15;
    int oppCash=0;

    int myEarnings=0;
    int sateliteEarnings = 1;
    int areaEarningsRatio = 1;

    private void OnEnable()
    {
        MainController.NextTurn+=GetPaid;
    }
    //eventy
    public void UpdateEarnings()
    {
        myEarnings = 0;
        //foreachsatelite
        myEarnings += SatMenager.Instance.MineSateliteCounter * sateliteEarnings;
        //forarea
        myEarnings*= SatMenager.Instance.myArea* areaEarningsRatio;

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
            SatMenager.Instance.OnMineSateliteCreate.Invoke();
            UpdateEarnings();
        }

    }

    void GetPaid(int nrTurn)
    {
        myCash = +myEarnings;
    }


    void Win()
    {

        //onWin;
    }

}
