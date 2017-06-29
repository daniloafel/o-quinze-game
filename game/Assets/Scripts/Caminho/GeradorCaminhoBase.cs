using UnityEngine;
using System.Collections.Generic;

public class GeradorCaminhoBase : MonoBehaviour {

	public GameObject[] itens;
    public GameObject[] obstaculos;

	public GameObject[] caminhos;

	public Transform jogadorTransform;

	public float zpos= 0.0f;
	public float ultimoZ = 30.0f;
	public float posicaoZ = 0.0f;
	public float zonaSegura = 30.0f;
	public float tamanhoCaminho = 30.0f;

	public int maxObstaculos = 4;
    public int maxCaminhosNaTela = 8;

    public List<GameObject> caminhosNaTela;

    void Start () {
        caminhosNaTela = new List<GameObject>();
        jogadorTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //Gerando caminhos iniciais com o mesmo padrão
        for (int i = 0; i < maxCaminhosNaTela; i++){
            if (i < maxCaminhosNaTela/2)
                AddCaminho(0);
            else
                AddCaminho();
        }
    }

    void Update () {
		if (jogadorTransform.position.z - zonaSegura > posicaoZ - maxCaminhosNaTela * tamanhoCaminho + tamanhoCaminho/2){
            AddCaminho();
            DelCaminho();
        }
    }

    //Define qual peça do Array será instanciada e retorna a peça
	private GameObject setupPeca(float x, int index){
		GameObject peca = Instantiate(caminhos[index]) as GameObject;
		peca.transform.SetParent(transform);
		peca.transform.position = new Vector3(x, 0, posicaoZ);
		return peca;
	}

    //Adiciona caminho novo na fase
    private void AddCaminho(int op = -1){
        //Define um dos tres prefabs para serem instanciados aleatoriamente
		int tipoCaminho = Random.Range(0, caminhos.Length);
		if (op != -1)
			tipoCaminho = 0;
		/*
        if (tipoCaminho == 4){
            int temp = Random.Range(0, 2);
            if (temp == 1){
                tipoCaminho = 6;
            }
            else if (temp == 2){
                tipoCaminho = 7;
            }
        }
        else if (tipoCaminho == 5){
            int temp = Random.Range(0, 3);
            if (temp == 1){
                tipoCaminho = 8;
            }
            else if (temp == 2){
                tipoCaminho = 9;
            }
            else if (temp == 3){
                tipoCaminho = 10;
            }
        }
		*/
        zpos = 0.0f;
		GameObject caminho = setupPeca (0, tipoCaminho);
        maxObstaculos = (caminho.transform.childCount / 3);
        if (op == -1){
			for(int i = 0; i < maxObstaculos; i++){
                AddObjeto(caminho, i);
            }
        }
        posicaoZ += tamanhoCaminho;
        caminhosNaTela.Add(caminho);
    }

    //Adicioana os objetos aleatoriamente na estrada recebida por paramentro
	public virtual void AddObjeto(GameObject caminho, int linha){
        GameObject objeto;
		int tipo = Random.Range (0, 2);
        if (tipo == 1){
            objeto = Instantiate(obstaculos[Random.Range(0,obstaculos.Length)]) as GameObject;
        }
        else{
            objeto = Instantiate(itens[Random.Range(0,itens.Length)]) as GameObject;
        }
        // 3 é o número de trilhas
        int col = Random.Range(0, 3);
        int childIndex = 3 * linha + col;
		Transform parentTransform = caminho.transform.GetChild(0).transform.GetChild(childIndex).transform;
        objeto.transform.SetParent(parentTransform);
        objeto.transform.position = parentTransform.position;
    }

    private void DelCaminho(){
		Destroy (caminhosNaTela [0]);
		caminhosNaTela.RemoveAt (0);
    }

}
