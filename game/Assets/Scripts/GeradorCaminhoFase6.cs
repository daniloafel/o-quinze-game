using UnityEngine;
using System.Collections.Generic;

public class GeradorCaminhoFase6 : GeradorCaminhoBase {
	private const int ESQ = -2;
	private const int MEIO = 0;
	private const int DIR = 2;

	private int atual = 0;
	private int maxTrilhosNaTela = 15;



	public GameObject[] trilhos;
/*
	private Transform jogadorTransform;

	public float zpos= 0.0f;
	public float posicaoZ = 0.0f;
	public float zonaSegura = 20.0f;
	public float tamanhoCaminho = 10.0f;
	

	private List<GameObject> caminhosNaTela;
*/	private List<int> selecionados;


	void Start () {
		caminhosNaTela = new List<GameObject>();
		selecionados = new List<int>();
		jogadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
		for (int i = 0; i < maxTrilhosNaTela; i++){
			if (i < 3)
				AddTrilhoInicial();
			else
				AddTrilho();
		}
	}

	void FixedUpdate () {
		if (jogadorTransform.position.z - zonaSegura > posicaoZ - maxTrilhosNaTela * tamanhoCaminho){
			AddTrilho();
			DelCaminho();
		}
	}
		


	private void AtualizaAtual(){
		int pecaAtual = selecionados [Random.Range (0, selecionados.Count)];
		switch (pecaAtual) {
			case 0:
				// SE A PEÇA ESCOLHIDA FOR A PEÇA QUE VAI PARA A DIREITA
				atual += 2;
				break;
			case 1:
				// SE A PEÇA ESCOLHIDA FOR A PEÇA QUE VAI PARA A ESQUERDA
				atual -= 2;
				break;
			case 2:
				// SE A PEÇA ESCOLHIDA FOR A PEÇA QUE CONTINUA PARA A FRENTE A POSIÇÃO NÃO MUDA
				break;
		}
	}

	private int EscolheTrilho(){
		int escolha = 2;
		switch (atual) {
		case MEIO:
			// SE ESTOU NO TRILHO DO MEIO, POSSO ADICIONAR QUALQUER TRILHO [\,|, /]
			escolha = Random.Range (0, 3);
			while (selecionados.Contains (escolha))
				escolha = Random.Range (0, 3);
			break;
		case ESQ:
			// SE ESTOU NO TRILHO MAIS A ESQUERDA, SÓ POSSO ADICIONAR UM TRILHO INDO PARA A DIREITA(trilhos[0]) OU PARA FRENTE (trilhos[2]) [/, |]
			escolha = 2*Random.Range (0, 2);
			while (selecionados.Contains (escolha))
				escolha = 2*Random.Range (0, 2);
			break;
		case DIR:
			// SE ESTOU NO TRILHO MAIS A DIREITA, SÓ POSSO ADICIONAR UM TRILHO INDO PARA A ESQUERDA(trilhos[1]) OU PARA FRENTE (trilhos[2]) [/, |]
			escolha = 1+Random.Range(0, 2);
			while (selecionados.Contains (escolha))
				escolha = 1+Random.Range (0, 2);
			break;
		}
		selecionados.Add (escolha);
		return escolha;
	}

	private int NumeroDeCaminhos(){
		if (Time.timeSinceLevelLoad < 15.0f)
			return 1;
		else if (Time.timeSinceLevelLoad < 30.0f)
			return Random.Range(1,3);
		else
			return (atual == MEIO)? Random.Range(1,4):Random.Range(1,3);
	}

	private void AddTrilho(){
		selecionados = new List<int>();
		int nCaminhos = NumeroDeCaminhos ();
		for (int i = 0; i < nCaminhos; i++) {
			int tipoTrilho = EscolheTrilho ();
			GameObject trilho;
			InstanciaPeca (tipoTrilho);
		}
		posicaoZ += tamanhoCaminho;
		AtualizaAtual ();
	}

	private void AddTrilhoInicial(){
		InstanciaPeca (2);
		posicaoZ += tamanhoCaminho;
		atual = MEIO;
	}

	private void InstanciaPeca(int index){
		GameObject trilho;
		trilho = Instantiate(trilhos[index]) as GameObject;
		trilho.transform.SetParent(transform);
		trilho.transform.position = new Vector3(atual, 0, posicaoZ);
		caminhosNaTela.Add(trilho);
	}

	private void DelCaminho(){
		Destroy(caminhosNaTela[0]);
		caminhosNaTela.RemoveAt(0);
	}
}
