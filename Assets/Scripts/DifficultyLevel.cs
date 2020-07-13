using UnityEngine;

[CreateAssetMenu(fileName = "Crabber settings", menuName = "Difficulty level", order = 1)]
public class DifficultyLevel : ScriptableObject
{
	[SerializeField]
	int numberOfRounds = 7;

	/// <summary>
	/// Represents the number of pots that the player is capable of carrying on their boat. 
	/// In case you don't know, a pot is what a crab fishing trap is called. The trap is made of metal, so it
	/// sinks to the bottom of the water bed. It's attached with a rope to a floating buoy at the surface.
	/// </summary>
	[SerializeField]
	private int startingPots = 5;

	/// <summary>
	/// How many swarms does the current scene (or level or map or lake or whatever you wish to call it) contain.
	/// </summary>
	[SerializeField]
	int numberOfSwarms = 7;

	/// <summary>
	/// Setting this to a small number has the possibility that the swarms could overlap.
	/// </summary>
	[SerializeField]
	int emptySpaceBetweenCenters = 2;

	/// <summary>
	/// Describes the concentration of crab in each crab swarm. The first number is the crab in the center of the population, 
	/// the second number is the ring around the center, and the third number is the number on the outer ring.
	/// </summary>
	[SerializeField]
	int[] populationConcentration = { 12, 2, 1 };

	[SerializeField]
	private int tripCostIncrease = 3;

	public int[] PopulationConcentration { get => populationConcentration;}
	public int EmptySpaceBetweenCenters { get => emptySpaceBetweenCenters;}
	public int NumberOfSwarms { get => numberOfSwarms;}
	public int StartingPots { get => startingPots;}
	public int NumberOfRounds { get => numberOfRounds;}
	public int TripCostIncrease { get => tripCostIncrease;}
}