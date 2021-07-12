using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMapper : MonoBehaviour
{

    public static void indexObjects(Transform pointParent){
        int k=0;
        for(int i=0;i<14;i++){
            for(int j=0;j<10;j++){
                pointParent.GetChild(k).GetComponent<SnappPoint>().setCoordinates(j,i);
                k++;
            }
        }
    }
    // Update is called once per frame
}
