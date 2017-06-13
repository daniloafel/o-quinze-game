using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanicaPontuacao : MonoBehaviour {
	private AudioSource audioSource;
	public AudioClip item;
	public AudioClip obstaculo;

	private static float energiaMaxima = 1000.0f;
	private float pontuacao;
	private float energia;

	private int multiplicador;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
		energia = 300.0f;
		pontuacao = 0.0f;
		multiplicador = 1;
	}

	void Update(){
		if (Time.timeSinceLevelLoad % 15 == 0 && multiplicador < 10) {
			multiplicador++;
		}

		pontuacao += Time.deltaTime * multiplicador;
		energia -= Time.deltaTime * 5;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag.Contains("Item")){
			ComputaItem (hit.gameObject.tag);
			Destroy(hit.gameObject);
		}else if (hit.gameObject.tag.Contains("Obstaculo")){
			ComputaObstaculo (hit.gameObject.tag);
			hit.gameObject.GetComponent<Collider> ().enabled = false;
		}
	}


	void ComputaItem(string tag){
		audioSource.PlayOneShot (item, 0.7F);
		pontuacao += GetPontos (tag);	
		energia += GetPontos (tag);
	}


	void ComputaObstaculo(string tag){
		audioSource.PlayOneShot (obstaculo, 0.7F);
		multiplicador = 1;
		pontuacao += GetPontos (tag);	
		energia += GetPontos (tag);
	}

	public int GetPontos(string tagHit){
		if (tagHit.Equals("Item5"))  return 5;
		if (tagHit.Equals("Item10")) return 10;
		if (tagHit.Equals("Item15")) return 15;
		if (tagHit.Equals("Item20")) return 20;
		if (tagHit.Equals("Item30")) return 30;
		if (tagHit.Equals("Obstaculo20")) return -20;
		if (tagHit.Equals("Obstaculo50")) return -50;
		if (tagHit.Equals("Obstaculo1000")) return -1000;
		return 0;
	}

	public float GetPontuacao(){
		return pontuacao;
	}

	public float GetEnergia(){
		return energia;
	}

	public float GetFillAmount(){
		return energia/energiaMaxima;
	}


}
