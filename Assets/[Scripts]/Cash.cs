using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    public int myMoney;
    public int myEarnings=1;
    public int myArea=0;
    public int sateliteEarnings = 1;
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(myMoney);
            stream.SendNext(myEarnings);
            stream.SendNext(myArea);            
            stream.SendNext(sateliteEarnings);
        }
        else
        {				
            myMoney = (int) stream.ReceiveNext();
            myEarnings = (int) stream.ReceiveNext();
            myArea = (int) stream.ReceiveNext();
            sateliteEarnings = (int) stream.ReceiveNext();
        }
    }
}
