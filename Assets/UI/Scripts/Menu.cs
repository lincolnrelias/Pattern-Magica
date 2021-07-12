using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject optionsPanel;
    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    GameObject editPanel;
    [SerializeField]
    AudioClip btnClickSound;
    AudioSource audioSource;
    [SerializeField]
    AudioSource musicAs;
    [SerializeField]
    TMP_Text clearText;
    IEnumerator Start() {
        audioSource= GetComponent<AudioSource>();
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
        checkDificulties();
        checkPrefs();
        PlayerPrefs.Save();
        
    }
    void checkPrefs(){
        if(!PlayerPrefs.HasKey("ModoPunicao")){
            PlayerPrefs.SetInt("ModoPunicao",2);}
        if(!PlayerPrefs.HasKey("PlayerName")){
            PlayerPrefs.SetString("PlayerName","Jogador");
        }
        if(!PlayerPrefs.HasKey("CastleHealth")){
            PlayerPrefs.SetFloat("CastleHealth",90);}
        if(!PlayerPrefs.HasKey("Dificuldade")){
            PlayerPrefs.SetString("Dificuldade","protocolo");}  
        if(!PlayerPrefs.HasKey("volMusica")){
            PlayerPrefs.SetFloat("volMusica",.6f);}  
        if(!PlayerPrefs.HasKey("currentSequence")){
            PlayerPrefs.SetString("currentSequence","fase1");
        }
        if(!PlayerPrefs.HasKey("Volume")){
            PlayerPrefs.SetFloat("Volume",.7f);
        }
        if(!PlayerPrefs.HasKey("currentId")){
            PlayerPrefs.SetInt("currentId",20);
        }
        if(!PlayerPrefs.HasKey("MostrarNumeroMandalas")){
            PlayerPrefs.SetInt("MostrarNumeroMandalas",1);
        }
        if(!PlayerPrefs.HasKey("MostrarIndicadores")){
            PlayerPrefs.SetInt("MostrarIndicadores",1);
        }
    }
    void checkDificulties(){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        if (!(Directory.Exists(saveDirectory)))
            Directory.CreateDirectory(saveDirectory);

        string setDirectory = Path.Combine(Application.persistentDataPath, "sets");
        if (!(Directory.Exists(setDirectory)))
            Directory.CreateDirectory(setDirectory);
        string readEasyFilePath =Path.Combine(setDirectory, "fácil.csv");
        string fase1 =Path.Combine(setDirectory, "fase1.csv");
        string fase2 =Path.Combine(setDirectory, "fase2.csv");
        string fase3 =Path.Combine(setDirectory, "fase3.csv");
        string fase4 =Path.Combine(setDirectory, "fase4.csv");
        string fase5 =Path.Combine(setDirectory, "fase5.csv");
        string readMediumFilePath =Path.Combine(setDirectory, "médio.csv");
        WriteProtocolFiles(saveDirectory);
        WriteEasyFiles(saveDirectory);
        WriteMediumFiles(saveDirectory);
        if(!File.Exists(readEasyFilePath)){
            
            string lines = "facil1\nfacil2\nfacil3\nfacil4\nfacil5\nfacil6\nfacil7";
            File.WriteAllText(readEasyFilePath,lines);
        }
        if(!File.Exists(readMediumFilePath)){
            
            string lines = "medio1\nmedio2\nmedio3\nmedio4\nmedio5\nmedio6";
            File.WriteAllText(readMediumFilePath,lines); 
        }
        if(!File.Exists(fase1)){
            
            string lines = "protocolo1\nprotocolo1\nprotocolo1\nprotocolo1";
            File.WriteAllText(fase1,lines);
        }
        if(!File.Exists(fase2)){
            string lines = "protocolo2\nprotocolo2\nprotocolo2\nprotocolo2";
            File.WriteAllText(fase2,lines); 
        }
        if(!File.Exists(fase3)){
            string lines = "protocolo1\nprotocolo2\nprotocolo3";
            File.WriteAllText(fase3,lines); 
        }
        if(!File.Exists(fase4)){
            string lines = "protocolo4\nprotocolo5\nprotocolo6";
            File.WriteAllText(fase4,lines); 
        }
        if(!File.Exists(fase5)){
            string lines = "protocolo1\nprotocolo2\nprotocolo3\nprotocolo4\nprotocolo5\nprotocolo6";
            File.WriteAllText(fase5,lines); 
        }
        if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
    }
    void WriteEasyFiles(string path){
        string lines="";
        lines = "4,2\n4,11\n1";
        File.WriteAllText(path+"/facil1.csv",lines);
        lines = "1,3\n8,7\n1,11\n2";
        File.WriteAllText(path+"/facil2.csv",lines);
        lines = "1,3\n7,3\n7,9\n1,9\n3";
        File.WriteAllText(path+"/facil3.csv",lines);
        lines = "2,4\n6,3\n8,6\n6,9\n2,8\n4";
        File.WriteAllText(path+"/facil4.csv",lines);
        lines = "1,3\n5,2\n8,4\n8,8\n5,10\n1,9\n5";
        File.WriteAllText(path+"/facil5.csv",lines);
        lines = "1,4\n4,2\n8,3\n9,7\n8,11\n4,12\n1,10\n6";
        File.WriteAllText(path+"/facil6.csv",lines);
        lines = "1,5\n2,2\n5,2\n8,5\n8,9\n5,12\n2,12\n1,9\n7";
        File.WriteAllText(path+"/facil7.csv",lines);
        if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
    }
    void WriteMediumFiles(string path){
        string lines="";
        lines = "7,4\n7,9\n2,4\n2,9\n8";
        File.WriteAllText(path+"/medio1.csv",lines);
        lines = "1,4\n1,10\n6,11\n8,7\n6,3\n9";
        File.WriteAllText(path+"/medio2.csv",lines);
        lines = "1,2\n4,2\n2,5\n6,5\n4,7\n8,7\n6,9\n10";
        File.WriteAllText(path+"/medio3.csv",lines);
        lines = "3,1\n6,2\n7,6\n5,9\n3,10\n0,7\n5,6\n11";
        File.WriteAllText(path+"/medio4.csv",lines);
        lines = "3,3\n8,3\n5,0\n5,12\n8,9\n3,9\n12";
        File.WriteAllText(path+"/medio5.csv",lines);
        lines = "4,2\n1,4\n7,4\n9,6\n1,9\n2,11\n7,12\n9,11\n13";
        File.WriteAllText(path+"/medio6.csv",lines);
        if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
    }
    void WriteProtocolFiles(string path){
        string lines="";
        lines = "1,1\n1,12\n14";
        File.WriteAllText(path+"/protocolo1.csv",lines);
        lines = "3,1\n3,12\n15";
        File.WriteAllText(path+"/protocolo2.csv",lines);
        lines = "2,1\n2,10\n5,13\n16";
        File.WriteAllText(path+"/protocolo3.csv",lines);
        lines = "2,12\n2,3\n5,0\n17";
        File.WriteAllText(path+"/protocolo4.csv",lines);
        lines = "1,2\n1,11\n7,11\n7,2\n18";
        File.WriteAllText(path+"/protocolo5.csv",lines);
        lines = "1,3\n1,9\n5,11\n9,6\n5,1\n19";
        File.WriteAllText(path+"/protocolo6.csv",lines);
        if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
    }
    public void resetAll(){
        string patternDirectory = Path.Combine(Application.persistentDataPath, "PadroesPatternMagic");
        string saveDirectory = Path.Combine(Application.persistentDataPath, "DadosPatternMagic");
        DirectoryInfo di = new DirectoryInfo(patternDirectory);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        di = new DirectoryInfo(saveDirectory);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        PlayerPrefs.DeleteAll();
        checkDificulties();
        checkPrefs();
        StartCoroutine(Utils.showMessage(clearText,"Limpo!",3f));
        if(Application.platform == RuntimePlatform.WebGLPlayer){
        Application.ExternalCall("FS.syncfs(false, function(err) {console.log('Error: syncfs failed!');});"); 
        }
    }
    private void Update(){
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space)){
            PlayerPrefs.DeleteAll();
        }
        #endif
        musicAs.volume = PlayerPrefs.GetFloat("volMusica");
        
    }
    public void playClickSound(){
        audioSource.clip = btnClickSound;
        audioSource.Play();
    }
   public void IniciarJogo(){
        SceneManager.LoadScene("Main",LoadSceneMode.Single);
    }
    public void FecharJogo(){
        Application.Quit();
    }
    public void AbrirEditor(){
        SceneManager.LoadScene("LevelEditor");
    }
    public void patternSet(){
        editPanel.SetActive(true);
        playClickSound();
    }
    public void OpcoesAbrir(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        playClickSound();
    }
    public void OpcoesFechar(){
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        editPanel.SetActive(false);
        playClickSound();
    }
}
