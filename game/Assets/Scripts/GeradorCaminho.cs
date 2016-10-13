using UnityEngine;
using System.Collections.Generic;

public class GeradorCaminho : MonoBehaviour {

    public GameObject[] caminhos;
    public GameObject[] obstaculos;
    public GameObject[] itens;

    private float ultimoZ = 30.0f;


    private Transform jogadorTransform;
    private float posicaoZ = 0.0f;
    private float tamanhoCaminho = 30.0f;
    private float zonaSegura = 20.0f;

    private int maxCaminhosNaTela = 5;

    private List<GameObject> caminhosNaTela;


    void Start () {
        caminhosNaTela = new List<GameObject>();
        jogadorTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < maxCaminhosNaTela; i++){
            if (i < 2)
                AddCaminho(0);
            else
                AddCaminho();
        }
    }

    void Update () {
        if (jogadorTransform.position.z - zonaSegura > posicaoZ - maxCaminhosNaTela * tamanhoCaminho){
            AddCaminho();
            DelCaminho();
        }
    }

    private void AddCaminho(int op = -1){
        GameObject caminho = Instantiate(caminhos[0]) as GameObject;
        caminho.transform.SetParent(transform);
        caminho.transform.position = new Vector3(0, 0, posicaoZ);

        if(op == -1){
            int numObjetos = Random.Range(0, 4);
            for(int i = 0; i < numObjetos; i++){
                AddObjeto(Random.Range(0, 2), caminho);
            }
        }
        ultimoZ = posicaoZ;
        posicaoZ += tamanhoCaminho;
        caminhosNaTela.Add(caminho);
    }

    private void AddObjeto(int tipo, GameObject caminho){
        GameObject objeto;
        if (tipo == 1)
            objeto = Instantiate(obstaculos[Random.Range(0,obstaculos.Length)]) as GameObject;
        else 
            objeto = Instantiate(itens[Random.Range(0,itens.Length)]) as GameObject;
        objeto.transform.SetParent(caminho.transform);
        int x = 2*Random.Range(-1, 2);
        float y = objeto.GetComponent<MeshFilter>().mesh.bounds.extents.y;
        float z = ultimoZ+ Random.Range(2.0f,20.0f);
        objeto.transform.position = new Vector3(x, y, z);
        ultimoZ = z;
    }

    private void DelCaminho(){
        Destroy(caminhosNaTela[0]);
        caminhosNaTela.RemoveAt(0);
    }

}
