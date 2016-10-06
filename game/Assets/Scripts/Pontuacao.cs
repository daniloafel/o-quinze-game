using UnityEngine;
using UnityEngine.UI;

public class Pontuacao : MonoBehaviour {

    private float pontuacao = 0.001f;
    private static float maxPontuacao = 1000.0f;


    public Text textoPontuacao;
    public Slider energia;
    public FimDeJogo fimDeJogo;
    public FimFase fimFase;
	public int timeDecay;
	public float pontuacaoInicial;

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
        Destroy(energia.gameObject);
        fimDeJogo.ToggleMenu();
    }

    public void Ganhou(){
        jogando = false;
        Destroy(energia.gameObject);
        fimFase.ToggleMenu();
    }

    public float getPontuacao(){
        return pontuacao;
    }
}
