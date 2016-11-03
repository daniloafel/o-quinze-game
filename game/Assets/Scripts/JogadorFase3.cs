using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JogadorFase3 : JogadorBase {
	public float maxVelocidade = 5.55f;

	void FixedUpdate(){
		if (velocidade < maxVelocidade) {
			velocidade += 0.1f*Time.deltaTime;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag.Equals ("Cabra")) {
			Destroy(hit.gameObject);		
			Ganhou ();
		}
		float velocidadeRem = this.getVelocidade(hit.gameObject.tag);
		velocidade += velocidadeRem;
		if (velocidadeRem != 0.0f) {
			Destroy(hit.gameObject);
			this.GetComponent<AudioSource>().PlayOneShot (obstaculo, 0.7F);
		}

		if (velocidade <= 3.0f)
			this.Perdeu ();
	}

	public float getVelocidade(string tagHit){
		if (tagHit.Equals("Obstaculo20")) return -0.5f;
		if (tagHit.Equals("Obstaculo50")) return -1.5f;
		if (tagHit.Equals("Obstaculo1000")) return -velocidade;

		return 0.0f;
	}

	public override int getPontos(string tagHit){
		return 1;
	}
}
