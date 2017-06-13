using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FimDeJogo : MonoBehaviour {

    public Image imgFundo;
	public Image RadialProgress;
	public GameObject Jogador;
	public GameObject GeradorCaminho;



    private float transicao = 0.0f;
    
    void Start(){
        gameObject.SetActive(false);
    }
    
    void Update(){
        transicao += Time.deltaTime;
		imgFundo.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, transicao);
    }
        
    public void FinalizarFase(){
        gameObject.SetActive(true);
		RadialProgress.gameObject.SetActive (false);
    }

    public void Restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
