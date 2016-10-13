using UnityEngine;
using System.Collections;

public class JogadorFase3 : MonoBehaviour {

	private Vector3 movimento;

	private CharacterController controlador;
	private Rigidbody rigibody;
	public AudioClip item;
	public AudioClip obstaculo;

	AudioSource audio;

	public float maxVelocidade = 4.0f;
	public float range = 0.2f;
	private float velocidadeVertical;
	private float tempoAnimacao;
	public float velocidade;

	private int proximoX;
	private int direcaoX = 0;

	private bool movendoX = false;

	private bool jogando = true;

	void Start () {
		audio = GetComponent<AudioSource>();
		controlador = GetComponent<CharacterController>();
		proximoX = 0;
		velocidadeVertical = -5.0f;
		velocidade = maxVelocidade;
		tempoAnimacao = 4.0f + Time.time;
	}

	void Update () {
		if (!jogando)
			return;
		//Tempo de animação
		if (Time.time < tempoAnimacao){
			controlador.Move(Vector3.forward * maxVelocidade * Time.deltaTime);
			return;
		}

		if (velocidade*velocidade < maxVelocidade*velocidade) {
			velocidade += 0.1f * Time.deltaTime;
		}
		// definindo a movimentação
		//eixo x
		if (!movendoX && (Input.GetMouseButton(0) || Input.touchCount > 0)){
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
		if (Mathf.Abs(transform.position.x - proximoX) > range){
			movendoX = true;
			movimento.x = direcaoX;
		}
		else{
			movendoX = false;
			movimento.x = 0.0f;
		}
		//eixo y
		if (Input.GetMouseButton(1))
			movimento.y = - velocidadeVertical;
		else
			movimento.y = velocidadeVertical;
		//eixo z
		movimento.z = velocidade;
		controlador.Move(movimento*velocidade*Time.deltaTime);


	}

	private int ProximoX(int a, int b){
		if (Input.mousePosition.x > Screen.width / 2)
			return a;
		else
			return b;
	}

	private void Perdeu() {
		jogando = false;
		GetComponent<Pontuacao>().Perdeu();
	}

	private void Ganhou(){
		jogando = false;
		GetComponent<Pontuacao>().Ganhou();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag.Equals ("Cabra")) {
			Destroy(hit.gameObject);		
			Ganhou ();
		}
		float velocidadeRem = getPontos(hit.gameObject.tag);
		velocidade += velocidadeRem;
		if (velocidadeRem != 0.0f) {
			Destroy(hit.gameObject);
			audio.PlayOneShot (obstaculo, 0.7F);
		}

		if (velocidade <= 0.0f)
			Perdeu ();
	}

	private float getPontos(string tagHit){
		if (tagHit.Equals("Obstaculo20")) return -0.5f;
		if (tagHit.Equals("Obstaculo50")) return -1.5f;
		if (tagHit.Equals("Obstaculo1000")) return -velocidade;

		return 0.0f;
	}
}
