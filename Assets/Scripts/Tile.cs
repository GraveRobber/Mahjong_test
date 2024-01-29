using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using static UnityEditor.Progress;

public class Tile : MonoBehaviour
{
    public SpriteRenderer tileTypeSprite;
    public SpriteRenderer tileBackSprite;
    public SpriteRenderer shadowSprite;

    public GameObject VFX;
    
    public Vector3 tilePos;

    public TileData tileType { get; set; }
    public int ID { get; set; }
    public ParticleSystem SelectVFX { get; set; }
    public Renderer VFXRenderer { get; set; }
    public int orderInLayer { get; set; }

    
    public bool IsBloced { get; set; }

    public bool IsSelected { get; set; }


    private void OnMouseDown()
    {
        //Grid tempParent = TEMP(this.gameObject);
        //print(TEMPVectpr());
        //if (!IsBloced)
        //{
        //    Destroy(this.gameObject);
        //    tempParent.GridUpdate();
        //}

        if (!IsBloced)
        {
            TileEvent.InvokeTileClikedEvent(this);
            
        }       

    }



    //Grid TEMP(GameObject temp)
    //{
    //    Transform parentTransform = temp.transform;

    //    while(parentTransform.parent != null)
    //    {
    //        parentTransform = parentTransform.parent;
    //    }

    //    if (parentTransform != null)
    //    {
    //        return parentTransform.GetComponent<Grid>();
    //    }

    //    else return null;
    //}

    //Vector3 TEMPVectpr()
    //{
    //    for(int i=0; i<TEMP(this.gameObject).TilesArray.GetLength(0); i++)
    //    {
    //        for(int j=0; j<TEMP(this.gameObject).TilesArray.GetLength(1); j++)
    //        {
    //            for(int k=0; k<TEMP(this.gameObject).TilesArray.GetLength(2); k++)
    //            {
    //                if (TEMP(this.gameObject).TilesArray[i, j, k] == this)
    //                {
    //                    return new Vector3(i, j, k);
    //                }
    //            }
    //        }
    //    }

    //    return Vector3.one;
    //}
}
