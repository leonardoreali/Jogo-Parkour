using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
	public Text timerText;
	public float startTime;
	public bool finnished = false;

	void Start()
	{
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update()
	{
		if (finnished == false) { 
		float t = Time.time - startTime;

		string minutes = ((int)t / 60).ToString();
		string seconds = (t % 60).ToString("f2");

		timerText.text = minutes + ":" + seconds;
		}
	}
	public void Finnish()
	{
		finnished = true;
		timerText.color = Color.red;
	}
}
