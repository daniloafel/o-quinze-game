using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour {

	public GameObject Fases;

//	public void Start(){
//		PlayerPrefs.DeleteAll ();
//	}

	public void ComecarJogo(){
		SceneManager.LoadScene(1);
	}

	public void SelecionarFase(){
		gameObject.SetActive (false);
		Fases.SetActive (true);
	}
}
