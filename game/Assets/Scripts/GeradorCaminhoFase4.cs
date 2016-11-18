using UnityEngine;
using System.Collections.Generic;

public class GeradorCaminhoFase4 : GeradorCaminhoBase {


	public override void AddObjeto(GameObject caminho, float z){
		GameObject objeto;
		objeto = Instantiate(obstaculos[Random.Range(0,obstaculos.Length)]) as GameObject;
		objeto.transform.SetParent(caminho.transform);
		z = Random.Range(0.0f+zpos,11.0f+zpos);
		objeto.transform.position = new Vector3(0,0, z+ultimoZ+tamanhoCaminho);
		//ultimoZ = z;
		if (zpos == 0.0f)
			zpos = -11.0f;
		else
			zpos = 0.0f;
	}
}
