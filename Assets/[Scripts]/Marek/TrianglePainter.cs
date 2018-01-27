using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Satelites
{
    public class TrianglePainter : MonoBehaviour
    {
        List<Vector3> dontRedrawThis = new List<Vector3>();

        public Color mineColor;
        public Texture2D globeTexture;
        SatMenager SM;
        Color32[] oldTex;
        Color32[] newTex;
        private void Start()
        {
            oldTex = globeTexture.GetPixels32();
            newTex = oldTex;
            Debug.Log(newTex.Length);
            SM = SatMenager.Instance;
            //OnTurnEnd.AddListener(drawTris);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                newTex = oldTex;
                IterateTris();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                globeTexture.SetPixels32(oldTex);
                globeTexture.Apply();
            }
            //OnTurnEnd.AddListener(drawTris);
        }

        void IterateTris()
        {
            //Debug.Log(SM.MineSateliteCounter);
            if (SM.MineSateliteCounter < 3)
            {
                Debug.Log("below 3 = "+SM.MineSateliteCounter);
                return;
            }

            for (int i = 0; i < SM.MineSateliteCounter; i++)
            {
                for (int j = i+1; j < SM.MineSateliteCounter; j++)
                {
                    for (int k = j+1; k < SM.MineSateliteCounter; k++)
                    {
                        
                        DrawTris(i,j,k);
                        
                    }
                }
            }
        }

        private void DrawTris(int q, int w, int e)
        {
            Vector2[] bigThree = new Vector2[] { SM.MineSatelitesPool[q].cords, SM.MineSatelitesPool[w].cords, SM.MineSatelitesPool[e].cords };

            //check for enemy

            for(int i = 0; i < SM.OpponentSateliteCounter - 1; i++)
            {
                if (IsPointInPolygon(SM.OpponentSatelitesPool[i].cords, bigThree))
                {
                    Debug.Log("enemy");
                    //remove from dont redraw
                    if(dontRedrawThis.Contains(new Vector3(q, w, e)));
                        dontRedrawThis.Remove(new Vector3(q, w, e));
                    return;
                }
            }
            //check if drawen
            if(dontRedrawThis.Contains(new Vector3(q, w, e)))
                return;
                //no enemy? Draw AND SAVE
            dontRedrawThis.Add(new Vector3(q, w, e));
            //Vector2[] vv = 
            SetupPaintedArray(bigThree);
            /*
            Debug.Log(vv.Length);
            foreach (Vector2 v in vv)
            {
                
                globeTexture.SetPixel((int)v.x, (int)v.y, mineColor);
            }*/
            globeTexture.SetPixels32(newTex);
            globeTexture.Apply();

        }

        private void SetupPaintedArray(Vector2[] trio)
        {
            List<Vector2> ListToPaint = new List<Vector2>();
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
                            newTex[(int)v.y * globeTexture.height + (int)v.x] = mineColor;

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
                            newTex[(int)v.y * globeTexture.height + (int)v.x] = mineColor;

                        }

                    }
                }
            }
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
        
    }
}
