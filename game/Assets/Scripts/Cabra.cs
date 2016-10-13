using UnityEngine;
using System.Collections;

public class Cabra : MonoBehaviour {

	private Transform jogador;

	private Vector3 movimento;

	private CharacterController controlador;
	Rigidbody rigidbody;
	private bool pular = false;

	public float velocidade = 4.0f;
	public float range = 0.2f;
	private float velocidadeVertical;

	private bool jogando = true;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		controlador = GetComponent<CharacterController>();
		jogador = GameObject.FindGameObjectWithTag("Player").transform;

	}

	void Update () {
		Debug.Log ("running");
		if (!jogando)
			return;
		// definindo a movimentação
		//eixo y
		if (pular) {
			movimento.y = velocidade;
			pular = false;
		}
		else
			movimento.y = -velocidade;
		//eixo z
		movimento.z = velocidade;
		if (transform.position.z - jogador.position.z < 80.0f){
			controlador.Move(movimento*velocidade*Time.deltaTime);
		}
	}



	private void OnControllerColliderHit(ControllerColliderHit hit) {
		string hitTag = hit.gameObject.tag;
		if (!hitTag.Equals("Caminho") && !hitTag.Equals("Player")){
			Debug.Log ("pular");
			pular = true;
		}
	}


}
