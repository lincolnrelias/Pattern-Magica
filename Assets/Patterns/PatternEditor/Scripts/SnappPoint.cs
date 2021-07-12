using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappPoint : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasButtonAttached=false;
    [SerializeField]
    int collumn;
    [SerializeField]
    int row;
    public int getCollumn(){
        return collumn;
    }
    public int getRow(){
        return row;
    }
    public int[,] getCoords(){
        return new int[row,collumn];
    }
    public void setCoordinates(int _row,int _collumn){
        row = _row;
        collumn = _collumn;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
