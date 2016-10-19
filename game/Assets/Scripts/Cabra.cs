using UnityEngine;
using System.Collections;

public class Cabra : MonoBehaviour {

	private Transform jogador;

	private Vector3 movimento;

	private CharacterController controlador;
	private bool pular = false;

	public float velocidade;
	public float gravidade = 20.0f;

	private bool jogando = true;

	void Start () {
		controlador = GetComponent<CharacterController>();
		jogador = GameObject.FindGameObjectWithTag("Player").transform;
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
		movimento.z = velocidade;
		if (transform.position.z - jogador.position.z < 80.0f){
			controlador.Move(movimento*velocidade*Time.deltaTime);
		}
	}



	private void OnControllerColliderHit(ControllerColliderHit hit) {
		string hitTag = hit.gameObject.tag;
		if (!hitTag.Equals("Caminho") && !hitTag.Equals("Player")){
			pular = true;
		}
	}


}
