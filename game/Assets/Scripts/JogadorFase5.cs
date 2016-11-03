using UnityEngine;
using System.Collections;

public class JogadorFase5 : JogadorBase {

	public float maxVelocidade = 6.0f;
	public float tempoMaximo = 50.0f;
	public float posicaoConceicao = 2000.0f;

	void FixedUpdate(){
		if (velocidade < maxVelocidade) {
			velocidade += 0.2f*Time.deltaTime;
		}
		pontuacao.pontuacao = tempoMaximo - Time.timeSinceLevelLoad;

		if (Time.timeSinceLevelLoad > tempoMaximo && transform.position.z < posicaoConceicao)
			Perdeu();
		if (Time.timeSinceLevelLoad <= tempoMaximo && transform.position.z >= posicaoConceicao)
			Ganhou ();
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {

		float velocidadeRem = this.getVelocidade(hit.gameObject.tag);
		velocidade += velocidadeRem;
		if (velocidadeRem != 0.0f) {
			Destroy(hit.gameObject);
			this.GetComponent<AudioSource>().PlayOneShot (obstaculo, 0.7F);
		}
	}

	public float getVelocidade(string tagHit){
		if (tagHit.Equals("Obstaculo20")) return -0.5f;
		if (tagHit.Equals("Obstaculo50")) return -1.0f;
		if (tagHit.Equals("Obstaculo1000")) return -1.5f;

		return 0.0f;
	}

	public override int getPontos(string tagHit){
		return 1;
	}


}
