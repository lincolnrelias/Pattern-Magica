using System.Collections;
using TMPro;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    public static IEnumerator showMessage(TMP_Text messageHolder,string message,float duration){
        messageHolder.text=message;
        yield return new WaitForSecondsRealtime(duration);
        messageHolder.text="";
    }
}
