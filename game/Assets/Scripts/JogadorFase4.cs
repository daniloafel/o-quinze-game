﻿using UnityEngine;
using System.Collections;

public class JogadorFase4 : JogadorBase {

	public override void moverX(){
		movimento.x = 0;
	}


	void OnControllerColliderHit (ControllerColliderHit hit) {
		Debug.Log (hit.gameObject.tag);
	}

	public override int getPontos(string tagHit){
		return 1;
	}
} 
