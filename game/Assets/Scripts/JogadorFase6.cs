using UnityEngine;
using System.Collections;

public class JogadorFase6 : JogadorBase {


	void LateUpdate(){
		if (transform.position.y < 0.0f)
			Perdeu ();

		if (transform.position.z > 1000.0f)
			Ganhou ();
	}


	public override int getPontos(string tagHit){
		return 1;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
	}
}
