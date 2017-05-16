using UnityEngine;
using System.Collections.Generic;

public class GeradorCaminhoBase : MonoBehaviour {

	public GameObject[] itens;
    public GameObject[] caminhos;
    public GameObject[] obstaculos;

	public Transform jogadorTransform;

	public float zpos= 0.0f;
	public float ultimoZ = 30.0f;
	public float posicaoZ = 0.0f;
	public float zonaSegura = 30.0f;
	public float tamanhoCaminho = 30.0f;

	public int maxObstaculos = 4;
    public int maxCaminhosNaTela = 7;

    public List<GameObject> caminhosNaTela;
	public List<GameObject> lateraisNaTela;

    void Start () {
        caminhosNaTela = new List<GameObject>();
        jogadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < maxCaminhosNaTela; i++){
            Debug.Log("entrei " + i + " vezes.");
            if (i < 3)
                AddCaminho(0);
            else
                AddCaminho();
        }

        Debug.Log(caminhosNaTela[0].gameObject.transform.localScale);

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

	private void addLaterais(){
		GameObject lateralEsq = setupPeca (-13.0f, 1);
		GameObject lateralDir = setupPeca (13.0f, 1);
		caminhosNaTela.Add (lateralEsq);
		caminhosNaTela.Add (lateralDir);
	}

    //Adiciona caminho novo na fase
    private void AddCaminho(int op = -1){

        //Define um dos tres prefabs para serem instanciados aleatoriamente
        int tipoCaminho = Random.Range(0, 5);

        if (tipoCaminho == 4)
        {
            int temp = Random.Range(0, 2);
            if (temp == 1)
            {
                tipoCaminho = 6;
            }
            else if (temp == 2)
            {
                tipoCaminho = 7;
            }
        }
        else if (tipoCaminho == 5)
        {
            int temp = Random.Range(0, 3);
            if (temp == 1)
            {
                tipoCaminho = 8;
            }
            else if (temp == 2)
            {
                tipoCaminho = 9;
            }
            else if (temp == 3)
            {
                tipoCaminho = 10;
            }
        }

        //tipoCaminho = 0;

        zpos = 0.0f;
		GameObject caminho = setupPeca (0, tipoCaminho);
//		addLaterais ();

        if(op == -1){
			float z = 2.0f;
			for(int i = 0; i < maxObstaculos; i++){
				if (Random.Range(0,2) == 0)
                	AddObjeto(caminho,z);
				z += (tamanhoCaminho) / maxObstaculos;
            }
        }
        ultimoZ = posicaoZ;
        posicaoZ += tamanhoCaminho;
        caminhosNaTela.Add(caminho);
    }

    //Adicioana os objetos aleatoriamente na estrada recebida por paramentro
	public virtual void AddObjeto(GameObject caminho, float z){
		int tipo = Random.Range (0, 2);
        GameObject objeto;
        if (tipo == 1)
            objeto = Instantiate(obstaculos[Random.Range(0,obstaculos.Length)]) as GameObject;
        else 
            objeto = Instantiate(itens[Random.Range(0,itens.Length)]) as GameObject;
        objeto.transform.SetParent(caminho.transform);
        int x = 2*Random.Range(-1, 2);
		z += ultimoZ;
        objeto.transform.position = new Vector3(x, 0, z);
        ultimoZ = z;
    }

    private void DelCaminho(){
//		for (int i = 0; i < 3; i++) {
			Destroy (caminhosNaTela [0]);
			caminhosNaTela.RemoveAt (0);
//		}
    }

}
