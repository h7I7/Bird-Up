//\===========================================================================================
//\ File: ShopButton.cs
//\ Author: Morgan James
//\ Brief: Makes the shop buttons change color and become effective.
//\===========================================================================================

using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
	//All the different types of upgrades/unlock-ables.
	private enum Upgrade 
	{
		Shape,
		PlayerTexture,
		IslandTexture,
		BackgroundTexture,
		GravityModifyer,
		PlayerColour,
		Bouncyness,
		AmountOfJumps,
		Trail,
		Friction
	};

	private Button m_Button;
	private Image m_Image;
	private Text m_CostText;
	[SerializeField]
	private int m_BitValue;

	[SerializeField]
	private int m_Cost;

	[SerializeField]
	private Upgrade m_Upgrade;

	void Start()
	{
		m_Button = GetComponent<Button>();
		m_Image = GetComponent<Image>();
		m_CostText = transform.GetChild(1).GetChild(0).GetComponent<Text>();

		//Abbreviate the cost number so it can fit in the box if it is too large.
		if (m_Cost >= 1000000)
		{
			m_CostText.text = m_Cost/1000000 + "m";
		}
		else if ( m_Cost >= 1000)
		{
			m_CostText.text =  m_Cost/1000 + "k";
		}
		else
		{
			m_CostText.text = m_Cost + "";
		}

		//Add the on click event in accordance to the upgrade selected.
		switch (m_Upgrade)
		{
			case Upgrade.Shape:

				m_Button.onClick.AddListener(ShapeSelect);

				break;

			case Upgrade.Trail:

				m_Button.onClick.AddListener(TrailSelect);

				break;

			case Upgrade.PlayerTexture:

				m_Button.onClick.AddListener(PlayerTextureSelect);

				break;

			case Upgrade.GravityModifyer:

				m_Button.onClick.AddListener(GravitySelect);

				break;

			case Upgrade.Bouncyness:

				m_Button.onClick.AddListener(BouncinessSelect);

				break;

			case Upgrade.Friction:

				m_Button.onClick.AddListener(FrictionSelect);

				break;

			case Upgrade.BackgroundTexture:

				m_Button.onClick.AddListener(BackgroundSelect);

				break;

			case Upgrade.IslandTexture:

				m_Button.onClick.AddListener(IslandSelect);

				break;

			case Upgrade.PlayerColour:

				m_Button.onClick.AddListener(PlayerColourSelect);

				break;

			case Upgrade.AmountOfJumps:

				m_Button.onClick.AddListener(AmountOfJumpsSelect);

				break;


			default:
				break;
		}
	}

	//Change the color of the button depending on its state(locked, unlocked or unlocked and selected).
	void Update()
	{

		switch (m_Upgrade)
		{
			case Upgrade.Shape:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedShape, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedShape, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else 
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.AmountOfJumps:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedAmountOfJumps, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedAmountOfJumps, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.PlayerColour:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedPlayerColour, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedPlayerColour, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.PlayerTexture:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedPlayerTexture, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedPlayerTexture, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.GravityModifyer:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedGravityModifyer, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedGravityModifyer, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.Bouncyness:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedBouncyness, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedBouncyness, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.Friction:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedFriction, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedFriction, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.BackgroundTexture:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedBackgroundTexture, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedBackgroundTexture, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.IslandTexture:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedIslandTexture, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedIslandTexture, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			case Upgrade.Trail:

				//Selected
				if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.selectedTrail, m_BitValue))
				{
					m_Image.color = Color.green;
				}
				//Unlocked
				else if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedTrail, m_BitValue))
				{
					m_Image.color = Color.blue;
				}
				//Locked
				else
				{
					m_Image.color = Color.red;
				}

				break;

			default:
				break;
		}
		
	}


	//On click event for each upgrade that can unlock the upgrade if enough feathers are had or select the upgrade if it is unlocked.
	//Uses the save file to check what upgrade of each type is selected and unlocked.
	//Each upgrade has two integers, one for the selected and one for the unlocked.
	//The selected int has one bit turned on in accordance to what upgrade is selected.
	//The unlocked int has bits on in accordance to what is unlocked.
	void PlayerColourSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedPlayerColour, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedPlayerColour = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedPlayerColour, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedPlayerColour = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedPlayerColour, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedPlayerColour = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedPlayerColour, m_BitValue, true);
		}
	}

	void TrailSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedTrail, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedTrail = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedTrail, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedTrail = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedTrail, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedTrail = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedTrail, m_BitValue, true);
		}
	}

	void AmountOfJumpsSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedAmountOfJumps, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedAmountOfJumps = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedAmountOfJumps, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedAmountOfJumps = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedAmountOfJumps, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedAmountOfJumps = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedAmountOfJumps, m_BitValue, true);
		}
	}

	void ShapeSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedShape, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedShape = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedShape, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedShape = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedShape, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedShape = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedShape, m_BitValue, true);
		}
	}

	void PlayerTextureSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedPlayerTexture, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedPlayerTexture = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedPlayerTexture, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedPlayerTexture = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedPlayerTexture, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedPlayerTexture = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedPlayerTexture, m_BitValue, true);
		}
	}

	void GravitySelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedGravityModifyer, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedGravityModifyer = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedGravityModifyer, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedGravityModifyer = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedGravityModifyer, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedGravityModifyer = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedGravityModifyer, m_BitValue, true);

			Gravity.Instance.UpdateGravity(m_BitValue);
		}
	}

	void BouncinessSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedBouncyness, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedBouncyness = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedBouncyness, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedBouncyness = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedBouncyness, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedBouncyness = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedBouncyness, m_BitValue, true);
		}
	}

	void FrictionSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedFriction, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedFriction = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedFriction, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedFriction = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedFriction, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedFriction = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedFriction, m_BitValue, true);
		}
	}

	void BackgroundSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedBackgroundTexture, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedBackgroundTexture = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedBackgroundTexture, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedBackgroundTexture = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedBackgroundTexture, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedBackgroundTexture = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedBackgroundTexture, m_BitValue, true);
		}
	}

	void IslandSelect()
	{
		//Unlock
		if (SaveManager.Instance.CheckBit(SaveManager.Instance.state.unlockedIslandTexture, m_BitValue) == false)
		{
			if (SaveManager.Instance.state.Feathers >= m_Cost)
			{
				SaveManager.Instance.state.Feathers -= m_Cost;
				SaveManager.Instance.state.unlockedIslandTexture = SaveManager.Instance.SetBit(SaveManager.Instance.state.unlockedIslandTexture, m_BitValue, true);
			}
		}
		else
		{
			//Turn off all bits
			SaveManager.Instance.state.selectedIslandTexture = SaveManager.Instance.SetAllBits(SaveManager.Instance.state.selectedIslandTexture, false);
			//Set selected bit to be selected
			SaveManager.Instance.state.selectedIslandTexture = SaveManager.Instance.SetBit(SaveManager.Instance.state.selectedIslandTexture, m_BitValue, true);
		}
	}

}
