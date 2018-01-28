using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Satelites
{
    public class TrianglePainter : MonoBehaviour
    {
        List<Vector3> dontRedrawThisMine = new List<Vector3>();
        List<Vector3> dontRedrawThisOpp = new List<Vector3>();
        public float alowedDistance=30;
        public Color mineColor;
        public Color enemyColor;
        public Texture2D copy;
        public Texture2D globeTexture;
        Color32[] newTex;
        Color[] Buff;
        int GminX, GmaxX, GminY, GmaxY;
        int allPixels;

        

        public int GetPlayerPixelCount()
        {
            Color bc = new Color(0, 0, 0, 0);
            if (GminX == globeTexture.width || GminY == globeTexture.height || GmaxX == 0 || GmaxY == 0)
                return 0;
            //find min max x y
            Buff = globeTexture.GetPixels(GminX, GminY, GmaxX - GminX, GmaxY - GminY);
            foreach(Color c in Buff)
            {
                if (c !=bc)
                 allPixels++;
            }
            return allPixels;
        }

        private void OnEnable()
        {
            GminX = globeTexture.width;
            GminY = globeTexture.height;
            GmaxX = 0;
            GmaxY = 0;
            mineColor.a=.4f;
            enemyColor.a = .4f;
            //SatMenager.Instance.OnMineSateliteCreate.AddListener(TurnDraw);
            //SatMenager.Instance.OnOpponentSateliteCreate.AddListener(TurnDraw);
        }
        private void OnDisable()
        {
            //SatMenager.Instance.OnMineSateliteCreate.RemoveListener(TurnDraw);
            //SatMenager.Instance.OnOpponentSateliteCreate.RemoveListener(TurnDraw);
        }
        private void Start()
        {
            newTex = globeTexture.GetPixels32();
            
            //MainController.NextTurn +=  
            //OnTurnEnd.AddListener(drawTris);
        }
        public void TurnDraw()
        {
            allPixels = 0;
            dontRedrawThisMine.Clear();
            dontRedrawThisOpp.Clear();
            newTex = copy.GetPixels32();
            globeTexture.SetPixels32(newTex);
            IterateTris();
            globeTexture.SetPixels32(newTex);
            globeTexture.Apply();
            InGameManager.Instance.UpdateEarnings();


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                IterateTris();
                globeTexture.SetPixels32(newTex);
                globeTexture.Apply();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                dontRedrawThisMine.Clear();
                dontRedrawThisOpp.Clear();
                newTex = copy.GetPixels32();
                globeTexture.SetPixels32(newTex);
                globeTexture.Apply();
            }
            //OnTurnEnd.AddListener(drawTris);
        }

        void IterateTrisMe()
        {
            //MOJE
            if (SatMenager.mySatelliteSpawners.MineSateliteCounter < 3)
            {
                return;
            }

            for (int i = 0; i < SatMenager.mySatelliteSpawners.MineSateliteCounter; i++)
            {
                for (int j = i + 1; j < SatMenager.mySatelliteSpawners.MineSateliteCounter; j++)
                {
                    for (int k = j + 1; k < SatMenager.mySatelliteSpawners.MineSateliteCounter; k++)
                    {
                        //Debug.Log(i + " " + j + " " + k);
                        if (CheckIfValid(i, j, k,SatMenager.mySatelliteSpawners)) {
                            
                            DrawTris(i, j, k, true);
                            //Debug.Log(i +" "+ j + " " + k);
                        }

                    }
                }
            }
        }
        void IterateTrisEnemy()
        {
            //Przeciwnik
            //Debug.Log("SatMenager.enemySatelliteSpawners.MineSateliteCounter " + SatMenager.enemySatelliteSpawners.MineSateliteCounter);
            if (SatMenager.enemySatelliteSpawners.MineSateliteCounter < 3)
            {
                return;
            }

            for (int i = 0; i < SatMenager.enemySatelliteSpawners.MineSateliteCounter; i++)
            {
                for (int j = i + 1; j < SatMenager.enemySatelliteSpawners.MineSateliteCounter; j++)
                {
                    for (int k = j + 1; k < SatMenager.enemySatelliteSpawners.MineSateliteCounter; k++)
                    {
                        if(CheckIfValid(i, j, k, SatMenager.enemySatelliteSpawners))
                            DrawTris(i, j, k, false);

                    }
                }
            }
        }

        private bool CheckIfValid(int a, int b, int c, SatelliteSpawner SatelliteSpawners)
        {
            bool valid = true;
            Vector2 CordsA = SatelliteSpawners.MineSatelitesCords[a];
            Vector2 CordsB = SatelliteSpawners.MineSatelitesCords[b];
            Vector2 CordsC = SatelliteSpawners.MineSatelitesCords[c];
            //check distance between all
            //Debug.Log(Vector2.Distance(CordsA, CordsB));
            if (Vector2.Distance(CordsA, CordsB) > alowedDistance)
            {
                valid = false;
            }
            // Debug.Log(CordsA+" "+CordsB + " "+Vector2.Distance(CordsA, CordsC));
            if (Vector2.Distance(CordsA, CordsC) > alowedDistance)
            {
                valid = false;
            }
            //Debug.Log(Vector2.Distance(CordsC, CordsB));
            if (Vector2.Distance(CordsC, CordsB) > alowedDistance)
            {
                valid = false;
            }

            return valid;
        }


        void IterateTris()
        {
            IterateTrisMe();
            IterateTrisEnemy();
            

            //rysuj
            
        }

        private void DrawTris(int q, int w, int e,bool me)
        {
            if (me)
            {
                Vector2[] bigThree = new Vector2[] { SatMenager.mySatelliteSpawners.MineSatelitesCords[q], SatMenager.mySatelliteSpawners.MineSatelitesCords[w], SatMenager.mySatelliteSpawners.MineSatelitesCords[e] };

                //check for enemy

                for (int i = 0; i < SatMenager.enemySatelliteSpawners.MineSateliteCounter; i++)
                {

                    if (IsPointInPolygon(SatMenager.enemySatelliteSpawners.MineSatelitesCords[i], bigThree))
                    {
                        Debug.Log("enemy");
                        //remove from dont redraw
                        if (dontRedrawThisMine.Contains(new Vector3(q, w, e))) ;
                        dontRedrawThisMine.Remove(new Vector3(q, w, e));
                        return;
                    }
                }
                //check if drawen
                if (dontRedrawThisMine.Contains(new Vector3(q, w, e)))
                    return;
                //no enemy? Draw AND SAVE
                dontRedrawThisMine.Add(new Vector3(q, w, e));
                //Vector2[] vv = 
                SetupPaintedArray(bigThree,mineColor);
            }
            else
            {
                Vector2[] bigThree = new Vector2[] { SatMenager.enemySatelliteSpawners.MineSatelitesCords[q], SatMenager.enemySatelliteSpawners.MineSatelitesCords[w], SatMenager.enemySatelliteSpawners.MineSatelitesCords[e] };

                //check for enemy

                for (int i = 0; i < SatMenager.mySatelliteSpawners.MineSateliteCounter; i++)
                {

                    if (IsPointInPolygon(SatMenager.mySatelliteSpawners.MineSatelitesCords[i], bigThree))
                    {
                        Debug.Log("enemy");
                        //remove from dont redraw
                        if (dontRedrawThisOpp.Contains(new Vector3(q, w, e))) ;
                        dontRedrawThisOpp.Remove(new Vector3(q, w, e));
                        return;
                    }
                }
                //check if drawen
                if (dontRedrawThisOpp.Contains(new Vector3(q, w, e)))
                    return;
                //no enemy? Draw AND SAVE
                dontRedrawThisOpp.Add(new Vector3(q, w, e));
                //Vector2[] vv = 
                SetupPaintedArray(bigThree,enemyColor);
            }
            

        }

        private void SetupPaintedArray(Vector2[] trio,Color c)
        {
            //List<Vector2> ListToPaint = new List<Vector2>();
            float minX = trio[0].x;if (minX > trio[1].x) minX = trio[1].x; if (minX > trio[2].x) minX = trio[2].x;
            float minY = trio[0].y; if (minY > trio[1].y) minY = trio[1].y; if (minY > trio[2].y) minY = trio[2].y;
            float maxX = trio[0].x; if (maxX < trio[1].x) maxX = trio[1].x; if (maxX < trio[2].x) maxX = trio[2].x;
            float maxY = trio[0].y; if (maxY < trio[1].y) maxY = trio[1].y; if (maxY < trio[2].y) maxY = trio[2].y;
            //Debug.Log(minX+" " + maxX + " " + minY + " " + maxY);
            if (maxX - minX < globeTexture.width / 2)
            {
                for (int i = (int)minX; i < (int)maxX; i++)
                {
                    for (int j = (int)minY; j < (int)maxY; j++)
                    {
                        Vector2 v = new Vector2(i, j);
                        if (IsPointInPolygon(v, trio))
                        {

                            newTex[(int)v.y * globeTexture.height + (int)v.x] = c;

                            //allPixels++;
                        }

                    }
                }
            }
            else
            {
                for (int i=0; i<3;i++) {
                    if (trio[i].x < globeTexture.width / 2)
                        trio[i].x += globeTexture.width;
                }
                minX = trio[0].x; if (minX > trio[1].x) minX = trio[1].x; if (minX > trio[2].x) minX = trio[2].x;
                maxX = trio[0].x; if (maxX < trio[1].x) maxX = trio[1].x; if (maxX < trio[2].x) maxX = trio[2].x;

                for (int i = (int)minX; i < (int)maxX; i++)
                {
                    for (int j = (int)minY; j < (int)maxY; j++)
                    {
                        Vector2 v = new Vector2(i, j);
                        if (j> globeTexture.width)
                            v= new Vector2(i, j- globeTexture.width);
                        if (IsPointInPolygon(v, trio))
                        {
                            newTex[(int)v.y * globeTexture.height + (int)v.x] = c;
                           

                        }

                    }
                }
            }
            if(maxX>GmaxX)
                GmaxX = (int)maxX;
            if (maxY > GmaxY)
                GmaxY = (int)maxY;
            if (minY < GminY)
                GminY = (int)minY;
            if (minX < GminX)
                GminX = (int)minX;
            /*Vector2[] arrayToPaint = new Vector2[ListToPaint.Count];
            for (int i = 0; i < ListToPaint.Count; i++)
                arrayToPaint[i] = ListToPaint[i];*/



            //return arrayToPaint;
        }

        public bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
        {
            int polygonLength = polygon.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.x, pointY = point.y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            Vector2 endPoint = polygon[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.y;
            while (i < polygonLength)
            {
                startX = endX; startY = endY;
                endPoint = polygon[i++];
                endX = endPoint.x; endY = endPoint.y;
                //
                //if (Mathf.Abs(endX - startX) < globeTexture.width/2)
                //{
                    inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
                              && /* if so, test if it is under the segment */
                              ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
                //}
                /*else
                {
                    if(startX< globeTexture.width/2)
                        startX += globeTexture.width;
                    else
                        endX += globeTexture.width;

                    if (pointX < globeTexture.width / 2)
                        pointX += globeTexture.width;
                    inside ^= (endY > pointY ^ startY > pointY) // ? pointY inside [startY;endY] segment ? 
                              && // if so, test if it is under the segment 
                              ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
                    if (startX > globeTexture.width)
                        startX -= globeTexture.width;
                    else
                        endX -= globeTexture.width;
                }*/
                
            }
            return inside;
        }
        private void OnApplicationQuit()
        {
            newTex = copy.GetPixels32();
            globeTexture.SetPixels32(newTex);
            globeTexture.Apply();
        }

    }
}
