using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	const string GoButtonName = "GoButton";
	const string GridTrackerName = "GridTracker";

	GridTracker gridTracker;
	Button goButton;

	private void Awake() {
		gridTracker = GameObject.Find(GridTrackerName).GetComponent<GridTracker>();
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

		gridTracker.GetAllPotTiles();
		//gridTracker.removeAllPotMarkers();
		//playerController.resetAll
	}
}
