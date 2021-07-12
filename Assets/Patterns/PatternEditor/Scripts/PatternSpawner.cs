using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    GameObject pointParentPrefab;
    [SerializeField]
    GameObject patternExamplePrefab;
    [SerializeField]
    GameObject lineDrawer;
    [SerializeField]
    Sprite lineImage;
    [SerializeField]
    float lineThickness=10f;
    GameObject patternExampleInst;
    GameObject pointParent;
    int btnIndex=0;
    void Start()
    {
       pointParent = Instantiate(pointParentPrefab,transform.parent);
       pointParent.transform.localPosition = new Vector3(-260,-37,-520.011658f);
       pointParent.transform.localScale =new Vector3(1.5f,1.29999995f,1f);
       PointMapper.indexObjects(pointParent.transform);
    }

    // Update is called once per frame
    public GameObject getLineDrawer(){
        return lineDrawer;
    }
    public GameObject spawnPattern(List<int[]> listOfCoords,string patternName)
    {
         patternExampleInst = Instantiate(patternExamplePrefab,transform); 
         patternExampleInst.name = patternName;
        for (int i = 0; i < listOfCoords.Count; i++)
       {
           
            foreach (Transform point in pointParent.transform)
            {
                int row= point.GetComponent<SnappPoint>().getRow();   
                int collumn= point.GetComponent<SnappPoint>().getCollumn();  
                if(row==listOfCoords[i][0] && collumn==listOfCoords[i][1]){
                    GameObject currBtn=Instantiate(buttonPrefab,point.transform.position,Quaternion.identity);
                    currBtn.transform.SetParent(patternExampleInst.transform);
                    
                    if(btnIndex>0){
                      setBtnsAndDrawLine(btnIndex-1,btnIndex);
                    }
                    btnIndex++;
                    break;
                }
                
            }
           
        }
         setBtnsAndDrawLine(btnIndex-1,0);
        btnIndex=0;
        return patternExampleInst;
   }
   void setBtnsAndDrawLine(int index1, int index2){
       LineDrawer.drawLine(patternExampleInst.transform.GetChild(index1).gameObject,
        patternExampleInst.transform.GetChild(index2).gameObject,lineImage,lineDrawer,lineThickness,false);
   }
   
}
