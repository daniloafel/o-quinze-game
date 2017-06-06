using UnityEngine;

public class ControlladorCamera : MonoBehaviour {

	private Camera cameraPrincipal;
	private GameObject player;
	private CharacterController controlador;

	void Start () {
		cameraPrincipal = GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag ("Player");
		controlador = player.GetComponent<CharacterController>();
	}


	void Update () {
		Vector3 playerInfo = player.transform.position;
		Vector3 movimento = new Vector3 (playerInfo.x, playerInfo.y, playerInfo.z - 15);

		movimento.x = 0;
		movimento.y += 5;

		RaycastHit hit;
		if (Physics.Raycast(playerInfo, -Vector3.up, out hit)) {
			movimento.y = hit.point.y+5.0f;
		}

		cameraPrincipal.transform.position = movimento;
	}
}
