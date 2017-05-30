using UnityEngine;
using UnityEngine.UI;

public class FPSMeter : MonoBehaviour
{
	[SerializeField]
	private Text text;
	private float time = 0f;
	private int frames = 0;

	private void Awake ()
	{
		Application.targetFrameRate = 30;
	}

	private void Update ()
	{
		time += Time.deltaTime;
		frames++;

		if (time >= 1f) {			
			text.text = Mathf.RoundToInt ((float)frames / time).ToString ();
			time = 0f;
			frames = 0;
		}
	}
}
