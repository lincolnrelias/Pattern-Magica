using System.Collections;
using System.Collections.Generic;

public class PatternInfo 
{
 public string name;
 public List<int[]> listOfCoords;
 public PatternInfo(string _name,List<int[]> list){
     name=_name;
     listOfCoords=list;
 }
}
