using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	const string GoButtonName = "GoButton";

	Button goButton;

	private void Awake() {
		goButton = GameObject.Find(GoButtonName).GetComponent<Button>();
	}
	void Start()
	{
		goButton.onClick.AddListener( () => OnGoClicked() );
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnGoClicked() {
		Debug.Log("Going! :D");


	}
}
