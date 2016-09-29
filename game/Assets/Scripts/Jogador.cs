using UnityEngine;
using System.Collections;

public class Jogador : MonoBehaviour {

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    private Vector3 movimento;
    
    private CharacterController controlador;

    private float velocidade = 3.0f;
    private float velocidadeVertical;
    private float tempoAnimacao = 4.0f;


    private int proximoX;
    private int direcaoX = 0;
    //private int energia;

    private bool movendoX = false;
/*
    private static int maxEnergia = 1000;
    private float gravidade = 12.0f;
    */

    private bool jogando;

	void Start () {
        controlador = GetComponent<CharacterController>();
        jogando = true;
        //energia = 200;
        proximoX = 0;
        velocidadeVertical = -5.0f;
	}
	
	void Update () {
        if (!jogando)
            TerminarFase();
        //Tempo de animação
        if (Time.time < tempoAnimacao){
            controlador.Move(Vector3.forward * velocidade    * Time.deltaTime);
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
        if (Mathf.Abs(transform.position.x - proximoX) > 0.1f){
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
    
    private void TerminarFase(){

    }

    private void Perdeu() {

    }

    private void Ganhou(){
    }

}
