using UnityEngine;
using System.Collections;

public class Jogador : MonoBehaviour {

    private Vector3 movimento;
    
    private CharacterController controlador;

    private float velocidade = 4.0f;
    private float velocidadeVertical;
    private float tempoAnimacao;


    private int proximoX;
    private int direcaoX = 0;

    private bool movendoX = false;

    private bool jogando = true;

	void Start () {
        tempoAnimacao = 4.0f + Time.time;
        controlador = GetComponent<CharacterController>();
        proximoX = 0;
        velocidadeVertical = -5.0f;
	}
	
	void Update () {
        if (!jogando)
            return;
        //Tempo de animação
        if (Time.time < tempoAnimacao){
            controlador.Move(Vector3.forward * velocidade * Time.deltaTime);
            return;
        }

        // definindo a movimentação
        //eixo x
        if (!movendoX && (Input.GetMouseButton(0) || Input.touchCount > 0)){
            switch(proximoX) {
                case 2:
                    proximoX = ProximoX(2, 0);
                    break;
                case 0:
                    proximoX = ProximoX(2, -2);
                    break;
                case -2:
                    proximoX = ProximoX(0, -2);
                    break;
            }
            direcaoX = ProximoX(2,-2);
        }
        if (Mathf.Abs(transform.position.x - proximoX) > 0.2f){
            movendoX = true;
            movimento.x = direcaoX;
        }
        else{
            movendoX = false;
            movimento.x = 0.0f;
        }
        //eixo y
        if (Input.GetMouseButton(1))
            movimento.y = - velocidadeVertical;
        else
            movimento.y = velocidadeVertical;
        //eixo z
        movimento.z = velocidade;
        controlador.Move(movimento*velocidade*Time.deltaTime);

        
	}

    private int ProximoX(int a, int b){
        if (Input.mousePosition.x > Screen.width / 2)
            return a;
        else
            return b;
    }
    
    private void Perdeu() {
        jogando = false;
        GetComponent<Pontuacao>().Perdeu();
    }

    private void Ganhou(){
        jogando = false;
        GetComponent<Pontuacao>().Ganhou();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        int pontosAdd = getPontos(hit.gameObject.tag);
        if (pontosAdd != 0){
            GetComponent<Pontuacao>().AddPontos(pontosAdd);
            Destroy(hit.gameObject);
        }
        if (GetComponent<Pontuacao>().getPontuacao() <= 0)
            Perdeu();
        else if (GetComponent<Pontuacao>().getPontuacao() >= 1000.0f)
            Ganhou();
    }

    private int getPontos(string tagHit){
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

}
