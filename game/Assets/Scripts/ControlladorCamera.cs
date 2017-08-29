using UnityEngine;

public class ControlladorCamera : MonoBehaviour {

	private Camera cameraPrincipal;
	private GameObject player;

//	public GameObject sol;

	void Start () {
		cameraPrincipal = GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag ("Player");
	}


	void Update () {
		Vector3 playerInfo = player.transform.position;
		Vector3 movimento = new Vector3 (playerInfo.x, playerInfo.y, playerInfo.z - 15);

		movimento.x = 0;
		movimento.y += 10;

		RaycastHit[] hits;
		hits = Physics.RaycastAll(playerInfo, -Vector3.up, 100.0F);

		for (int i = 0; i < hits.Length; i++) {
			RaycastHit hit_ = hits [i];
			if (hit_.transform.tag == "Caminho") {
				movimento.y = hit_.point.y+10.0f;
			}
		}
		cameraPrincipal.transform.position = movimento;

//		Vector3 solP = new Vector3 (sol.transform.position.x, sol.transform.position.y +50*Time.deltaTime, sol.transform.position.z +50*Time.deltaTime);
//		sol.transform.position = solP;
	}
}
