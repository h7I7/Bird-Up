using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for all the variables we will want to store when saving the game
[Serializable]
public class SaveState
{
    // Player name for high scores
    public string Name = null;

    // Player stats
    public int Feathers = 0;
    public int HighScore = 0;

	//Player Selected abilities
	public int selectedShape = 0;
	public int selectedPlayerTexture = 0;
	public int selectedIslandTexture = 0;
	public int selectedBackgroundTexture = 0;
	public int selectedGravityModifyer = 0;
	public int selectedBouncyness = 0;
	public int selectedAmountOfJumps = 0;
	public int selectedTrail = 0;
	public int selectedFriction = 0;
	public int selectedPlayerColour = 0;

	//Player Unlocked abilities
	public int unlockedShape = 0;
	public int unlockedPlayerTexture = 0;
	public int unlockedIslandTexture = 0;
	public int unlockedBackgroundTexture = 0;
	public int unlockedGravityModifyer = 0;
	public int unlockedBouncyness = 0;
	public int unlockedAmountOfJumps = 0;
	public int unlockedTrail = 0;
	public int unlockedFriction = 0;
	public int unlockedPlayerColour = 0;
}
