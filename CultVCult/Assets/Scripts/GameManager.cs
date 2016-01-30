using UnityEngine;
using System.Collections;

enum GameState{PreMatch,Match,OutCome}

public class GameManager : MonoBehaviour {

	BeakerWrapper[,] m_Beakers;
	public int m_ArrayX, m_ArrayY;
	float m_SpawnChance = 60;
	public GameObject m_BeakerPrefab;
	GameState m_GameState;
	public int m_LocalId;
	bool m_NewMatch;
	public float m_TimeLimit;
	public float m_TimeOnRound;
	public float m_TimeReduction;
	int m_RevealedObjectives = 0;
	public GameObject[] m_WishList;
	// Use this for initialization
	void Start ()
	{
		m_NewMatch = true;
		m_GameState = GameState.PreMatch;
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
	
		switch (m_GameState) {
		case GameState.PreMatch:
			if (Input.GetButtonDown("Start"))
			{
				m_GameState = GameState.Match;
				UnlockPlayers();
				m_NewMatch =true;
				RevealObjective();
			}
			break;
		case GameState.Match:
			if(m_NewMatch)
			{
				m_TimeOnRound = m_TimeLimit;
				m_NewMatch = false;

			}
			if(m_TimeOnRound > 0)
			{
				Debug.Log( m_TimeOnRound );
				m_TimeOnRound = m_TimeOnRound - Time.deltaTime;

				switch (m_RevealedObjectives)
				{
				case 1:
					if(m_TimeOnRound < m_TimeLimit/2)
					{
						RevealObjective();
					}
					break;
				case 2:

					if(m_TimeOnRound < m_TimeLimit/3)
					{
						RevealObjective();
					}
					break;
				case 3:
					break;
				}

			}
			else{
				m_TimeLimit = m_TimeLimit - m_TimeReduction;
				m_GameState = GameState.OutCome;
			}
			break;
		case GameState.OutCome:
			LockPlayers();
			m_GameState = GameState.PreMatch;

			break;
		}


	}
	private void UnlockPlayers()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		foreach(GameObject player in players)
		{
			player.GetComponent<Player>().UnFreeze();
		}
	}

	private void LockPlayers()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		foreach(GameObject player in players)
		{
			player.GetComponent<Player>().Freeze();
		}
	}

	void RevealObjective()
	{
		if (m_RevealedObjectives < 3) {
			m_WishList[m_RevealedObjectives].GetComponent<SpriteRenderer>().color = Color.green;
			m_RevealedObjectives++;
		}
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