using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ProximaFase : MonoBehaviour {

    private float segProxFase = 10.0f;
    public Text contadorTexto;

	public Image RadialProgress;



	public string proximaFase;

    public Image imgFundo;


    private bool visivel = false;
    private float transicao = 0.0f;

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
        contadorTexto.text = ((int)segProxFase).ToString();
    }

    void Update () {
        if (!visivel)
            return;
        transicao += Time.deltaTime;
        imgFundo.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transicao);

        if(segProxFase <= 0.0f){
			SceneManager.LoadScene(proximaFase);
        }

        segProxFase -= Time.deltaTime;
        contadorTexto.text = ((int)segProxFase).ToString();
    }

    public void AtivarProximaFase(){
        gameObject.SetActive(true);
        visivel = true;
    }

	public void setProximaFase(string fase){
		proximaFase = fase;
	}
}
