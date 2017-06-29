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
		int pontos = GetPontos (hit.gameObject.tag);	
		if (pontos > 0){
			pontuacao += pontos;
			energia += pontos;
			audioSource.PlayOneShot (item, 0.7F);
			Destroy(hit.gameObject);
		}else if (pontos < 0){
			pontuacao += pontos;
			energia += pontos;

			audioSource.PlayOneShot (obstaculo, 0.7F);
			multiplicador = 1;
			hit.gameObject.GetComponent<Collider> ().enabled = false;
		}
	}




	void ComputaObstaculo(string tag){
	}

	public int GetPontos(string tagHit){
		if (tagHit.Contains ("I")) {
			return int.Parse (tagHit.Substring (1, tagHit.Length - 1));
		} else if (tagHit.Contains ("O")) {
			return int.Parse (tagHit.Substring (1, tagHit.Length - 1));
		}
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
