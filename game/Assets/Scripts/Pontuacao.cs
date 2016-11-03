﻿using UnityEngine;
using UnityEngine.UI;

public class Pontuacao : MonoBehaviour {

	public float pontuacao = 0.001f;
	public float maxPontuacao = 1000.0f;


	public Slider energia;
	public FimFase fimFase;
    public Text textoPontuacao;
	public FimDeJogo fimDeJogo;

	public int timeDecay;
	public float pontuacaoInicial = 0.001f;

	public string proximaFase;

    private bool jogando = true;
    void Start(){
		fimFase.setProximaFase (proximaFase);
		pontuacao = pontuacaoInicial;
    }

	void Update () {
        if (!jogando)
            return;
        textoPontuacao.text = ((int)pontuacao).ToString();
		pontuacao -= Time.deltaTime*2 * timeDecay;
        energia.value = (pontuacao / maxPontuacao);
    }

    public void AddPontos(int pontos){
        pontuacao += pontos;
    }

    public void Perdeu(){
        jogando = false;
		energia.enabled = false;
        fimDeJogo.ToggleMenu();
    }

    public void Ganhou(){
        jogando = false;
		if (energia != null)
        	Destroy(energia.gameObject);
        fimFase.ToggleMenu();
    }

    public float getPontuacao(){
        return pontuacao;
    }
}
