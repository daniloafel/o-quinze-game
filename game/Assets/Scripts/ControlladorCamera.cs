using UnityEngine;

public class ControlladorCamera : MonoBehaviour {

    private Transform lookAt;

    private Vector3 startrOffset;
    private Vector3 movimento;

    private float transicao = 0.0f;
    private float tempoAnimacao = 4.0f;
    private Vector3 animationOffset = new Vector3(0, 5, 5);

    void Start () {
        transicao = 0.0f;
        tempoAnimacao = 4.0f;
        animationOffset = new Vector3(0, 5, 5);
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startrOffset = transform.position - lookAt.position;
    }
	

	void LateUpdate () {
        movimento = lookAt.position + startrOffset;

        //X
        movimento.x = 0;
        //Y
        movimento.y = Mathf.Clamp(movimento.y, 1.8f, 3);

		movimento.z -= 4.0f;
        if (transicao > 1.0f){
            transform.position = movimento;
        }
        else{
            transform.position = Vector3.Lerp(movimento + animationOffset, movimento, transicao);
            transicao += Time.deltaTime * 1 / tempoAnimacao;
            transform.LookAt(lookAt.position + Vector3.up);
        }
    }
}
