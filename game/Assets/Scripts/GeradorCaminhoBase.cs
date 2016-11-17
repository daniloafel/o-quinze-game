﻿using UnityEngine;
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


    void Start () {
        caminhosNaTela = new List<GameObject>();
        jogadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < maxCaminhosNaTela; i++){
            if (i < 3)
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

    private void AddCaminho(int op = -1){
		zpos = 0.0f;
        GameObject caminho = Instantiate(caminhos[0]) as GameObject;
        caminho.transform.SetParent(transform);
        caminho.transform.position = new Vector3(0, 0, posicaoZ);

        if(op == -1){
			int numObjetos = Random.Range(0, maxObstaculos);
            for(int i = 0; i < numObjetos; i++){
                AddObjeto(caminho);
            }
        }
        ultimoZ = posicaoZ;
        posicaoZ += tamanhoCaminho;
        caminhosNaTela.Add(caminho);
    }

	public virtual void AddObjeto(GameObject caminho){
		int tipo = Random.Range (0, 2);
        GameObject objeto;
        if (tipo == 1)
            objeto = Instantiate(obstaculos[Random.Range(0,obstaculos.Length)]) as GameObject;
        else 
            objeto = Instantiate(itens[Random.Range(0,itens.Length)]) as GameObject;
        objeto.transform.SetParent(caminho.transform);
        int x = 2*Random.Range(-1, 2);
        float z = ultimoZ+ Random.Range(-14.0f,14.0f);
        objeto.transform.position = new Vector3(x, 0, z);
        ultimoZ = z;
    }

    private void DelCaminho(){
        Destroy(caminhosNaTela[0]);
        caminhosNaTela.RemoveAt(0);
    }

}
