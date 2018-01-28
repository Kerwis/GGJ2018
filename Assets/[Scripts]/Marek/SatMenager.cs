using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;
namespace Satelites
{
    public class SatMenager : Singleton<SatMenager>
    {

        public int myArea;
        
        public static SatelliteSpawner mySatelliteSpawners = new SatelliteSpawner();
        public static SatelliteSpawner enemySatelliteSpawners = new SatelliteSpawner();
        
        public SatMenager() { }

    }

}