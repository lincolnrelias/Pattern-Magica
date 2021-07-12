using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class Button : MonoBehaviour{
    
     Webcam webcamInstance;
    [SerializeField]
    int offsetY = 100;
    GameObject hand;
    RectTransform rt;
    public bool canCheck = false;
    Color32 checkedColor;
    GameObject checkedEffect;
    Color32 uncheckedColor;
    AudioSource audioSource;
    Vector3 effectOffset;
    AudioClip pointSound;
    Image img;
    bool isChecked;
    Animator animator;
    Rigidbody2D rb;
    Transform handContainer;
    int x,y,h,w;

    void Start(){
        webcamInstance = FindObjectOfType<Webcam>();
        img = GetComponent<Image>();
         rt = GetComponent<RectTransform>();
         audioSource = GetComponent<AudioSource>();
        rb=gameObject.AddComponent<Rigidbody2D>();
         rb.bodyType=RigidbodyType2D.Static;
         rt.localScale=new Vector3(1.3f,1.3f,1f);
         //Tenta pegar o componente do pai
         patternExample pattern  = transform.parent.GetComponent<patternExample>();
         animator = GetComponent<Animator>();
         hand = transform.GetChild(1).gameObject;
         //Se não der certo pega do primeiro filho do pai
         if(!pattern){pattern = transform.parent.GetChild(0).GetComponent<patternExample>();};
         checkedEffect = pattern.getPointEffect();
         pointSound =pattern.getPointSound();
         uncheckedColor = pattern.getColors()[1];
         checkedColor = pattern.getColors()[0];
         effectOffset = pattern.getEffectOffset();
        }

    public void switchColor(){
        if(img.color==checkedColor){
            img.color = uncheckedColor;
        }else{
            img.color = checkedColor;
        }
    }
    public void setChecked(bool state){
        isChecked = state;
        animator.SetBool("Checked",state);
    }
    public bool IsChecked(){
        return isChecked;
    }
    public void ejectButton(){
        rb.bodyType=RigidbodyType2D.Dynamic;
        rb.gravityScale=50;
         rb.mass=10;
         float xForce=Random.Range(-800f,800f);
        rb.AddForce(new Vector2(xForce,1000f),ForceMode2D.Impulse);
    }
    void Update(){
        // int x = (int) (100*webcamInstance.scale),
        //     y = (int) (190*webcamInstance.scale),
        //     s = (int) (100*webcamInstance.scale);
        hand.SetActive(PlayerPrefs.GetInt("MostrarIndicadores")==1 && canCheck && !isChecked);
        if(isChecked){
            return;
        }
             x = (int) (Mathf.Abs(rt.anchoredPosition.x)*webcamInstance.scaleHorizontal);
            y = (int) ((Mathf.Abs(rt.anchoredPosition.y-offsetY))*webcamInstance.scaleHorizontal);
            w = (int) (rt.rect.width*webcamInstance.scaleHorizontal);
            h= (int) (rt.rect.width*webcamInstance.scaleVertical);
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space)){
            checkSequence();
        }
        #endif
        if(webcamInstance.checkArea( x, y, w, w ) ){
            checkSequence();
        }
        animator.SetBool("Shine",canCheck && !isChecked && PlayerPrefs.GetInt("MostrarIndicadores")==1?true:false);
    }
    void checkSequence(){
        if(canCheck){
            animator.SetTrigger("Check");
            setChecked(true);
            GameObject effectTemp = Instantiate(checkedEffect,transform.position+effectOffset,Quaternion.identity);
            effectTemp.transform.SetParent(transform.parent.parent);
            effectTemp.transform.SetSiblingIndex(1);
            GameObject.Destroy(effectTemp,2f);
            if(!audioSource.isPlaying){
               audioSource.PlayOneShot(pointSound); 
                }
            }else{
                FindObjectOfType<Pattern>().errorAdd(transform.GetSiblingIndex()+1);
            }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(new Vector3(Screen.width/2,Screen.height/2,0),20f);
        Gizmos.DrawWireSphere(transform.position,20f);
    }
    private void OnDestroy() {
        if(hand){
         GameObject.Destroy(hand);   
        }
        
    }
}
