using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estrada : MonoBehaviour {


	public Transform intensObjs;
	public Transform Caminho;
	void Start(){
	}

	void Update(){
	}

	void OnDrawGizmos(){
		for (int i = 0; i < Caminho.childCount; i++) {
			Gizmos.DrawSphere(Caminho.GetChild (i).position, 1);
		}

	}
}
