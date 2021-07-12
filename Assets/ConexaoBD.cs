using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class ConexaoBD : MonoBehaviour {
    static bool cr_PostDataCoroutine_running;
    // Baseado no código disponível em
    // http://wiki.unity3d.com/index.php?title=Server_Side_Highscores#C.23_-_HSController.cs

    private string secretKey = "GrupoJogosSerios"; // Edit this value and make sure it's the same as the one stored on the server
    public string addDataURL = "https://rogarpon.com.br/projetos/support/addDataGamesPOST.php"; //be sure to add a ? to your url
                                                                                             //public string highscoreURL = "http://localhost/unity_test/display.php";
    // Use this for initialization
    public string addDataPOST_URL = "https://rogarpon.com.br/projetos/support/addDataGamesPOST.php";
	
    [SerializeField]
    TMP_Text clearText;
    public void PostData()
    {
        if ((!cr_PostDataCoroutine_running))
            StartCoroutine(PostDataCoroutineNew());
    }

    public IEnumerator PostDataCoroutine()//ItemEventoDB item)//string name, int score)
    {
        cr_PostDataCoroutine_running = true;

        string NomeDoJogo = "Jogo_2_PatternMagic";
        #if UNITY_EDITOR
            NomeDoJogo += "_TEST";
        #endif

        bool falha = false;
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string[] listaItens = GeradorRelatório.getReportData().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        string[][] listao = new string[listaItens.Length][];

        for (int i = 1; i < listaItens.Length; i++)
        {
            string[] linhaAtual = listaItens[i].Split(new char[] { ';' }, StringSplitOptions.None);
                for (int j = 0; j < linhaAtual.Length; j++)
                {
                    if (linhaAtual[j] == "" || linhaAtual[j] == " ")
                        linhaAtual[j] = "*****";
                    //Debug.Log(linhaAtual[j]);
                }

                string DataHora = linhaAtual[3];
                string IdentificadorNome = linhaAtual[0];
                string NomeDoPadrao = linhaAtual[1];
                string IDPadrao = linhaAtual[3];
                string AcertoOuErro = linhaAtual[4];
                string MandalaAtingida = linhaAtual[5];
                string TempoDesdeUltimoAcertoOuErro = linhaAtual[6];
                string DistanciaInimigoCastelo = linhaAtual[7];
                string VidaInicialCastelo = linhaAtual[8];
                string VidaAtualCastelo = linhaAtual[9];
                string hash = Md5Sum(DataHora + IdentificadorNome + secretKey);

                string post_url = addDataURL + "NomeDoJogo="+UnityWebRequest.EscapeURL(NomeDoJogo)+
                    "&DataHora=" + UnityWebRequest.EscapeURL(DataHora) +
                    "&IdentificadorNome=" + UnityWebRequest.EscapeURL(IdentificadorNome) +
                    "&NomeDoPadrao=" + UnityWebRequest.EscapeURL(NomeDoPadrao) +
                    "&IDPadrao=" + UnityWebRequest.EscapeURL(IDPadrao) +
                    "&AcertoOuErro=" + UnityWebRequest.EscapeURL(AcertoOuErro) +
                    "&MandalaAtingida=" + UnityWebRequest.EscapeURL(MandalaAtingida) +
                    "&TempoDesdeUltimoAcertoOuErro=" + UnityWebRequest.EscapeURL(TempoDesdeUltimoAcertoOuErro) +
                    "&DistanciaInimigoCastelo=" + UnityWebRequest.EscapeURL(DistanciaInimigoCastelo) +
                    "&VidaInicialCastelo=" + UnityWebRequest.EscapeURL(VidaInicialCastelo) +
                    "&VidaAtualCastelo=" + UnityWebRequest.EscapeURL(VidaAtualCastelo) +
                    "&hash=" + UnityWebRequest.EscapeURL(hash);

                // Post the URL to the site and create a download object to get the result.
                UnityWebRequest hs_post = new UnityWebRequest(post_url);
                #if UNITY_EDITOR 
                    Debug.Log(post_url);
                #endif
                yield return hs_post; // Wait until the upload is done

                if (hs_post.error != null)
                {
                    falha = true;
                    Debug.Log("Houve um erro na conexão com o banco de dados: " + hs_post.error);
                }
            
        }
            
        if (!falha)
        {
            Debug.Log("Sucesso na gravação às " + System.DateTime.Now);
        }
        cr_PostDataCoroutine_running = false;
    }
public IEnumerator PostDataCoroutineNew()
    {
        cr_PostDataCoroutine_running = true;

        string NomeDoJogo = "Jogo_2_PatternMagic";
        #if UNITY_EDITOR
            NomeDoJogo += "_TEST";
        #endif

        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string lista = GeradorRelatório.getReportData();
        string[] listaItens = lista.Split('\n');
        print(lista);
        string[][] listao = new string[listaItens.Length][];
        string[] newLista = new string[0];


        for (int i = 0; i < listaItens.Length-1; i++)
        {
            print(listaItens.Length);
            string[] linhaAtual = listaItens[i].Split(new char[] { ';' }, StringSplitOptions.None);
            if (linhaAtual.Length > 1)
            {
                for (int j = 0; j < linhaAtual.Length; j++)
                {
                    if (linhaAtual[j] == "" || linhaAtual[j] == " ")
                        linhaAtual[j] = "*****";
                }
            }
            Array.Resize(ref newLista, newLista.Length + 1);
            newLista[i] = string.Join(";", linhaAtual);
        }

        string conteudoPost = string.Join("§", newLista);
        print(conteudoPost);
        var conteudoBytes = System.Text.Encoding.UTF8.GetBytes(conteudoPost);
        
        var form = new WWWForm();
        form.AddField("nomeJogo", NomeDoJogo);
        form.AddField("hash", Md5Sum(NomeDoJogo + secretKey));
        form.AddField("conteudo", conteudoPost);

        //form.AddBinaryData("Conteudo", conteudoBytes, "text/csv");

        var w = new WWW(addDataPOST_URL, form);
        yield return w;
        if (w.error != null){
              Debug.Log("Houve um erro na conexão com o banco de dados: " + w.error);
        
        }else{
            StartCoroutine(Utils.showMessage(clearText,"Enviado!",3f));
            Debug.Log("Salvo com sucesso em: "+addDataPOST_URL);
        }
          
        cr_PostDataCoroutine_running = false;
    }
    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    /*
     * IEnumerator GetScores()
    {
        gameObject.guiText.text = "Loading Scores";
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            gameObject.guiText.text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
    }
    */

    }