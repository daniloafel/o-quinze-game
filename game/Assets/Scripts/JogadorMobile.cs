using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JogadorMobile : MonoBehaviour {
    private Vector3 movimento;

    private CharacterController controlador;

    public AudioClip item;
    public AudioClip obstaculo;

    new AudioSource audio;

    public float velocidade = 4.0f;
    public float range = 0.2f;
    private float velocidadeVertical;
    private float tempoAnimacao;


    private int proximoX;
    private int direcaoX = 0;

    private bool movendoX = false;

    private bool jogando = true;

    //Swipe

    public Text DisplayText;
    //First/Last finger position
    private Vector2 firstp;
    private Vector2 lastp;
    public float DragDistance;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        tempoAnimacao = 4.0f + Time.time;
        controlador = GetComponent<CharacterController>();
        proximoX = 0;
        velocidadeVertical = -5.0f;
    }

    void Update()
    {
        if (!jogando)
            return;
        //Tempo de animação
        if (Time.time < tempoAnimacao)
        {
            controlador.Move(Vector3.forward * velocidade * Time.deltaTime);
            return;
        }

        // definindo a movimentação
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                firstp = touch.position;
                lastp = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                lastp = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                //First check if it’s actually a drag
                if (Mathf.Abs(lastp.x - firstp.x) > DragDistance || Mathf.Abs(lastp.y - firstp.y) > DragDistance)
                { //It’s a drag
                  //Now check what direction the drag was
                  //First check which axis
                    if (Mathf.Abs(lastp.x - firstp.x) > Mathf.Abs(lastp.y - firstp.y))
                    {
                        //If the horizontal movement is greater than the vertical movement…
                        if (lastp.x > firstp.x)
                        { //Right move
                            DisplayText.text = "direita";
                            if (proximoX == 0)
                            {
                                proximoX = 2;
                            }
                            else if (proximoX == -2)
                            {
                                proximoX = 0;
                            }
                            else if (proximoX == 2)
                            {
                                proximoX = 2;
                            }
                            direcaoX = 2;
                        }
                        else
                        { //Left move
                            DisplayText.text = "esquerda";
                            if (proximoX == 0)
                            {
                                proximoX = -2;
                            }
                            else if (proximoX == 2)
                            {
                                proximoX = 0;
                            }
                            else if (proximoX == -2)
                            {
                                proximoX = -2;
                            }
                            direcaoX = -2;
                        }
                    }
                    else
                    {
                        //the vertical movement is greater than the horizontal movement
                        if (lastp.y > firstp.y)
                        {
                            //Up move
                            DisplayText.text = "cima";
                            transform.Translate(0, 1, 0);
                        }
                        else
                        {
                            //Down move
                            DisplayText.text = "baixo";
                            transform.Translate(0, -1, 0);
                        }
                    }
                }
                else
                {
                    //It’s a tap
                    DisplayText.text = "toque";
                }
            }
        }

        if (Mathf.Abs(transform.position.x - proximoX) > range)
        {
            movendoX = true;
            movimento.x = direcaoX;
        }
        else
        {
            movendoX = false;
            movimento.x = 0.0f;
        }
        //eixo y
        if (Input.GetMouseButton(1))
            movimento.y = -velocidadeVertical;
        else
            movimento.y = velocidadeVertical;
        //eixo z
        movimento.z = velocidade;
        controlador.Move(movimento * velocidade * Time.deltaTime);
    }

    private int ProximoX(int a, int b)
    {
        if (Input.mousePosition.x > Screen.width / 2)
            return a;
        else
            return b;
    }

    private void Perdeu()
    {
        jogando = false;
        GetComponent<Pontuacao>().Perdeu();
    }

    private void Ganhou()
    {
        jogando = false;
        GetComponent<Pontuacao>().Ganhou();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        int pontosAdd = getPontos(hit.gameObject.tag);
        if (pontosAdd != 0)
        {
            GetComponent<Pontuacao>().AddPontos(pontosAdd);
            Destroy(hit.gameObject);
            if (pontosAdd > 0)
                audio.PlayOneShot(item, 0.7F);
            else
                audio.PlayOneShot(obstaculo, 0.7F);
        }
        if (GetComponent<Pontuacao>().getPontuacao() <= 0)
            Perdeu();
        else if (GetComponent<Pontuacao>().getPontuacao() >= 1000.0f)
            Ganhou();
    }

    private int getPontos(string tagHit)
    {
        if (tagHit.Equals("Item5")) return 5;
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
