using System.Collections;
public class KeyTime{
    public int value;
    public float time;
    public string tipo;
    public string curTime;
    public float distance;
    public KeyTime(){}
    public KeyTime(string _tipo,int _value, float _time, float _distance){
        curTime =System.DateTime.Now.ToString("HH':'mm':'ss");
        tipo=_tipo;
        value=_value;
        time = _time;
        distance=_distance;
    }
}