using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : MonoBehaviour
{
    // Start is called before the first frame update
    Enemy target;
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    AudioClip explosionSound;
    bool isDead=false;
    [SerializeField]
    GameObject meteorHead;
    void Start()
    {
       target = FindObjectOfType<Enemy>();
       target.canAttack=false;
       target.canMove=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead || !target){
            Destroy(this.gameObject,1f);
            return;
            }
        transform.position  = Vector3.MoveTowards(transform.position,target.transform.position,speed*Time.deltaTime);
        transform.LookAt(target.transform.position);
        if(transform.position==target.transform.position){
            GetComponent<AudioSource>().PlayOneShot(explosionSound);
            target.Die();
            GetComponent<ParticleSystem>().Stop();
            meteorHead.SetActive(false);
            isDead=true;
        }
    }
}
