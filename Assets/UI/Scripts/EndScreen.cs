using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Webcam webcam;
    [SerializeField]
    TMP_Text patternText;
    [SerializeField]
    AudioClip startSound;
    [SerializeField]
    bool gameOver;
    [SerializeField]
    TMP_Text nextLevelButtonText;
    [SerializeField]
    FileInfo[] sequenceFiles;
    void Start()
    {
        StartCoroutine(startSequence());
    }
    IEnumerator startSequence(){
        Time.timeScale=0;
        webcam.stop();
        if(gameOver){
        Vector2 patternCounts = FindObjectOfType<Pattern>().GetPatternCounts();
        patternText.SetText("Você completou "+patternCounts[0]+" de "+patternCounts[1]+" feitiços antes de seu castelo ser destruído!");
        }
       
        
            if(PlayerPrefs.GetString("Dificuldade")=="protocolo" || PlayerPrefs.GetString("Dificuldade")=="custom"){
            string curLevel = PlayerPrefs.GetString("currentSequence");
            int nLevel=0;
            bool r = int.TryParse(curLevel.Substring(curLevel.Length-1),out nLevel);
            if(!r){
                nextLevelButtonText.text = "Recomeçar partida";
                }
            nLevel++;
            if(Application.platform == RuntimePlatform.WebGLPlayer){
            Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
            }
            yield return new WaitForSecondsRealtime(.2f);
            string nextPhase = curLevel.Substring(0,curLevel.Length-1)+nLevel;
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, @"sets"));
            sequenceFiles = dInfo.GetFiles();
            bool fExists=false;
            foreach (FileInfo item in sequenceFiles)
            {
                if(item.Name==nextPhase+".csv"){
                    fExists=true;
                }
            }
            
            if(Application.platform == RuntimePlatform.WebGLPlayer){
            Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
            }
            yield return new WaitForSecondsRealtime(.2f);
                if(fExists){
                    nextLevelButtonText.text = nextPhase;
                }else{
                    nextLevelButtonText.text = "Recomeçar partida";
                }
                
            }
        GetComponent<AudioSource>().PlayOneShot(startSound);
    }
    public void RestartGame(){
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Time.timeScale=1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public IEnumerator NextLevel(){
        if(PlayerPrefs.GetString("Dificuldade")=="protocolo" || PlayerPrefs.GetString("Dificuldade")=="custom"){
            string curLevel = PlayerPrefs.GetString("currentSequence");
            int nLevel=0;
            bool r = int.TryParse(curLevel.Substring(curLevel.Length-1),out nLevel);
            if(Application.platform == RuntimePlatform.WebGLPlayer){
            Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
            }
            yield return new WaitForSecondsRealtime(.2f);
            if(!r){
                StopCoroutine(NextLevel());}
            nLevel++;
            string curLevelName = curLevel.Substring(0,curLevel.Length-1)+1;
            string nextPhase = curLevel.Substring(0,curLevel.Length-1)+nLevel;
            bool hasPhase1 = false;
            bool hasNextPhase = false;
            foreach (FileInfo item in sequenceFiles)
            {
                if(item.Name==curLevelName+".csv"){
                    hasPhase1=true;
                }
                if(item.Name==nextPhase+".csv"){
                    hasNextPhase=true;
                }
            }
            if(Application.platform == RuntimePlatform.WebGLPlayer){
            Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
            }
            yield return new WaitForSecondsRealtime(.2f);
            if(nextLevelButtonText.text == "Recomeçar partida" && hasPhase1){
                PlayerPrefs.SetString("currentSequence",curLevelName);
            }else if(hasNextPhase){
                PlayerPrefs.SetString("currentSequence",nextPhase);
            }
            PlayerPrefs.Save();
                
        }
        RestartGame();
    }
    public void nLevel(){
        StartCoroutine(NextLevel());
    }
    public void Menu(){
        
        webcam.stop();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Time.timeScale=1;
        SceneManager.LoadScene("Menu");
    }
}
