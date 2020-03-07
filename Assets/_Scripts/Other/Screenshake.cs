using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour {

	public Transform _camera;


	public void ShakeVertical (float duration, float frequency, float amplitude) {
		StartCoroutine(ShakeCoroutineV(duration, frequency, amplitude));
	}


	public void ShakeHorizontal (float duration, float frequency, float amplitude) {
		StartCoroutine(ShakeCoroutineH(duration, frequency, amplitude));
	}


	IEnumerator ShakeCoroutineV(float duration, float frequency, float amplitude)
	{
		float timer = 0;
		while((timer+=Time.deltaTime )< duration)
		{
			_camera.localPosition = Vector3.up * Mathf.Cos(frequency * timer/duration * Mathf.PI * 2) * amplitude * (1-timer / duration);
			yield return true;
		}
		_camera.localPosition = Vector3.zero;
	}


	IEnumerator ShakeCoroutineH(float duration, float frequency, float amplitude)
	{
		float timer = 0;
		while((timer+=Time.deltaTime )< duration)
		{
			_camera.localPosition = Vector3.right * Mathf.Cos(frequency * timer/duration * Mathf.PI * 2) * amplitude * (1-timer / duration);
			yield return true;
		}
		_camera.localPosition = Vector3.zero;
	}
}