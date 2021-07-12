using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class opcoesMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text nameField;
    [SerializeField]
    Slider volumeSlider;
    [SerializeField]
    Slider musicVolSlider;
    [SerializeField]
    Slider castleHealthSlider;
    [SerializeField]
    TMP_Text castleHealthDisplay;
    [SerializeField]
    GameObject btnFacilChecked;
    [SerializeField]
    GameObject btnMedioChecked;
    [SerializeField]
    GameObject btnProtChecked;
    [SerializeField]
    GameObject btnCustomChecked;
    [SerializeField]
    TMP_Text currentSequenceText;
    [SerializeField]
    Toggle nMandalasToggle;
    [SerializeField]
    Toggle mIndicadoresToggle;
    [SerializeField]
    ConexaoBD conexao;
    Menu menu;
    private void Start() {
        menu = FindObjectOfType<Menu>();
        nMandalasToggle.isOn = PlayerPrefs.GetInt("MostrarNumeroMandalas")==1?true:false;
        mIndicadoresToggle.isOn = PlayerPrefs.GetInt("MostrarIndicadores")==1?true:false;
        castleHealthSlider.value=PlayerPrefs.GetFloat("CastleHealth");
        volumeSlider.value=PlayerPrefs.GetFloat("Volume");
        musicVolSlider.value=PlayerPrefs.GetFloat("volMusica");
    }
    // Start is called before the first frame update
    public void setDFácil(){
        PlayerPrefs.SetString("Dificuldade","fácil");
        PlayerPrefs.SetString("currentSequence","fácil");
        PlayerPrefs.Save();
        menu.playClickSound();
    }
    public void setDMédio(){
         PlayerPrefs.SetString("Dificuldade","médio");
         PlayerPrefs.SetString("currentSequence","médio");
         PlayerPrefs.Save();
         menu.playClickSound();
    }
    public void setDProtocolo(){
         PlayerPrefs.SetString("Dificuldade","protocolo");
         PlayerPrefs.SetString("currentSequence","fase1");
         PlayerPrefs.Save();
         menu.playClickSound();
    }
    public void setCustom(){
        PlayerPrefs.SetString("Dificuldade","custom");
        PlayerPrefs.Save();
         menu.playClickSound();
    }
    public void setName(){
        PlayerPrefs.SetString("PlayerName",nameField.text);
        PlayerPrefs.Save();
        menu.playClickSound();
    }
    public void downloadReport(){
        saveToDb();
    }
    public void setVolume(){
        PlayerPrefs.SetFloat("Volume",volumeSlider.value);
        PlayerPrefs.Save();
    }
    public void setVolMusica(){
        PlayerPrefs.SetFloat("volMusica",musicVolSlider.value);
        PlayerPrefs.Save();
    }
    public void setCastleHealth(){
        PlayerPrefs.SetFloat("CastleHealth",castleHealthSlider.value);
        PlayerPrefs.Save();
    }
    public void setP1Nodo(){
        PlayerPrefs.SetInt("ModoPunicao",1);
        PlayerPrefs.Save();
        menu.playClickSound();
    }
    public void setPTodos(){
        PlayerPrefs.SetInt("ModoPunicao",2);
        PlayerPrefs.Save();
        menu.playClickSound();
    }
    public void changeMostrarNMandalas(){
        PlayerPrefs.SetInt("MostrarNumeroMandalas",nMandalasToggle.isOn?1:0);
        PlayerPrefs.Save();
        if(menu){
            menu.playClickSound();
        }
    }
    public void changeMostrarIndicadores(){
        PlayerPrefs.SetInt("MostrarIndicadores",mIndicadoresToggle.isOn?1:0);
        PlayerPrefs.Save();
        if(menu){
            menu.playClickSound();
        }
    }
    public void GerarRelatorio(){
        PlayerPrefs.SetString("GerarRelatorio","sim");
        PlayerPrefs.Save();
        menu.playClickSound();
    }
    public void NGerarRelatorio(){
        PlayerPrefs.SetString("GerarRelatorio","nao");
        PlayerPrefs.Save();
        menu.playClickSound();
    }
     private void LateUpdate() {
         castleHealthDisplay.SetText(Mathf.RoundToInt(castleHealthSlider.value).ToString()+"s");
         currentSequenceText.text = "Sequência personalizada escolhida:\n"+PlayerPrefs.GetString("currentSequence");
         volumeSlider.value = PlayerPrefs.GetFloat("Volume");
         musicVolSlider.value = PlayerPrefs.GetFloat("volMusica");
        if(PlayerPrefs.GetString("Dificuldade")=="fácil"){
            btnFacilChecked.SetActive(true);
            btnMedioChecked.SetActive(false);
            btnCustomChecked.SetActive(false);
            btnProtChecked.SetActive(false);
        }else
        if(PlayerPrefs.GetString("Dificuldade")=="médio"){
            btnFacilChecked.SetActive(false);
            btnMedioChecked.SetActive(true);
            btnCustomChecked.SetActive(false);
            btnProtChecked.SetActive(false);
        }else
        if(PlayerPrefs.GetString("Dificuldade")=="protocolo"){
            btnFacilChecked.SetActive(false);
            btnMedioChecked.SetActive(false);
            btnProtChecked.SetActive(true);
            btnCustomChecked.SetActive(false);
        }else
        if(PlayerPrefs.GetString("Dificuldade")=="custom"){
            btnFacilChecked.SetActive(false);
            btnMedioChecked.SetActive(false);
            btnProtChecked.SetActive(false);
            btnCustomChecked.SetActive(true);
        }
     }
      public void saveToDb(){
        string saveDirectory = Path.Combine(Application.persistentDataPath, "DadosPatternMagic");
        if(!Directory.Exists(saveDirectory)){
            return;
        }
        conexao.PostData();
    }
}
