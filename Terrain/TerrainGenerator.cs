//\===========================================================================================
//\ File: TerrainGenerator.cs
//\ Author: Morgan James
//\ Brief: Generates and places platforms and entitys.
//\===========================================================================================

using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
	[Header("Game Objects")]
	[SerializeField]
	private GameObject m_Player;

	[Header("Terrain Generation Settings")]
	[SerializeField]
	private float m_SegmentDistance;
	[SerializeField]
	private float m_SpawnDistance;
	[SerializeField]
	private float m_DespawnDistance;
	[SerializeField]
	private float m_PlatformSpawnRangeHeight;
	[SerializeField]
	private float m_PlatformSpawnRangeWidth;
	[SerializeField]
	private float m_PlatformRadiusCheck;

	[Header("Check for fast terrain loading but not accurate")]
	public bool m_UseFastGeneration;

	[Header("Platform Settings")]
	public int m_PlatformWidthMin;
	public int m_PlatformWidthMax;
	public int m_PlatformHeightMin;
	public int m_PlatformHeightMax;
	public int m_PlatformFillMin;
	public int m_PlatformFillMax;

	[HideInInspector]
	public float m_Seed;

	private bool m_KeepGenerating = false;

	[System.Serializable]
	public struct Spawnable
	{
		public GameObject m_GameObject;
		public float m_Weight;
	}

	[Header("Entity's")]
	[SerializeField]
	private float m_EntityRadiusCheck;

	[SerializeField]
	private Spawnable[] m_Entity;
	private float m_TotalEntitySpawnWeight;

	void OnValidate()
	{
		if (m_Entity != null)
		{
			m_TotalEntitySpawnWeight = 0f;
			foreach (var spawnable in m_Entity)
				m_TotalEntitySpawnWeight += spawnable.m_Weight;
		}
	}

	//A segment of platforms and entitys like a chunk in Minecraft.
	class Segment
	{
		public List<GameObject> m_Platforms;
		public List<GameObject> m_Entitys;
		public Vector2 m_Position;
		public PoissonDiscSampler m_Sampler;

		public Segment(List<GameObject> a_liPlatforms, List<GameObject> a_liEntitys, Vector2 a_v2Position, PoissonDiscSampler a_Sampler)
		{
			m_Position = a_v2Position;
			m_Platforms = a_liPlatforms;
			m_Entitys = a_liEntitys;
			m_Sampler = a_Sampler;
		}
	}

	//Create a new list of segments.
	List<Segment> Segments = new List<Segment>();

	void Start()
	{
		//Set the seed to the device time.
		m_Seed = Time.time;
	}

	void Update()
	{
		//If there is no need to generate early out.
		if (!m_KeepGenerating)
			return;

		//Update the player position
		Vector2 v2PlayerPos = new Vector2(m_Player.transform.position.x, m_Player.transform.position.y);

		//Spawn a segment if it's close enough to the player and delete it if it's too far away.
		foreach(Segment segment in Segments.ToArray())
		{
			//Top spawn
			if (Vector2.Distance(v2PlayerPos, new Vector2(segment.m_Position.x, segment.m_Position.y + (m_SegmentDistance * segment.m_Sampler.rect.height))) < m_SpawnDistance)
			{
				bool bCanSpawn = true;

				foreach (Segment temp in Segments.ToArray())
				{
					if (temp.m_Position == new Vector2(segment.m_Position.x, segment.m_Position.y + (m_SegmentDistance * segment.m_Sampler.rect.height)))
					{
						bCanSpawn = false;
					}
				}

				if (bCanSpawn == true)
				{
					Generate(new Vector2(segment.m_Position.x, segment.m_Position.y + (m_SegmentDistance * segment.m_Sampler.rect.height)));
				}
			}

			//Right spawn
			if (Vector2.Distance(v2PlayerPos, new Vector2(segment.m_Position.x + (m_SegmentDistance * segment.m_Sampler.rect.width), segment.m_Position.y)) < m_SpawnDistance)
			{
				bool bCanSpawn = true;

				foreach (Segment temp in Segments.ToArray())
				{
					if (temp.m_Position == new Vector2(segment.m_Position.x + (m_SegmentDistance * segment.m_Sampler.rect.width), segment.m_Position.y))
					{
						bCanSpawn = false;
					}
				}

				if (bCanSpawn == true)
				{
					Generate(new Vector2(segment.m_Position.x + (m_SegmentDistance * segment.m_Sampler.rect.width), segment.m_Position.y));
				}
			}

			//Bottom spawn
			if (Vector2.Distance(v2PlayerPos, new Vector2(segment.m_Position.x, segment.m_Position.y - (m_SegmentDistance * segment.m_Sampler.rect.height))) < m_SpawnDistance)
			{
				bool bCanSpawn = true;

				foreach (Segment temp in Segments.ToArray())
				{
					if (temp.m_Position == new Vector2(segment.m_Position.x, segment.m_Position.y - (m_SegmentDistance * segment.m_Sampler.rect.height)))
					{
						bCanSpawn = false;
					}
				}

				if (bCanSpawn == true)
				{
					Generate(new Vector2(segment.m_Position.x, segment.m_Position.y - (m_SegmentDistance * segment.m_Sampler.rect.height)));
				}
			}

			//Left spawn
			if (Vector2.Distance(v2PlayerPos, new Vector2(segment.m_Position.x - (0.5f * segment.m_Sampler.rect.width), segment.m_Position.y)) < m_SpawnDistance)
			{
				bool bCanSpawn = true;

				foreach (Segment temp in Segments.ToArray())
				{
					if (temp.m_Position == new Vector2(segment.m_Position.x - (m_SegmentDistance * segment.m_Sampler.rect.width), segment.m_Position.y))
					{
						bCanSpawn = false;
					}
				}

				if (bCanSpawn == true)
				{
					Generate(new Vector2(segment.m_Position.x - (m_SegmentDistance * segment.m_Sampler.rect.width), segment.m_Position.y));
				}
			}

			//If a segment is too far away remove it.
			if (Vector2.Distance(v2PlayerPos, new Vector2(segment.m_Position.x, segment.m_Position.y)) > m_DespawnDistance)
			{
				foreach (GameObject enitity in segment.m_Entitys)
				{
					Destroy(enitity);
				}
				foreach (GameObject platform in segment.m_Platforms)
				{
					platform.SetActive(false);
					ObjectPooler.instance.m_PoolDictionary["Platform"].Enqueue(platform);
					Segments.Remove(segment);
				}
			}
		}
	}

	//Spawn a segment.
	void Generate(Vector2 a_position)
	{
		//Create a list of platforms and entitys of which this segment will have.
		List<GameObject> lgPlatforms = new List<GameObject>();
		List<GameObject> lgEntitys = new List<GameObject>();

		//Create a poisson disc sampler for both platforms and entitys of which to place them.
		PoissonDiscSampler pPlatformSampler = new PoissonDiscSampler(m_PlatformSpawnRangeWidth, m_PlatformSpawnRangeHeight, m_PlatformRadiusCheck);
		PoissonDiscSampler pEntitySampler = new PoissonDiscSampler(m_PlatformSpawnRangeWidth, m_PlatformSpawnRangeHeight, m_EntityRadiusCheck);

		//Spawn platforms.
		foreach (Vector2 v2Sample in pPlatformSampler.Samples())					
		{
			//Get a platform from the pool and enable and position it according to the poisson disc sampler.
			GameObject newPlatform = ObjectPooler.instance.SpawnFromPool("Platform", new Vector3(v2Sample.x + (a_position.x - (0.5f * pPlatformSampler.rect.width)), v2Sample.y + (a_position.y - (0.5f * pPlatformSampler.rect.height)), 0), Quaternion.identity);
			
			//Add the platform to the list of platforms.
			lgPlatforms.Add(newPlatform);

			//Increase the seed so that platforms don't look the same.
			m_Seed++;

		}

		foreach (Vector2 v2Sample in pEntitySampler.Samples())
		{
			//Pick a random entity based on weight.
			//Generate a random position in the list.
			float fPick = Random.value * m_TotalEntitySpawnWeight;
			int iChosenIndex = 0;
			float fCumulativeWeight = m_Entity[0].m_Weight;

			//Step through the list until we've accumulated more weight than this.
			//The length check is for safety in case rounding errors accumulate.
			while (fPick > fCumulativeWeight && iChosenIndex < m_Entity.Length - 1)
			{
				iChosenIndex++;
				fCumulativeWeight += m_Entity[iChosenIndex].m_Weight;
			}

			//Spawn and entity at a position from the poisson disc sampler.
			GameObject newEntity = Instantiate(m_Entity[iChosenIndex].m_GameObject, new Vector3(v2Sample.x + (a_position.x - (0.5f * pPlatformSampler.rect.width)), v2Sample.y + (a_position.y - (0.5f * pPlatformSampler.rect.height)), 0), Quaternion.Euler(0, 0, 0));
			newEntity.transform.parent = transform;//Set the parent.

			lgEntitys.Add(newEntity);//Add the entity to the entity list.
		}

		//Create a segment with the platform and entity list.
		Segment segment = new Segment(lgPlatforms, lgEntitys, a_position, pPlatformSampler);

		//Add the segment to the segment list.
		Segments.Add(segment);
	}

	//Add the platforms back into the pool and destroy the entitys.
	public void DeleteObjects()
	{
		foreach (Segment segment in Segments.ToArray())
		{
			//put all platforms from all segments back into the pool.
			foreach (GameObject platform in segment.m_Platforms)
			{
				platform.SetActive(false);
				ObjectPooler.instance.m_PoolDictionary["Platform"].Enqueue(platform);
			}
			//Destroy the entitys in all segments.
			foreach (GameObject entity in segment.m_Entitys)
			{
				Destroy(entity);
			}
		}
	}

	// Generate a new map
	public void Regenerate()
	{
		Generate(new Vector2(m_Player.transform.position.x, m_Player.transform.position.y));
	}

	//Keeps the map generating.
	public void StartGenerating()
	{
		m_KeepGenerating = true;
	}

	//Stops the map generating.
	public void StopGenerating()
	{
		m_KeepGenerating = false;
	}
	
}
