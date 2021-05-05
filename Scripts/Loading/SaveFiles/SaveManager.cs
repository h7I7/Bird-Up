using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for saving and loading the save files
public class SaveManager : MonoBehaviour
{
    // An instance of this class
    public static SaveManager Instance {set; get; }

    [SerializeField]
    private bool m_deleteSaveOnLoad = false;

    [SerializeField]
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Set the instance
        if (Instance == null)
            Instance = this;

        if (m_deleteSaveOnLoad)
            Delete();
    }

    void OnApplicationQuit()
    {
        Save();
        Debug.Log("Application quit, game saved");
    }

    void OnApplicationPause()
    {
        if (Time.time > 1)
        {
            Save();
            Debug.Log("Application paused, game saved");
        }
    }

    // Saving the game
    public void Save()
    {
        PlayerPrefs.SetString("Save", SaveHelper.Encrypt(SaveHelper.Serialise(state)));
    }

    // Loading a save file
    public void Load()
    {
        // If there is already a save then read it
        if (PlayerPrefs.HasKey("Save"))
        {
            try
            {
                Debug.Log("Save file found, attempting to read...");
                state = SaveHelper.DeSerialise<SaveState>(SaveHelper.Decrypt(PlayerPrefs.GetString("Save")));
            }
            catch(System.Exception e)
            {
                Debug.Log("Error reading file, deleting corrupted save." + System.Environment.NewLine + "Error message:" + System.Environment.NewLine + e.Message);
                Delete();
                Debug.Log("Creating new save file");
                Save();
            }
        }
        // Else create a save
        else
        {
            Debug.Log("Save file not found, attempting to create...");
            state = new SaveState();
			
			state.unlockedShape = SetBit(state.unlockedShape, 0, true);
			state.selectedShape = SetBit(state.selectedShape, 0, true);

			state.selectedBouncyness = SetBit(state.selectedBouncyness, 0, true);
			state.unlockedBouncyness = SetBit(state.unlockedBouncyness, 0, true);

			state.selectedFriction = SetBit(state.selectedFriction, 0, true);
			state.unlockedFriction = SetBit(state.unlockedFriction, 0, true);

			state.selectedGravityModifyer = SetBit(state.selectedGravityModifyer, 0, true);
			state.unlockedGravityModifyer = SetBit(state.unlockedGravityModifyer, 0, true);

			state.selectedIslandTexture = SetBit(state.selectedIslandTexture, 0, true);
			state.unlockedIslandTexture = SetBit(state.unlockedIslandTexture, 0, true);

			state.selectedPlayerColour = SetBit(state.selectedPlayerColour, 0, true);
			state.unlockedPlayerColour = SetBit(state.unlockedPlayerColour, 0, true);

			state.selectedTrail = SetBit(state.selectedTrail, 0, true);
			state.unlockedTrail = SetBit(state.unlockedTrail, 0, true);

			state.selectedPlayerTexture = SetBit(state.selectedPlayerTexture, 0, true);
			state.unlockedPlayerTexture = SetBit(state.unlockedPlayerTexture, 0, true);

			state.selectedBackgroundTexture = SetBit(state.selectedBackgroundTexture, 0, true);
			state.unlockedBackgroundTexture = SetBit(state.unlockedBackgroundTexture, 0, true);

			state.unlockedAmountOfJumps = SetBit(state.unlockedAmountOfJumps, 0, true);
			state.selectedAmountOfJumps = SetBit(state.selectedAmountOfJumps, 0, true);

			Save();
        }
    }

    // Deletes the save file if there is one
    public void Delete()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            Debug.Log("DELETING SAVE!");
            PlayerPrefs.DeleteAll();
            state = new SaveState();
        }
    }

    // Check if a bit in an int is turned on
    public bool CheckBit(int a_intToCheck, int a_bitIndex)
    {
        return (a_intToCheck & (1 << a_bitIndex)) != 0;
    }

    // Set a bit inside an int variable
    public int SetBit(int a_intToSet, int a_bitIndex, bool a_OnOff)
    {
        // If we want to turn a bit on
        if (a_OnOff == true)
            a_intToSet |= 1 << a_bitIndex;
        // If we want to turn a bit off
        else
            a_intToSet ^= 1 << a_bitIndex;

		return a_intToSet;

	}

	// Set a bit inside an int variable
	public int SetAllBits(int a_intToSet, bool a_OnOff)
	{
        // If we want to turn a bit on
        if (a_OnOff == true)
            a_intToSet = ~0;
        // If we want to turn a bit off
        else
            a_intToSet = 0;

        return a_intToSet;
	}

	public int CheckBitPos(int a_intToCheck)
	{
		int size = System.Runtime.InteropServices.Marshal.SizeOf(a_intToCheck.GetType()) * 8;

		for (int i = 0; i < size; ++i)
		{
			if (CheckBit(a_intToCheck, i))
			{
				return i;
			}
		}

		return -1;
	}

	public int[] CheckAllBitPos(int a_intToCheck)
	{
		int size = System.Runtime.InteropServices.Marshal.SizeOf(a_intToCheck.GetType()) * 8;
		List<int> positions = new List<int>();


		for (int i = 0; i < size; ++i)
		{
			if (CheckBit(a_intToCheck, i))
			{
				positions.Add(i);
			}
		}

		return positions.ToArray();
	}
}