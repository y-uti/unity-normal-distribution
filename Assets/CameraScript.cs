using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public float radius = 8f;
	public float height = 3f;
	public float speed  = 1f;

	void Update()
	{
		float t = Time.timeSinceLevelLoad;
		float xp = radius * Mathf.Cos(t * speed * Mathf.PI / 60f);
		float yp = height;
		float zp = radius * Mathf.Sin(t * speed * Mathf.PI / 60f);
		transform.position = new Vector3(xp, yp, zp);
		transform.LookAt(Vector3.zero);
	}
}
