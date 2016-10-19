using UnityEngine;
using System.Collections;

public abstract class JogadorBase : MonoBehaviour {
	public Vector3 movimento;

	private CharacterController controlador;

	public AudioClip item;
	public AudioClip obstaculo;
	public AudioSource audio;

	public float velocidade;
	public float range = 0.2f;
	public float velocidadePulo;
	private float tempoAnimacao;
	public float gravidade = 20.0f;

	private int contPulo;
	private int proximoX;
	private int direcaoX = 0;
	public int previnirPulosConsecutivos = 1;

	private bool jogando = true;
	private bool movendoX = false;

	void Start () {
		proximoX = 0;
		tempoAnimacao = 4.0f + Time.time;
		audio = GetComponent<AudioSource>();
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
		if (Mathf.Abs(transform.position.x - proximoX) > range){
			movendoX = true;
			movimento.x = direcaoX;
		}
		else{
			movendoX = false;
			movimento.x = 0.0f;
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
		}
		movimento.y -= gravidade * Time.deltaTime;
	}

	void Update () {
		if (!jogando)
			return;
		if (Time.time < tempoAnimacao){
			controlador.Move(Vector3.forward * velocidade * Time.deltaTime);
			return;
		}
		moverX();
		moverY ();
		movimento.z = velocidade;
		controlador.Move(movimento * velocidade*Time.deltaTime);
	}

	private int ProximoX(int a, int b){
		return (Input.mousePosition.x > Screen.width / 2) ? a : b;
	}

	public void Perdeu() {
		jogando = false;
		GetComponent<Pontuacao>().Perdeu();
	}

	public void Ganhou(){
		jogando = false;
		GetComponent<Pontuacao>().Ganhou();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		int pontosAdd = getPontos(hit.gameObject.tag);
		if (pontosAdd != 0){
			GetComponent<Pontuacao>().AddPontos(pontosAdd);
			Destroy(hit.gameObject);
			if (pontosAdd > 0) {
				audio.PlayOneShot (item, 0.7F);
			}else
				audio.PlayOneShot (obstaculo, 0.7F);
		}
		if (GetComponent<Pontuacao>().getPontuacao() <= 0)
			Perdeu();
		else if (GetComponent<Pontuacao>().getPontuacao() >= 1000.0f)
			Ganhou();
	}

	public abstract int getPontos (string tagHit);

}
