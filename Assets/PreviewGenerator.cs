using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PreviewGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject btnParent;
    [SerializeField]
    GameObject LineParent;
    GameObject btnInst;
    GameObject lineInst;
    public void showPreviewElements(){
        btnInst = Instantiate(btnParent,transform);
        lineInst = Instantiate(LineParent,transform);
        btnInst.transform.localPosition = new Vector3(-126.400002f,-21,0);
         lineInst.transform.localPosition = new Vector3(-126.400002f,-21,0);
         btnInst.transform.localScale = new Vector3(1.2f,1.05f,1);
         lineInst.transform.localScale = new Vector3(1.2f,1.05f,1);
         lineInst.transform.SetAsFirstSibling();
         GameObject.Destroy(btnInst.transform.GetChild(btnInst.transform.childCount-1).gameObject);
    }
    public void clearPreview(){
        GameObject.Destroy(btnInst);
        GameObject.Destroy(lineInst);
    }

}
