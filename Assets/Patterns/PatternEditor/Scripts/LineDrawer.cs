using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LineDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    public static void drawLine(GameObject one, GameObject two, Sprite _lineImage,GameObject _lineDrawer,float _lineThickness,bool editor){
        RectTransform btn1 = one.GetComponent<RectTransform>();
        RectTransform btn2 = two.GetComponent<RectTransform>();
 
        RectTransform aux;
        if (btn1.localPosition.x > btn2.localPosition.x)
        {
            aux = btn1;
            btn1 = btn2;
            btn2 = aux;
        }
        GameObject lineObj  = new GameObject("lineObj", typeof(RectTransform));
        lineObj.transform.SetParent(_lineDrawer.transform);
        Image lineImg = lineObj.AddComponent<Image>();
        lineImg.sprite=_lineImage;
        lineImg.color=Color.black;
        RectTransform rectTransform = lineObj.GetComponent<RectTransform>();
        if (btn1.gameObject.activeSelf && btn2.gameObject.activeSelf)
        {
            rectTransform.localPosition = (btn1.localPosition + btn2.localPosition) / 2;
            Vector3 dif =  btn2.localPosition - btn1.localPosition;
            float sizeX = dif.magnitude;
            rectTransform.sizeDelta = new Vector3(sizeX, _lineThickness);
            rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }
   }
   public static void updateLinesScales(Transform lineDrawerParent){
        for(int i=0;i<lineDrawerParent.childCount;i++){
           lineDrawerParent.GetChild(i).localScale= new Vector3(1,1,1);
        }
   }
   public static void clearLines(GameObject _lineDrawer){
       foreach (Transform item in _lineDrawer.transform)
       {
           GameObject.Destroy(item.gameObject);
       }
   }
}
