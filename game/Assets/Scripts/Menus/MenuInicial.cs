using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour {

	public GameObject Fases;

////	Toggle comentários para limpar os dados
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
