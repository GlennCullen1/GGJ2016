using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	BeakerWrapper[,] m_Beakers;
	public int m_ArrayX, m_ArrayY;
	float m_SpawnChance = 60;
	public GameObject m_BeakerPrefab;

	public int m_LocalId;
	// Use this for initialization
	void Start ()
	{
		m_Beakers = new BeakerWrapper[m_ArrayX, m_ArrayY];
		//int blue = 2;
		//int red = 2;
		//int green = 2;

		for (int cntx = 0; cntx < m_ArrayX; cntx++) {
			for (int cnty = 0; cnty < m_ArrayY; cnty++) {
				m_Beakers [cntx, cnty].m_IsOccupied = false;
			}
		}

		m_Beakers [2, 2] = new BeakerWrapper ();
		m_Beakers [2, 2].m_Locked = false;
		m_Beakers [2, 2].m_PlayerID = -1;
		m_Beakers [2, 2].m_Beaker = (GameObject)Instantiate(m_BeakerPrefab,new Vector3(0,0,0), Quaternion.identity);
		m_Beakers [2, 2].m_IsOccupied = true;
		m_Beakers [2,2].m_Beaker.GetComponent<Beaker>().SetCoords(new Vector2(2,2));
	
		m_Beakers [3, 3] = new BeakerWrapper ();
		m_Beakers [3, 3].m_Locked = false;
		m_Beakers [3, 3].m_PlayerID = -1;
		m_Beakers [3, 3].m_Beaker = (GameObject)Instantiate(m_BeakerPrefab,new Vector3(1,2,0), Quaternion.identity);
		m_Beakers [3, 3].m_IsOccupied = true;
		m_Beakers [3, 3].m_Beaker.GetComponent<Beaker>().SetCoords(new Vector2(3,3));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BeakerClicked(Vector2 id, GameObject Player, Color color)
	{
		/*
		if (m_Beakers [(int)id.x, (int)id.y].m_Locked != true) {

			//HERE GET PLAYER ID
			int PlayerID = 1;
			m_Beakers [(int)id.x, (int)id.y].m_Locked = true;
			m_Beakers [(int)id.x, (int)id.y].m_PlayerID = PlayerID;
			m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().SetColor(Color.green);
			bool matched = false;
			foreach (BeakerWrapper obj in m_Beakers)
			{
				if (obj.m_PlayerID == PlayerID && obj.m_Beaker != m_Beakers [(int)id.x, (int)id.y].m_Beaker && !matched  )
				{
					m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().Mix(color);
					obj.m_Beaker.GetComponent<Beaker>().DeSelect();
					Vector2 loc = obj.m_Beaker.GetComponent<Beaker>().GetCoords();
					m_Beakers[(int)loc.x,(int)loc.y].m_Locked = false;
					m_Beakers [(int)id.x, (int)id.y].m_Locked = false;
					m_Beakers[(int)loc.x,(int)loc.y].m_PlayerID = 0;
					m_Beakers [(int)id.x, (int)id.y].m_PlayerID = 0;
					//obk
					matched = true;
				}
			}
			if (matched == false)
			{
				//make a player color
				Color playerC = Player.GetComponent<Player>().m_PlayerColor;
				m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().Select(playerC);
			}
		}*/
		m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().Mix(color);
	}
}

public struct BeakerWrapper
{
	public bool m_IsOccupied;
	public GameObject m_Beaker;
	public bool m_Locked;
	public int m_PlayerID; 
}