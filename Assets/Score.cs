using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    int amountToAddPerBtn;
    float score=0;
    void Start()
    {
        
    }
    public void addScore(float timeSinceLastHit){
        timeSinceLastHit = Mathf.Clamp(timeSinceLastHit,.5f,2f);
        score+=amountToAddPerBtn/timeSinceLastHit;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        scoreText.text=Mathf.RoundToInt(score).ToString();
    }
}
