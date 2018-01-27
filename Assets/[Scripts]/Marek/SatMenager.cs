using System.Collections;
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

        public List<GameObject> MineSatelitesPool;
        int MineSateliteCounter;
        public List<GameObject> OpponentSatelitesPool;
        int OpponentSateliteCounter;

        GameObject satToDestroy = null;

        UnityEvent OnMineSateliteCreate;
        UnityEvent OnMineSateliteDestroy;
        UnityEvent OnOpponentSateliteCreate;
        UnityEvent OnOpponentSateliteDestroy;

        public SatMenager()
        {
        }

        private void OnEnable()
        {
            MineSateliteCounter = 0;
            OpponentSateliteCounter = 0;

            foreach (GameObject o in MineSatelitesPool)
            {
                o.SetActive(false);
            }
            foreach (GameObject o in OpponentSatelitesPool)
            {
                o.SetActive(false);
            }

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && OnMineSateliteCreate != null)
            {
                OnMineSateliteCreate.Invoke();
            }
        }
        void CreateMineSatelite()
        {
            if (MineSateliteCounter == MineSatelitesPool.Count)
                return;

            MineSateliteCounter++;
            MineSatelitesPool[MineSateliteCounter - 1].SetActive(true);
            SetupMineSatelite(MineSatelitesPool[MineSateliteCounter - 1]);
        }

        private void SetupMineSatelite(GameObject sat)
        {
            sat.transform.rotation = Aim.transform.rotation;
        }

        void DestroyMineSatelite()
        {
            if (satToDestroy == null)
                return;
            if (MineSateliteCounter == 0)
                return;
            if (!MineSatelitesPool.Contains(satToDestroy))
                return;
            int j = MineSatelitesPool.IndexOf(satToDestroy);
            MineSatelitesPool[j] = MineSatelitesPool[MineSateliteCounter - 1];
            MineSatelitesPool.RemoveAt(MineSateliteCounter - 1);
            MineSateliteCounter--;

        }

        void CreateOpponentSatelite()
        {
            if (OpponentSateliteCounter == OpponentSatelitesPool.Count)
                return;

            OpponentSateliteCounter++;
            OpponentSatelitesPool[OpponentSateliteCounter - 1].SetActive(true);
        }
        void DestroyOpponentSatelite()
        {
            if (satToDestroy == null)
                return;
            if (OpponentSateliteCounter == 0)
                return;
            if (!OpponentSatelitesPool.Contains(satToDestroy))
                return;
            int j = OpponentSatelitesPool.IndexOf(satToDestroy);
            OpponentSatelitesPool[j] = OpponentSatelitesPool[OpponentSateliteCounter - 1];
            OpponentSatelitesPool.RemoveAt(OpponentSateliteCounter - 1);
            OpponentSateliteCounter--;
        }

        void OnDisable()
        {
            OnMineSateliteCreate.RemoveListener(CreateMineSatelite);
            OnMineSateliteDestroy.RemoveListener(DestroyMineSatelite);

            OnOpponentSateliteCreate.RemoveListener(CreateOpponentSatelite);
            OnOpponentSateliteDestroy.RemoveListener(DestroyOpponentSatelite);
        }

        public void DestroySatelite(GameObject satToDestroy)
        {
            this.satToDestroy = satToDestroy;
            if (OpponentSatelitesPool.Contains(satToDestroy))
                OnOpponentSateliteDestroy.Invoke();
            if (MineSatelitesPool.Contains(satToDestroy))
                OnMineSateliteDestroy.Invoke();

        }
    }

}