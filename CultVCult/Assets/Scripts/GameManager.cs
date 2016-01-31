using UnityEngine;
using System.Collections;
using System.Linq;

enum GameState{PreMatch,Match,OutCome, GameOver}

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
				if(Random.Range(0,100)<m_SpawnChance)
				{
					//Vector3 Pos = new Vector3( (float)((-(4.25*m_ArrayX/2)+(cntx*4.25))*m_BeakerPrefab.transform.localScale.x), (float)((-(4.9*m_ArrayY/2)+cnty*4.9))*m_BeakerPrefab.transform.localScale.y,0.0f);
					Vector3 Pos = new Vector3((float)(-8+3.5*cntx),(float)(-4+1.5*cnty+0.5),0.0f);
					m_Beakers [cntx, cnty] = new BeakerWrapper ();
					m_Beakers [cntx, cnty].m_Locked = false;
					m_Beakers [cntx, cnty].m_PlayerID = -1;
					m_Beakers [cntx, cnty].m_Beaker = (GameObject)Instantiate(m_BeakerPrefab,Pos, Quaternion.identity);
					m_Beakers [cntx, cnty].m_IsOccupied = true;
					m_Beakers [cntx, cnty].m_Beaker.GetComponent<Beaker>().SetCoords(new Vector2(cntx,cnty));
				}
			}
		}


	/*
		m_Beakers [3, 3] = new BeakerWrapper ();
		m_Beakers [3, 3].m_Locked = false;
		m_Beakers [3, 3].m_PlayerID = -1;
		m_Beakers [3, 3].m_Beaker = (GameObject)Instantiate(m_BeakerPrefab,new Vector3(1,2,0), Quaternion.identity);
		m_Beakers [3, 3].m_IsOccupied = true;
		m_Beakers [3, 3].m_Beaker.GetComponent<Beaker>().SetCoords(new Vector2(3,3));

		m_Beakers [4, 4] = new BeakerWrapper ();
		m_Beakers [4, 4].m_Locked = false;
		m_Beakers [4, 4].m_PlayerID = -1;
		m_Beakers [4, 4].m_Beaker = (GameObject)Instantiate(m_BeakerPrefab,new Vector3(-2,0,0), Quaternion.identity);
		m_Beakers [4, 4].m_IsOccupied = true;
		m_Beakers [4, 4].m_Beaker.GetComponent<Beaker>().SetCoords(new Vector2(4,4));*/
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
				ResetAllBeakers();
				ResetObjectives();
				RevealObjective();
				ResetGoals();
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
			if(!DidTheyDie())
			{
				m_GameState = GameState.PreMatch;
				Debug.Log ("we made it");
				UpdateScores();
			}
			else
			{
				m_GameState = GameState.GameOver;
				UpdateScores();
			}
			break;
		case GameState.GameOver:
			Debug.Log("You were all devoured");
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

	void ResetAllBeakers()
	{
		foreach (BeakerWrapper beaker in m_Beakers) {
			if(beaker.m_Beaker)
			{
				beaker.m_Beaker.GetComponent<Beaker>().ClearBeaker();
			}
		}
	}

	void RevealObjective()
	{
		if (m_RevealedObjectives < 3) {
			m_WishList[m_RevealedObjectives].GetComponent<SpriteRenderer>().color = Colors.GetRandomColor();
			m_RevealedObjectives++;
		}
	}

	void ResetObjectives()
	{
		foreach (GameObject wish in m_WishList) {
			wish.GetComponent<SpriteRenderer>().color = Color.black;
			m_RevealedObjectives = 0;
		}
	 }

	bool DidTheyDie()
	{
		bool first = false;
		bool second = false;
		bool third = false;
		try{

				Debug.Log (System.String.Format ("The three objectives were: 1. {0} 2. {1} 3. {2}", 
			           						Colors.floatToNames [m_WishList[0].GetComponent<SpriteRenderer> ().color],
			                                 Colors.floatToNames [m_WishList[1].GetComponent<SpriteRenderer> ().color],
			                                 Colors.floatToNames [m_WishList[2].GetComponent<SpriteRenderer> ().color]));

		var bufferWish = m_WishList.ToList();
			Debug.Log (bufferWish.Count);
		foreach (BeakerWrapper beaker in m_Beakers) {
				if (beaker.m_Beaker && beaker.m_Beaker.GetComponent<Beaker>().GetColor()!= Color.white) {
				
					for(int cnt = 0; cnt < bufferWish.Count; cnt++)
					{
						if (Colors.floatToNames [bufferWish[cnt].GetComponent<SpriteRenderer> ().color] == 
						    Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]) {
							Debug.Log( System.String.Format ("1. Location: {0}, {1}; Color: {2} with beaker colour: {3}", 
							                                 beaker.m_Beaker.GetComponent<Beaker>().GetCoords().x,
							                                 beaker.m_Beaker.GetComponent<Beaker>().GetCoords().y,
							                                 Colors.floatToNames [bufferWish[cnt].GetComponent<SpriteRenderer> ().color],
							                                 Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]));
							bufferWish.RemoveAt(cnt);
							cnt--;
							break;
						}
					}

					//foreach (GameObject wish in m_WishList) {
	



						/*
					if (!first) {
						if (Colors.floatToNames [wish.GetComponent<SpriteRenderer> ().color] == 
							Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]) {
								Debug.Log( System.String.Format ("1. Location: {0}, {1}; Color: {2} with beaker colour: {3}", 
								                                beaker.m_Beaker.GetComponent<Beaker>().GetCoords().x,
								                                beaker.m_Beaker.GetComponent<Beaker>().GetCoords().y,
								                                Colors.floatToNames [wish.GetComponent<SpriteRenderer> ().color],
								          						Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]));
							first = true;
							break;
						}
					}
				
					else if (!second) {
						if (Colors.floatToNames [wish.GetComponent<SpriteRenderer> ().color] == 
							Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]) {
								Debug.Log(System.String.Format ("2. Location: {0}, {1}; Color: {2} with beaker colour: {3}", 
								                                beaker.m_Beaker.GetComponent<Beaker>().GetCoords().x,
								                                beaker.m_Beaker.GetComponent<Beaker>().GetCoords().y,
								                                Colors.floatToNames [wish.GetComponent<SpriteRenderer> ().color],
								          Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]));							
								second = true;
							break;
						}
					}
					else if (!third) {
						if (Colors.floatToNames [wish.GetComponent<SpriteRenderer> ().color] == 
							Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]) {
								Debug.Log(System.String.Format ("3. Location: {0}, {1}; Color: {2} with beaker colour: {3}", 
								                                beaker.m_Beaker.GetComponent<Beaker>().GetCoords().x,
								                                beaker.m_Beaker.GetComponent<Beaker>().GetCoords().y,
								                                Colors.floatToNames [wish.GetComponent<SpriteRenderer> ().color],
								          Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()]));
								third = true;
							break;
						}
					}*/
				
				}
			}
			return bufferWish.Any();			
		}
		catch(System.Exception ex)
		{
			Debug.Log(ex);
			return false;
		}
		/*
		if(first&&second&&third)
		{
			return false;
		}
		else
		{
			return true;
		}*/
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
		Debug.Log (id.x+","+id.y);
		m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().Mix(color);

	}

	public int HowManyColor(Color targetcolor)
	{ int cnt = 0;
		foreach (BeakerWrapper beaker in m_Beakers) {
			if(beaker.m_Beaker &&
			   beaker.m_Beaker.GetComponent<Beaker>().GetColor()!= Color.white &&
			   Colors.floatToNames[beaker.m_Beaker.GetComponent<Beaker>().GetColor()] == Colors.floatToNames[targetcolor])
			{
				cnt++;
			}
		}
		return cnt;
	}

	private void UpdateScores()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		foreach (GameObject player in players) {
			player.GetComponent<Player>().GetScore();
		}
	}

	private void ResetGoals()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		foreach (GameObject player in players) {
			player.GetComponent<Player>().NewTarget();
		}
	}
}
			                  
			                  


public struct BeakerWrapper
{
	public bool m_IsOccupied;
	public GameObject m_Beaker;
	public bool m_Locked;
	public int m_PlayerID; 
}