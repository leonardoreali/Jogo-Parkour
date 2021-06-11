using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Timer time = other.GetComponent<Timer>();
			time.Finnish();
			GameObject.Find("Player").SendMessage("Voc� terminou esse belissimo jogo feito para o Goty");
		}
	}
}
