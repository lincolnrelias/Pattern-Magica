using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]Transform hittingPoint;
    [SerializeField]float speed = 5f;
    [SerializeField]float damage=5f;
    
    [SerializeField]AudioClip[] footstepSounds;
    [SerializeField]AudioClip[] attackSounds;
    [SerializeField]float hitVolume = 0.3f;
        [SerializeField]
    AudioClip[] deathSounds;
    [SerializeField]
    GameObject deathEffect;
    CastleHealth castleHealth;
    Animator enemyAnimator;
    bool dead = false;
    AudioSource enemyAudioSrc;
    float lastHittime=0;
    public bool canAttack=true;
    public bool canMove=true;
    bool attacking=false;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyAudioSrc = GetComponent<AudioSource>();
        castleHealth = FindObjectOfType<CastleHealth>();
        hittingPoint = FindObjectOfType<hittingPoint>().transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(dead)return;
        transform.LookAt(hittingPoint.position);
        if(canMove){
            enemyAnimator.SetBool("Running",true);
            transform.position = Vector3.MoveTowards(transform.position, hittingPoint.position, speed * Time.deltaTime);
        }else{
            enemyAnimator.SetBool("Running",false);
        }
        if(canAttack && transform.position==hittingPoint.position){
            enemyAnimator.SetBool("Running",false);
            enemyAnimator.SetBool("isAttacking",true);
            attacking=true;
        }else{
            enemyAnimator.SetBool("isAttacking",false);
            attacking=false;
        }
        if(attacking){
            if(!canAttack){
                enemyAnimator.SetBool("Running",false);
                enemyAnimator.SetBool("isAttacking",true);
            }
        }
    }
    public void Die(){
        dead=true;
        enemyAnimator.SetTrigger("Death");
        GameObject effect = GameObject.Instantiate(deathEffect,transform.position,Quaternion.identity);
        effect.GetComponent<ParticleSystem>().Play();
        enemyAudioSrc.volume=1f;
        enemyAudioSrc.PlayOneShot(deathSounds[Random.Range(0,deathSounds.Length)]);
        GameObject.Destroy(effect,1.5f);
        GameObject.Destroy(this.gameObject,2f);
    }
    void Hit(){
    castleHealth.TakeDamage(damage);
    enemyAudioSrc.clip=attackSounds[0];
    enemyAudioSrc.volume = hitVolume;
    enemyAudioSrc.Play();
    lastHittime=Time.time;
    }
    void FootR(){
       enemyAudioSrc.clip = footstepSounds[0];
       enemyAudioSrc.volume = 0.1f;
       enemyAudioSrc.Play();
    }
    void FootL(){
       enemyAudioSrc.clip = footstepSounds[1];
       enemyAudioSrc.volume = 0.1f;
       enemyAudioSrc.Play();
    }
    void NewEvent(){

    }
}
