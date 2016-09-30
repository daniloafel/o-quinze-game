using UnityEngine;
using UnityEngine.UI;

public class Pontuacao : MonoBehaviour {

    private float pontuacao = 0.001f;
    private static float maxPontuacao = 1000.0f;


    public Text textoPontuacao;
    public Slider energia;
    public FimDeJogo fimDeJogo;
    public FimFase1 fimFase1;
    
    private bool jogando = true;
    void Start(){
        pontuacao = 990.0f;
    }

	void Update () {
        if (!jogando)
            return;
        textoPontuacao.text = ((int)pontuacao).ToString();
        pontuacao -= Time.deltaTime;
        energia.value = (pontuacao / maxPontuacao);
        Debug.Log(energia.value);
    }

    public void AddPontos(int pontos){
        pontuacao += pontos;
    }

    public void Perdeu(){
        jogando = false;
        Destroy(energia.gameObject);
        fimDeJogo.ToggleMenu();
    }

    public void Ganhou(){
        jogando = false;
        Destroy(energia.gameObject);
        fimFase1.ToggleMenu();
    }

    public float getPontuacao(){
        return pontuacao;
    }
}
