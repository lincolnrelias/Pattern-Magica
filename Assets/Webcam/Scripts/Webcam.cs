/*
	Detecção de movimento com Webcam
	Desenvolvido por Murillo Brandão
	Sob orientação do Prof. Dr. Luciano Vieira de Araújo
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Webcam : MonoBehaviour {

	/*
		PARAMETROS
		Display - RawImage para exibir o blend
		threshold - sensibilidade da detecção
		blendColor - cor de exibição da detecção
	 */
	public RawImage Display,Display2;
	[SerializeField]
	[Range(0,255)]
	int rMin = 150,rMax=255;
	[SerializeField]
	[Range(0,255)]
	int gMax = 100,gMin=0;
	[SerializeField]
	[Range(0,255)]
	int bMax = 100,bMin=0;
	[SerializeField]
	float minDiff=1f;
	
	[Range(0, 255)]
	public int threshold = 100;
	[Range(0f, 1f)]
	public float density = .02f;

	public Color32 blendColor = new Color32(255,217, 20, 20);
	Vector3 rect;
	bool countForCheck=true;
	bool initialized = false;
	WebCamTexture webcamTexture;
	Texture2D blendTexture, copyTexture;
	int diffsum = 0;
	int nullCount = 0;
	int itCounter = 1;
	Color32[] lastData = null;
	Color32[] blendData = null;
	Color32[] checkData = null;
	[SerializeField]
	TMP_Text resumeCounter;
	[SerializeField]
	GameObject counterObj;
	Pattern pattern;
	int resumeTime=3;
	

	[HideInInspector]
	public float scaleHorizontal = 1f;
	[HideInInspector]
	public float scaleVertical = 1f;
	// Escala da webcam com relação ao display
	public Text test_webcamtexture, test_tx, test_ty, test_p;



	IEnumerator Start() {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
        if( Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone) ){
			StartCoroutine(StartWebcam());
        }
		pattern=FindObjectOfType<Pattern>();

    }
	public void stop(){
		webcamTexture.Stop();
		Display.enabled=false;
	}
	public void resume(){
		StartCoroutine(displayAfterDelay());
	}
	IEnumerator displayAfterDelay(){
		Time.timeScale=0;
		countForCheck=false;
		counterObj.SetActive(true);
		int resumetimeOld = resumeTime;
		resumeCounter.text=resumeTime.ToString();
		webcamTexture.Play();
		yield return new WaitForSecondsRealtime(.5f);
		
		Display.enabled=true;
		while(resumeTime>2){
			resumeTime--;
			Debug.Log(resumeTime);
			resumeCounter.text=resumeTime.ToString();
			yield return new WaitForSecondsRealtime(1f);
		}
		resumeCounter.text=1.ToString();
		yield return new WaitForSecondsRealtime(1f);
		
		Time.timeScale=1;
		countForCheck=true;
		counterObj.SetActive(false);
		resumeTime = resumetimeOld;
		StopCoroutine(displayAfterDelay());
	}
	/*
		StartWebcam
		Inicializa a webcam e texturas
	 */
	IEnumerator StartWebcam(){
        WebCamDevice[] devices = WebCamTexture.devices;
        //test_webcamtexture.text = ("cameras " + devices.Length);
        webcamTexture = new WebCamTexture(devices[0].name,Screen.currentResolution.width/2,Screen.currentResolution.height/2,20);
        //webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        //test_webcamtexture.text = ("WebcamTexture "+webcamTexture.width + "x" + webcamTexture.height);

        int thresholdTries = 0;

        while (webcamTexture.width <= 16 && thresholdTries < 1000)
        {
            while (!webcamTexture.didUpdateThisFrame)
            {
                yield return new WaitForEndOfFrame();
            }
            webcamTexture.Pause();
            Color32[] colors = webcamTexture.GetPixels32();
            webcamTexture.Stop();

            yield return new WaitForEndOfFrame();

            webcamTexture.Play();
            thresholdTries++;
            Debug.Log(webcamTexture.width + "x" + webcamTexture.height);
        }


        blendTexture = new Texture2D( webcamTexture.width, webcamTexture.height );
        copyTexture = new Texture2D(webcamTexture.width, webcamTexture.height);

        if ( Display ){
			Display.texture = blendTexture;
			scaleHorizontal = webcamTexture.width / Display.rectTransform.rect.width;
			scaleVertical = webcamTexture.height/Display.rectTransform.rect.height;
		}

        if (Display2)
        {
            Display2.texture = copyTexture;
        }

		initialized = true;
		
	}

	/*
		UPDATE
		Todo frame calcula a diferença de frames e atualiza a textura
	 */
	void Update()
    {
		if(!webcamTexture || !webcamTexture.isPlaying){
			return;
		}
        // Aguarda inicialização e inicializa variaveis
		if (!initialized || webcamTexture.width == 0) return;
        if (lastData == null)
        {
            lastData = webcamTexture.GetPixels32();
            blendData = new Color32[lastData.Length];
			checkData = new Color32[ lastData.Length ];
            for (int i = 0; i < lastData.Length; i++)
            {
                blendData[i] = new Color32(0, 0, 0, 0);
				checkData[i] = new Color32(0, 0, 0, 0);
            }
        }
		Difference();
		 if (diffsum == 0)
        {
            if (nullCount++ < 5)  return;
        }
        else nullCount = 0;	
        


        // Atualiza textura e vetor de checagem
		for(int i=0; i<blendData.Length; i++){
			checkData[i] = blendData[i];
		}
		blendTexture.SetPixels32( blendData );
		blendTexture.Apply();
        copyTexture.SetPixels32(lastData);
        copyTexture.Apply();
    }



    /*
		DIFFERENCE
		Calcula a diferença entre o frame atual e o frame anterior
		Coloca a diferença em blendData
	*/
    void Difference(){
		if(!webcamTexture.isPlaying){
			return;
		}
		Color32[] actualData = webcamTexture.GetPixels32();
		diffsum = 0;	
		for(int i=0, len=actualData.Length; i<len; i++){
			int a = actualData[i].r - lastData[i].r;
			if( (a^a>>31)-(a>>31) > threshold && 
			(actualData[i].r>=rMin && actualData[i].g>=gMin && actualData[i].b>=bMin) &&
			(actualData[i].r<=rMax && actualData[i].g<=gMax && actualData[i].b<=bMax)){
				blendData[i] = blendColor;
				diffsum += 1;
			}
			else{
				blendData[i] = new Color(0, 0, 0, 0);
			}
		}
		lastData = actualData;
	}


	/*
		CHECKAREA
		Checa se há interação em uma região da webcam
		retorna true ou false
	 */
	bool cantCheckArea(){
		return pattern.inHitInterval() || pattern.InErrorInterval() || !countForCheck || !initialized || checkData == null; 
	}
	public bool checkArea( int x, int y, int width, int height ){
		if(cantCheckArea()) return false;
		
		int sum = 0;
		for(int i=0; i<width*height; i++){
			int tx = x + i%width;
				tx = webcamTexture.width - tx;
			int ty = y + (int)Mathf.Floor(i/width);
				ty = webcamTexture.height - ty;
			int p = (webcamTexture.width * ty) + tx;
			sum += (checkData[p].a > 0) ? 1 : 0;
		}
		float d = ((float) sum)/((float) (width*height));
		
		return ( d > density );
	}
}