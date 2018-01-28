using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    public int myMoney;
    
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(myMoney);
        }
        else
        {				
            myMoney = (int) stream.ReceiveNext();
        }
    }
}
