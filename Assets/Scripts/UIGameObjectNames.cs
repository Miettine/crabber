using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Crabber settings", menuName = "UI Game Object names", order = 1)]
public class UIGameObjectNames : ScriptableObject 
{
	[SerializeField]
	string roundTextGameObjectName = "RoundsText";
	[SerializeField]
	string logTextGameObjectName = "LogText";
	[SerializeField]
	string menuButtonGameObjectName = "MenuButton";
	[SerializeField]
	string quitButtonGameObjectName = "QuitButton";
	[SerializeField]
	string moneyTextGameObjectName = "MoneyText";
	[SerializeField]
	string notificationTextGameObjectName = "NotificationText";
	[SerializeField]
	string tripCostTextGameObjectName = "TripCostText";
	[SerializeField]
	string futureTripCostTextGameObjectName = "FutureTripCostText";
	[SerializeField]
	string potsLeftTextGameObjectName = "PotsLeftText";
	[SerializeField]
	string goButtonName = "GoButton";
	[SerializeField]
	string crabCollectedTextGameObjectName = "CrabCollectedText";

	public string CrabCollectedTextGameObjectName { get => crabCollectedTextGameObjectName; }
	public string GoButtonName { get => goButtonName; }
	public string PotsLeftTextGameObjectName { get => potsLeftTextGameObjectName; }
	public string FutureTripCostTextGameObjectName { get => futureTripCostTextGameObjectName; }
	public string TripCostTextGameObjectName { get => tripCostTextGameObjectName; }
	public string NotificationTextGameObjectName { get => notificationTextGameObjectName; }
	public string MoneyTextGameObjectName { get => moneyTextGameObjectName; }
	public string QuitButtonGameObjectName { get => quitButtonGameObjectName; }
	public string MenuButtonGameObjectName { get => menuButtonGameObjectName; }
	public string LogTextGameObjectName { get => logTextGameObjectName; }
	public string RoundTextGameObjectName { get => roundTextGameObjectName; }
}
