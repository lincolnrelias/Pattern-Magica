using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    [SerializeField]int MaxHealth = 100;
    [SerializeField]Image healthBarImage;
    [SerializeField]float smoothness=5f;
    [SerializeField]Pattern pattern;
    [SerializeField]GameObject gameOverScreen;
    [SerializeField]AudioSource musicAs;
    float currentHealth;
    // Start is called before the first frame update
    void Start()
    {   
        MaxHealth =Mathf.RoundToInt(PlayerPrefs.GetFloat("CastleHealth"));
        PlayerPrefs.SetFloat("CastleHealth",MaxHealth);
        PlayerPrefs.Save();
        currentHealth = MaxHealth;
        healthBarImage.fillAmount = 1;
        musicAs.volume = PlayerPrefs.GetFloat("volMusica");
        
    }

    // Update is called once per frame
    void Update()
    {
        updateHealthBar();
    }
    public void TakeDamage(float damage){
        currentHealth-=damage;
        if(currentHealth<=0){
            GameOverSequence();
        }
    }
    public float getCurrHealth(){
        return currentHealth;
    }
    public float getMaxHealth(){
        return MaxHealth;
    }
    void updateHealthBar(){
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount,
        Mathf.Clamp(currentHealth/MaxHealth,0,1),
        smoothness*Time.deltaTime);
    }

    void GameOverSequence(){
        musicAs.Stop();
        gameOverScreen.SetActive(true);
        Time.timeScale=0;
        FindObjectOfType<Pattern>().GerarRelatório();
    }
}
