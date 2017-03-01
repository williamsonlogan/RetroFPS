using UnityEngine;

public class Billboard : MonoBehaviour {
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt (Camera.main.transform.position, Vector3.up);
		transform.rotation = Quaternion.Euler (0.0f, transform.rotation.eulerAngles.y, 0.0f);
	}
}
