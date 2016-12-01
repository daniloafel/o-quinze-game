using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MenuFases : MonoBehaviour {

	public GameObject Menu;

	public List<Button> botoesFases;

	public void Start(){
		int maxFase = PlayerPrefs.GetInt ("maxFase");
		for (int i = 0; i < maxFase; i++) {
			botoesFases [i].interactable = true;
		}
		for (int j = maxFase + 1; j < 7; j++) {
			botoesFases [j].interactable = false;
		}
	}

	public void AbrirFase(int index){
		Debug.Log ("AbrirFase" + index);
		SceneManager.LoadScene (index + 1);
	}

	public void VoltarMenuInicial(){
		gameObject.SetActive (false);
		Menu.SetActive (true);
	}

	public void BtFase1(){
		SceneManager.LoadScene (1);
	}
	public void BtFase2(){
		SceneManager.LoadScene (2);
	}
	public void BtFase3(){
		SceneManager.LoadScene (3);
	}
	public void BtFase4(){
		SceneManager.LoadScene (4);
	}
	public void BtFase5(){
		SceneManager.LoadScene (5);
	}
	public void BtFase6(){
		SceneManager.LoadScene (6);
	}
	public void BtFaseExtra(){
//		SceneManager.LoadScene (7);
	}


}
