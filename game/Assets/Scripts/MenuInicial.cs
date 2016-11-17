using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuInicial : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	 
	// Update is called once per frame
	void Update () {
	
	}

	public void ComecarJogo(){
		SceneManager.LoadScene(6);
	}
}
