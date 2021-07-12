using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Pattern : MonoBehaviour
{   
    AudioClip completionSound,pointSound,errorSound;
    Color32 checkedColor,uncheckedColor;
    int currentNode,exampleIndex=1;
    float interval;
    AudioSource audioSource;
    bool done=false,playing=true,reseting=true;
    string lastPatternName;
    float lastHitTime,lastErrorTime;
    GameObject[] patternPrefabs;
    [SerializeField]
    float minErrorInterval=2f;
    [SerializeField]
    float minHitInterval=0.3f;
    [SerializeField]
    float intervalBetweenPatterns= 1.5f;
    [SerializeField]
    GameObject spell;
    [SerializeField]
    GameObject mageObj;
    [SerializeField]
	GameObject completionScreen;
    [SerializeField]
    Webcam webcam;
    [SerializeField]
    Score score;
    List<KeyTime> parAcertosEErros=new List<KeyTime>();
    // Start is called before the first frame update
    [SerializeField]
    EnemySpawner enemySpawner;
    [SerializeField]AudioSource musicAs;
    int currPatternIndex=0;
    GeradorRelatório geradorRelatório;
    bool ischecking=false;
    bool lastWasError=false;
    PatternSpawner patternSpawner;
    string[] patternList;
    void Start()
    {
        StartSequence();
    }
    public void StartSequence(){
        Time.timeScale=1;
        geradorRelatório = GetComponent<GeradorRelatório>();
        patternSpawner=GetComponent<PatternSpawner>();
        string setName = PlayerPrefs.GetString("currentSequence");
        patternList =File.ReadAllLines(Path.Combine(Application.persistentDataPath, "sets/"+setName+".csv"));
        if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
        lastHitTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        StartCoroutine(spawnPatternAfterDelay(intervalBetweenPatterns));
    }

    // Update is called once per frame
    void Update()
    {
        LineDrawer.updateLinesScales(patternSpawner.getLineDrawer().transform);
        if(playing || done || reseting){
            lastHitTime = Time.time;
            return;
        }
        if(currentNode>=transform.childCount && !InErrorInterval()){
            if(currentNode==transform.childCount){PatternCompletion();}
            return;
            }
        Transform child = transform.GetChild(currentNode);
        Button currBtn = child.GetComponent<Button>();
        currBtn.canCheck=true;
        if(currBtn.IsChecked()){
            currentNode=currentNode=Mathf.Clamp(currentNode+1,1,transform.childCount);
            lastWasError=false;
            float hitTime = Time.time-lastHitTime;
            parAcertosEErros.Add(new KeyTime("Acerto",currentNode-1,hitTime,distanceToEnemy()));
            score.addScore(hitTime);
            lastHitTime = Time.time;
        }     
        

    }
    float distanceToEnemy(){
        Transform enemy = FindObjectOfType<Enemy>().transform;
        Transform point = FindObjectOfType<hittingPoint>().transform;
        return Vector3.Distance(enemy.position,point.position);
    }
    public bool InErrorInterval(){
        
        return Time.time-lastErrorTime<minErrorInterval;
    }
    public bool inHitInterval(){
        return Time.time-lastHitTime<minHitInterval;
    }
    public void errorAdd(int btnIndex){
        if(InErrorInterval() || playing || currentNode<1){return;};
        lastErrorTime = Time.time;
        parAcertosEErros.Add(new KeyTime("Erro",btnIndex,Time.time-lastErrorTime,distanceToEnemy()));
        lastWasError=true;
        Button btnCurr;
        FindObjectOfType<Enemy>().canMove=false;
        
            int nodeC = transform.childCount-1;
            while(nodeC>0){
                btnCurr = transform.GetChild(nodeC).GetComponent<Button>();
                btnCurr.ejectButton();
                nodeC--;
            }
                PatternCompletion();
        
            audioSource.PlayOneShot(errorSound);
        
    }
    public Vector2 GetPatternCounts(){
        return new Vector2(currPatternIndex-1,patternList.Length);
    }
    void PatternCompletion(){
        if(done || reseting){return;};
        done = true;
        lastPatternName = transform.GetChild(0).name.Replace("(Clone)","");
        LineDrawer.clearLines(patternSpawner.getLineDrawer());
        if(!lastWasError){
        Enemy enemy = FindObjectOfType<Enemy>();
        audioSource.PlayOneShot(completionSound);
        if(enemy){
            Vector3 spellPos = enemy.transform.position;
            spellPos.y+=30f;
            Instantiate(spell,spellPos,Quaternion.identity);
            mageObj.GetComponent<Animator>().SetTrigger("cast");
        }  
        }
        
        reseting=true;
        int childs = transform.childCount;
        for (int i = childs - 1; i >= 0; i--){GameObject.Destroy(transform.GetChild(i).gameObject,1.5f);}
        if(currPatternIndex<patternList.Length || lastWasError){
            WriteCurrPatternInfo();
           StartCoroutine(spawnPatternAfterDelay(1.75f)); 
        }else {
            StartCoroutine(Completion());
            return;
        }
        
        
    }
    IEnumerator Completion(){
        GerarRelatório();
        yield return new WaitForSeconds(2f);
        Time.timeScale=0;
        webcam.stop();
        musicAs.Stop();
        completionScreen.SetActive(true);
    }
    public void GerarRelatório(){
        WriteCurrPatternInfo();
        geradorRelatório.GerarRelatorioFinalCSV();
        geradorRelatório.saveCSV();
    }
    IEnumerator spawnPatternAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        spawnPattern();
        StopCoroutine(spawnPatternAfterDelay(delay));
    }
    void spawnPattern()
    {
        Transform child;
        int spawnIndex = lastWasError?currPatternIndex-1:currPatternIndex;
        string saveDir = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        if(!Directory.Exists(saveDir))
    {
    Directory.CreateDirectory(saveDir);
    }
        string patternPath = Path.Combine(saveDir,patternList[spawnIndex]+".csv");
            string[] patternInfo = File.ReadAllLines(patternPath);
            
            Array.Resize(ref patternInfo,patternInfo.Length-1);
            List<int[]> patternInfoList = new List<int[]>();
            foreach (string item in patternInfo)
            {
                int[] btnInfo= {int.Parse(item.Split(',')[0]),int.Parse(item.Split(',')[1])};
                patternInfoList.Add(btnInfo);
            }
            child = patternSpawner.spawnPattern(patternInfoList,patternList[spawnIndex]).transform;
            setParentTransform(child);
        if(!lastWasError){currPatternIndex++;}
        
    }

     void setParentTransform(Transform child)
    {
        int childs = child.childCount;
        for (int i = 0; i < childs; i++)
        {
            child.GetChild(0).SetParent(transform);
        };
    }

    public void newPattern(AudioClip pointS,AudioClip errorS,AudioClip completionS,bool play,float _interval){
        pointSound=pointS;
        errorSound=errorS;
        completionSound=completionS;
        interval=_interval;
        done=false;
        exampleIndex = 1;
        currentNode = 1;
        lastErrorTime = Time.time;
        
        if(play){
            StartCoroutine(iterateTroughPoints());
        }else{playing=false;};
        reseting=false;
    }
    void WriteCurrPatternInfo(){
        geradorRelatório.addContentCSV(parAcertosEErros,lastPatternName);
        parAcertosEErros = new List<KeyTime>();
    }
    public string ToCSV(List<KeyTime> pairList)
{
    string str="";
    foreach(var par in pairList)
    {
        str+= par.value.ToString()+","+par.time.ToString("F2").Replace(",",".")+";\n";
    }

    return str;
}
    void showButtonNumbers(){
            Transform child = transform.GetChild(exampleIndex);
            child.GetChild(0).GetComponent<TMP_Text>().text=exampleIndex.ToString();
    }
    void hideButtonNumbers(){
        int index = transform.childCount-1;
        while(index>0){
            Transform child = transform.GetChild(index);
            child.GetChild(0).GetComponent<TMP_Text>().text="";
            index--;
        }
    }
    IEnumerator iterateTroughPoints(){
        playing=true;
        patternExample pattern = transform.GetChild(0).GetComponent<patternExample>();
        
        while(exampleIndex<transform.childCount){
            if(exampleIndex>1){
                transform.GetChild(exampleIndex-1).GetComponent<Button>().setChecked(false);
            }
            if(PlayerPrefs.GetInt("MostrarNumeroMandalas")==1){
                showButtonNumbers();
            }
            Transform child = transform.GetChild(exampleIndex);
            child.GetComponent<Image>().color=pattern.getColors()[0];
            exampleIndex++;
            yield return new WaitForSecondsRealtime(interval);
            audioSource.PlayOneShot(pointSound);
        }
        transform.GetChild(exampleIndex-1).GetComponent<Button>().setChecked(false);
        yield return new WaitForSecondsRealtime(interval);
        if(!lastWasError){
          enemySpawner.SpawnEnemy();  
        }else{
           FindObjectOfType<Enemy>().canMove=true;
        }
        if(PlayerPrefs.GetInt("MostrarNumeroMandalas")==1){
                hideButtonNumbers();
            }
        
        playing=false;
    }
}
