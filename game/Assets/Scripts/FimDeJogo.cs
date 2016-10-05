using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FimDeJogo : MonoBehaviour {

    public Image imgFundo;


    private bool visivel = true;
    private float transicao = 0.0f;
    
    void Start(){
        gameObject.SetActive(false);
    }
    
    void Update(){
        if (!visivel)
            return;
        transicao += Time.deltaTime;
        imgFundo.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transicao);

    }
        
    public void ToggleMenu(){
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("Fase 1 - Viagem da familia de Chico Bento para Fortaleza");
    }


}
