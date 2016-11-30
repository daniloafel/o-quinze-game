using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public abstract class JogadorBase : MonoBehaviour {
	public Vector3 movimento;

	public Pontuacao pontuacao;
	public CharacterController controlador;
	public GeradorCaminhoBase geradorCaminho;


	public AudioClip item;
	public AudioClip obstaculo;
	public AudioSource musica;

	public float velocidade;
	public float range = 0.2f;
	public float tempoAnimacao;
	public float velocidadePulo;
	public float gravidade = 20.0f;
	public float tempoAbaixado = 0.0f;

	public int proximoX;
	private int contPulo;
	public int direcaoX = 0;
	public int previnirPulosConsecutivos = 1;

	private bool jogando = true;
	private bool movendoX = false;

	private Vector2 lastp;
	private Vector2 firstp;
	public float DragDistance;



	void Start () {
		proximoX = 0;
		tempoAnimacao = 4.0f + Time.time;
		musica = GetComponent<AudioSource>();
		controlador = GetComponent<CharacterController>();

	}

	public virtual void  moverX(){
		if (!movendoX && Input.GetMouseButton(0)){
			switch(proximoX) {
			case 2:
				proximoX = ProximoX(2, 0);
				break;
			case 0:
				proximoX = ProximoX(2, -2);
				break;
			case -2:
				proximoX = ProximoX(0, -2);
				break;
			}
			direcaoX = ProximoX(2,-2);
		}
	}

	public void moverY(){
		if (controlador.isGrounded) {
			if (!Input.GetMouseButton(1))
				contPulo++;
			else if (contPulo >= previnirPulosConsecutivos) {
				movimento.y = velocidadePulo;
				contPulo = 0;
			}
			Debug.Log ("isGrounded");
			// playerRigidbody.AddForce (new Vector3 (0.0f, velocidadePulo, 0.0f));
		}
		movimento.y -= gravidade * Time.deltaTime;

	}

	public virtual void moverXMobile (){
		if (lastp.x > firstp.x){ //Right move
			if (proximoX >= 0){
				proximoX = 2;
			}else{
				proximoX = 0;
			}
			direcaoX = 2;
		}
		else{ //Left move
			if (proximoX <= 0){
				proximoX = -2;
			}else{
				proximoX = 0;
			}
			direcaoX = -2;
		}
	}

	public void moverYMobile (){
		if (lastp.y > firstp.y) {
			movimento.y = velocidadePulo;
		}
		else{
			tempoAbaixado = 2.0f;
		}
	}

	void Update () {
		if (!jogando)
			return;
		if (Time.time < tempoAnimacao){
			controlador.Move(Vector3.forward * 3.0f * Time.deltaTime);
			return;
		}
		///weird ass behaviour
		// if (Input.GetMouseButton(1))
		// 	 playerRigidbody.AddForce (new Vector3 (0.0f, velocidadePulo, 0.0f));
		// if (Input.GetMouseButton(0))
		// 	playerRigidbody.AddForce (new Vector3 (0.0f, 0.0f, velocidade));
		// definindo a movimentação
		foreach (Touch touch in Input.touches){
			if (touch.phase == TouchPhase.Began){
				firstp = touch.position;
				lastp = touch.position;
			}
			if (touch.phase == TouchPhase.Moved){
				lastp = touch.position;
			}

			if (touch.phase == TouchPhase.Ended){
				if (Mathf.Abs(lastp.x - firstp.x) > DragDistance || Mathf.Abs(lastp.y - firstp.y) > DragDistance){
					if (Mathf.Abs(lastp.x - firstp.x) > Mathf.Abs(lastp.y - firstp.y))
						moverXMobile();
					 else 
						moverYMobile ();
				}
				else {
				}
			}
		}
		moverX();
		moverY();
		if (Mathf.Abs(transform.position.x - proximoX) > range){
			movendoX = true;
			movimento.x = direcaoX;
		}
		else{
			movendoX = false;
			movimento.x = 0.0f;
		}
		if (tempoAbaixado <= 0.0f) {
			transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			controlador.height = 2.0f;
		} else {
			controlador.height = 0.5f;
			transform.localRotation = Quaternion.Euler (new Vector3 (-90, 0, 0));
			tempoAbaixado -= Time.deltaTime;
		}
		movimento.y -= gravidade * Time.deltaTime;
		movimento.z = velocidade;
		controlador.Move(movimento * velocidade*Time.deltaTime);

		//gambis para não atravessar o plano
		//if (transform.position.y < 1.0f)
		//	movimento.y = 1.0805f;
	}

	private int ProximoX(int a, int b){
		return (Input.mousePosition.x > Screen.width / 2) ? a : b;
	}

	public void Perdeu() {
		jogando = false;
		GetComponent<Pontuacao>().Perdeu();
		Analytics.CustomEvent("Perdeu", new Dictionary<string, object>
			{	
				{ "Fase", SceneManager.GetActiveScene().name}, 
				{ "Tempo", Time.timeSinceLevelLoad }, 
				{ "Distancia", geradorCaminho.posicaoZ}, 
				{ "Velocidade", velocidade}, 
				{ "Pontuacao Inicial", pontuacao.pontuacaoInicial}
			});
	}

	public void Ganhou(){
		jogando = false;
		GetComponent<Pontuacao>().Ganhou();
		Analytics.CustomEvent("Proxima Fase", new Dictionary<string, object>
			{
				{ "Fase", SceneManager.GetActiveScene().name}, 
				{ "Tempo", Time.timeSinceLevelLoad }, 
				{ "Distancia", geradorCaminho.posicaoZ}, 
				{ "Velocidade", velocidade}, 
				{ "Pontuacao Inicial", pontuacao.pontuacaoInicial}
			});
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		int pontosAdd = getPontos(hit.gameObject.tag);
		if (pontosAdd != 0){
			GetComponent<Pontuacao>().AddPontos(pontosAdd);
			Destroy(hit.gameObject);
			if (pontosAdd > 0) {
				musica.PlayOneShot (item, 0.7F);
			}else
				musica.PlayOneShot (obstaculo, 0.7F);
		}
		if (GetComponent<Pontuacao>().getPontuacao() <= 0)
			Perdeu();
		else if (GetComponent<Pontuacao>().getPontuacao() >= 1000.0f)
			Ganhou();
	}

	public abstract int getPontos (string tagHit);

}
