﻿using UnityEngine;
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

	public Vector3 moveX;
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
		int fase = SceneManager.GetActiveScene ().buildIndex - 1;
		if (PlayerPrefs.GetInt ("maxFase") < fase)
			PlayerPrefs.SetInt ("maxFase", fase);

	}
	
    public virtual void  moverX(){
        //Mover com o mouse ou touch
		if (Input.GetMouseButtonUp(0)){
			switch(proximoX) {
			case 2:
				proximoX = ProximoX (2, 0);
				break;
			case 0:
				proximoX = ProximoX(2, -2);
				break;
			case -2:
				proximoX = ProximoX(0, -2);
				break;
			}
			moveX.x = proximoX;
			direcaoX = ProximoX(2,-2);
		}

        //Mover com teclado
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Não entendi como funciona o de cima
        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            //Não entendi como funciona o de cima
        }
    }

	public void moverY(){
		if (controlador.isGrounded) {
			if (!Input.GetMouseButton(1))
				contPulo++;
			else if (contPulo >= previnirPulosConsecutivos) {
				movimento.y = velocidadePulo;
				Debug.Log ("pular");
				contPulo = 0;
			}
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
		moveX.x = proximoX;
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
        //Debug.Log(movimento);
        //Camera.main.gameObject.transform.position = Vector3.MoveTowards(Camera.main.gameObject.transform.position,  movimento, 1f*Time.deltaTime);
	
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


//		if ((Mathf.Abs(transform.position.x - proximoX) >= range)){
//			movendoX = true;
//			movimento.x = direcaoX;
//		}
//		else{
//			movendoX = false;
//			movimento.x = 0.0f;
//		}
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

		moveX.y = transform.position.y;
		moveX.y = Mathf.Clamp (moveX.y, -0.01f, 100.0f);
		moveX.z = transform.position.z;

		transform.position = Vector3.Lerp(transform.position, moveX, Mathf.SmoothStep(0f,1f, Mathf.PingPong(0.35f, 1f)));
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
		else if (GetComponent<Pontuacao>().getPontuacao() >= 2000.0f)
			Ganhou();
	}

	public abstract int getPontos (string tagHit);

}