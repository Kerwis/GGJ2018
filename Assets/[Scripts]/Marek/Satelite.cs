using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite : MonoBehaviour
{
    Renderer rend;
    Ray ray;
    MeshCollider meshCollider;
    void Start()
    {
        //ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(transform.position,transform.forward,out hit,100f))
        {
            Debug.Log("brak hitu");
            return;
        }
        Debug.Log(hit.transform.gameObject.name);
        
        rend = hit.transform.GetComponent<Renderer>();
        meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
        {
            Debug.Log(rend.name +" .. "+ rend.sharedMaterial + " .. " + meshCollider);
            return;
        }

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        for (int i = 0; i <= 100; i++) {
            Debug.Log(pixelUV.x+" "+ pixelUV.y);
            tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
            tex.Apply();
            pixelUV += new Vector2(1, 1);
        }
        
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, transform.forward * 50f);
    }
}