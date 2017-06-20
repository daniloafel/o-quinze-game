using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ControladorFase : MonoBehaviour {

	private GameObject player;
	private Image loading;
	private Image imgFundoFim;
	private MecanicaPontuacao mecanicaPontuacao;
	private MovimentoPersonagem movimentoPersonagem;
	private ControlladorCamera controladorCamera;
	private float transicao;

	private float segProxFase;


	public GameObject radialProgress;
	public GameObject cutScene;
	public GameObject menuFimDeJogo;

	void Start () {
		transicao = 0.0f;
		segProxFase = 10.0f;

		player = GameObject.FindGameObjectWithTag ("Player");
		loading = GameObject.FindGameObjectWithTag ("Loading").GetComponent<Image>();
		controladorCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<ControlladorCamera>();
		
		imgFundoFim = menuFimDeJogo.GetComponent<Image> ();
		mecanicaPontuacao = player.GetComponent<MecanicaPontuacao> ();
		movimentoPersonagem = player.GetComponent<MovimentoPersonagem> ();

		cutScene.SetActive(false);
		menuFimDeJogo.SetActive(false);
		radialProgress.SetActive(true);
	}
	
	void Update () {
		if (AtingiuObjetivo()) {
			IniciarProximaFase ();
		}else if (mecanicaPontuacao.GetEnergia () <= 0.0f) {
			JogarDeNovo ();
		}
		
		loading.fillAmount = mecanicaPontuacao.GetFillAmount ();

	}


	//modificar para qualquer que seja a condição de finalizar a fase
	bool AtingiuObjetivo(){
		return (player.transform.position.z > 10000.0);
	}

	void IniciarProximaFase(){		
		cutScene.SetActive (true);
		radialProgress.SetActive (false);
		movimentoPersonagem.SetJogando (false);
		transicao += Time.deltaTime;
//		cutScene.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transicao);

		if(segProxFase <= 0.0f){
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		segProxFase -= Time.deltaTime;
	}

	void JogarDeNovo (){
		menuFimDeJogo.SetActive (true);
		radialProgress.SetActive (false);
		movimentoPersonagem.SetJogando (false);
		transicao += Time.deltaTime;
		imgFundoFim.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, transicao);
	}

	public void Restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

}
