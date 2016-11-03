using UnityEngine;
using System.Collections;

public class Cabra : MonoBehaviour {

	public JogadorFase3 jogador;

	private Vector3 movimento;

	private CharacterController controlador;
	private bool pular = false;

	public float velocidade;
	public float gravidade = 20.0f;
	public float maxVelocidade = 5.5f;

	private bool jogando = true;

	void Start () {
		controlador = GetComponent<CharacterController>();
		movimento = new Vector3 ();
	}

	void Update () {
		if (!jogando)
			return;
		// definindo a movimentação
		//eixo y
		if (pular) {
			movimento.y = velocidade;
			pular = false;	
		} else {
			movimento.y = -velocidade;
		}
		//eixo z
		movimento.y -= gravidade * Time.deltaTime;
		if (transform.position.z - jogador.transform.position.z < 80.0f) {
			movimento.z = velocidade;
		} else {
			movimento.z = 0.0f;
		}
		if (jogador.velocidade < jogador.maxVelocidade)
			velocidade = jogador.velocidade;
		else
			velocidade = maxVelocidade;
		movimento.z = velocidade;
		controlador.Move(movimento*velocidade*Time.deltaTime);
	}



	private void OnControllerColliderHit(ControllerColliderHit hit) {
		string hitTag = hit.gameObject.tag;
		if (!hitTag.Equals("Caminho") && !hitTag.Equals("Player")){
			pular = true;
		}
	}


}
