using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

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
	
	// Update is called once per frame
	void Update () {
        movimento = lookAt.position + startrOffset;

        //X
        movimento.x = 0;
        //Y
        movimento.y = Mathf.Clamp(movimento.y, 3, 5);

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
