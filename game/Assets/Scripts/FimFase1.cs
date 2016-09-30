using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class FimFase1 : MonoBehaviour {

    private float segProxFase = 10.0f;
    public Text contadorTexto;

    public Image imgFundo;


    private bool visivel = false;
    private float transicao = 0.0f;

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
        contadorTexto.text = ((int)segProxFase).ToString();
    }

    // Update is called once per frame
    void Update () {
        if (!visivel)
            return;
        transicao += Time.deltaTime;
        imgFundo.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transicao);

        if(segProxFase <= 0.0f){
            Debug.Log("chamou a cena");
            SceneManager.LoadScene("Fase 1 - Viagem da familia de Chico Bento para Fortaleza");
        }

        segProxFase -= Time.deltaTime;
        contadorTexto.text = ((int)segProxFase).ToString();
    }

    public void ToggleMenu(){
        gameObject.SetActive(true);
        visivel = true;
    }
}
