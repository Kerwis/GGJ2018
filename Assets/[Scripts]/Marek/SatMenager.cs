﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;
namespace Satelites
{
    public class SatMenager : Singleton<SatMenager>
    {
        [SerializeField]
        public GameObject Aim;
        
        public List<Satelite> MineSatelitesPool;
        [HideInInspector]
        public int MineSateliteCounter;
        public List<Satelite> OpponentSatelitesPool;
        [HideInInspector]
        public int OpponentSateliteCounter;

        public int myArea;

        Satelite satToDestroy = null;

        public UnityEvent OnMineSateliteCreate;
        public UnityEvent OnMineSateliteDestroy;
        public UnityEvent OnOpponentSateliteCreate;
        public UnityEvent OnOpponentSateliteDestroy;

        public SatMenager() { }

        public PhotonView myView;
        

        private void OnEnable()
        {
            MineSateliteCounter = 0;
            OpponentSateliteCounter = 0;

            #region EventsInit
            if (OnMineSateliteCreate == null)
                OnMineSateliteCreate = new UnityEvent();
            if (OnMineSateliteDestroy == null)
                OnMineSateliteDestroy = new UnityEvent();

            if (OnOpponentSateliteCreate == null)
                OnOpponentSateliteCreate = new UnityEvent();
            if (OnOpponentSateliteDestroy == null)
                OnOpponentSateliteDestroy = new UnityEvent();

            OnMineSateliteCreate.AddListener(CreateMineSatelite);
            OnMineSateliteDestroy.AddListener(DestroyMineSatelite);

            OnOpponentSateliteCreate.AddListener(CreateOpponentSatelite);
            OnOpponentSateliteDestroy.AddListener(DestroyOpponentSatelite);
            #endregion
        }
        private void Start()
        {
           
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && OnMineSateliteCreate != null)
            {
                Debug.Log(MineSateliteCounter);
                OnMineSateliteCreate.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.O) && OnMineSateliteCreate != null)
            {
                OnOpponentSateliteCreate.Invoke();
            }
        }
        void CreateMineSatelite()
        {
            //TODO Add check max satelite
            
            myView.RPC("CreateSatellite", PhotonTargets.All);
            
        }

        [PunRPC]
        private void CreateSatellite()
        {
            GameObject go = PhotonNetwork.Instantiate("Satelite", Vector3.zero, Quaternion.identity, 0);
            Satelite sat = go.GetComponentInChildren<Satelite>();
            if (myView.isMine)
            {
                MineSatelitesPool.Add(sat);
                MineSateliteCounter++;
            }
            else
            {
                OpponentSatelitesPool.Add(sat);
                MineSateliteCounter++;
            }
            SetupSatelite(sat);
            TrianglePainter.Instance.TurnDraw();
        }

        private void SetupSatelite(Satelite sat)
        {
            sat.transform.parent.transform.rotation = Aim.transform.rotation;
            sat.Setup();
        }

        //TODO
        void DestroyMineSatelite()
        {
            if (satToDestroy == null)
                return;
            if (MineSateliteCounter == 0)
                return;
            //if (!MineSatelitesPool.Contains(satToDestroy))
                return;
            //int j = MineSatelitesPool.IndexOf(satToDestroy);
            //MineSatelitesPool[j] = MineSatelitesPool[MineSateliteCounter - 1];
            //MineSatelitesPool.RemoveAt(MineSateliteCounter - 1);
            MineSateliteCounter--;

        }

        void CreateOpponentSatelite()
        {
            //Raczej tego nie potrzeba
            
            TrianglePainter.Instance.TurnDraw();
        }
        void DestroyOpponentSatelite()
        {
            //Też tego nie potrzeba
            
            if (satToDestroy == null)
                return;
            if (OpponentSateliteCounter == 0)
                return;
            //if (!OpponentSatelitesPool.Contains(satToDestroy))
                return;
            //int j = OpponentSatelitesPool.IndexOf(satToDestroy);
            //OpponentSatelitesPool[j] = OpponentSatelitesPool[OpponentSateliteCounter - 1];
            //OpponentSatelitesPool.RemoveAt(OpponentSateliteCounter - 1);
            OpponentSateliteCounter--;
        }

        void OnDisable()
        {
            OnMineSateliteCreate.RemoveListener(CreateMineSatelite);
            OnMineSateliteDestroy.RemoveListener(DestroyMineSatelite);

            OnOpponentSateliteCreate.RemoveListener(CreateOpponentSatelite);
            OnOpponentSateliteDestroy.RemoveListener(DestroyOpponentSatelite);
        }

        public void DestroySatelite(Satelite satToDestroy)
        {
            this.satToDestroy = satToDestroy;
            //if (OpponentSatelitesPool.Contains(satToDestroy))
            //    OnOpponentSateliteDestroy.Invoke();
            //if (MineSatelitesPool.Contains(satToDestroy))
            //    OnMineSateliteDestroy.Invoke();

        }
    }

}