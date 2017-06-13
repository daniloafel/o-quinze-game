using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MovimentoPersonagem : MonoBehaviour {
	private CharacterController controlador;
	private float velocidade;
	private float gravidade;
	private Vector3 movimento;
	private bool jogando;

	//referente ao pulo
	private int contPulo;
	public int previnirPulosConsecutivos = 1;

	//referentes aos controles mobile
	private float tamanhoSwipe;
	private Vector2 ponto1;
	private Vector2 ponto2;
	private bool inputToque;

	//posições das trilhas
	private static float[] posicoesX = {-3.5f, 0f, 3.5f}; 
	//trilha atual
	private int trilha;



	void Start () {
		//começa na trilha do meio
		trilha = 1;
		velocidade = 50.0f;
		gravidade = 10.0f;
		tamanhoSwipe = 0;
		controlador = GetComponent<CharacterController> ();
		inputToque = false;
		jogando = true;
	}

	public void Update(){
		if (!jogando) {
			/***************************
			 * TO-DO:
			 * - Rodar animação parado
			 ***************************/
			return;
		}
		DetectarToques ();
		GetInputUsuario();
		
		//Move o personagem para os lados
		Vector3 v;
		v.x = posicoesX [trilha];
		v.y = controlador.transform.position.y;
		v.z = controlador.transform.position.z;
		transform.position = Vector3.MoveTowards (transform.position, v, Time.deltaTime*velocidade/2);

		//Move o personagem para frente e aplicando gravidade
		movimento.x = 0;
		movimento.y -= gravidade;
		movimento.z = velocidade;
		controlador.Move (movimento*Time.deltaTime);
	}


	private void GetInputUsuario(){

		switch (Application.platform) {
			case RuntimePlatform.Android:
			case RuntimePlatform.IPhonePlayer:
				DetectarSwipe ();
				break;
			default:
				DetectarMouse ();
				break;
		}
	}

	private void DetectarMouse(){
		
		if (Input.GetMouseButtonUp (0)) { //clique com o botão esquerdo
			if (Input.mousePosition.x > Screen.width / 2)
				MudaParaTrilhaDireita ();
			else
				MudaParaTrilhaEsquerda ();
		} else if (Input.GetMouseButtonUp (1)) { //clique com o botão direito
			if (Input.mousePosition.y > Screen.height / 2)
				Pular ();
			else
				Deslizar ();

		}
		if (!Input.GetMouseButton(1))
			contPulo++;
	}

	private void MudaParaTrilhaDireita(){
		if (trilha < 2)
			trilha++;
		
	}

	private void MudaParaTrilhaEsquerda(){
		if (trilha > 0)
			trilha--;
	}

	private void Pular(){
		if (controlador.isGrounded) {
			if (contPulo >= previnirPulosConsecutivos) {
				movimento.y = 80.0f;
				contPulo = 0;
				/***************************
				 * TO-DO:
				 * - Rodar animação de pulo
				 ***************************/
			}
		}
	}

	private void Deslizar(){
		Debug.Log ("Deslizar");
		/***************************
		 * TO-DO:
		 * - Rodar animação de deslize
		 * - Alterar o character controler
		 ***************************/
	}


	private void DetectarSwipe(){
		if (!inputToque)
			return;
		float difX = Mathf.Abs(ponto2.x - ponto1.x);
		float difY = Mathf.Abs(ponto2.y - ponto1.y);

		if (difX > difY) { // swipe horizontal
			if (ponto2.x > ponto1.x)  
				MudaParaTrilhaDireita();
			else 
				MudaParaTrilhaEsquerda();	
		}
		else { // swipe vertical
			if (ponto2.y > ponto1.y)
				Pular ();
			else
				Deslizar ();
		}
		inputToque = false;
	}

	private void DetectarToques (){
		foreach (Touch touch in Input.touches){
			if (touch.phase == TouchPhase.Began){
				ponto1 = touch.position;
				ponto2 = touch.position;
			} else if (touch.phase == TouchPhase.Moved){
				ponto2 = touch.position;
			}

			if (touch.phase == TouchPhase.Ended){
				if (Mathf.Abs(ponto2.x - ponto1.x) > tamanhoSwipe || Mathf.Abs(ponto2.y - ponto1.y) > tamanhoSwipe){
					inputToque = true;
				}
			}
		}
	}

	public void SetJogando(bool estado){
		jogando = estado;
	}
}