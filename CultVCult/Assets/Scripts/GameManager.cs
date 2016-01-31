using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public static Dictionary<string, AudioSource> AudioSources = new Dictionary<string, AudioSource>();
    // Use this for initialization
    void Start ()
	{
		m_NewMatch = true;
		m_GameState = GameState.PreMatch;
		m_Beakers = new BeakerWrapper[m_ArrayX, m_ArrayY];

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
		    if (DidTheyDie())
		    {
                AudioSources["Growl"].Play();
		        m_GameState = GameState.GameOver;
		        UpdateScores();
		    }
		    else
		    {
		        m_GameState = GameState.PreMatch;
		        UpdateScores();
		    }
		    break;
		case GameState.GameOver:
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
		var bufferWish = m_WishList.ToList();
			Debug.Log (bufferWish.Count);
		foreach (BeakerWrapper beaker in m_Beakers)
        {
			if (beaker.m_Beaker && beaker.m_Beaker.GetComponent<Beaker>().GetColor()!= Color.white) {
				
				for(int cnt = 0; cnt < bufferWish.Count; cnt++)
				{
					if (Colors.floatToNames [bufferWish[cnt].GetComponent<SpriteRenderer> ().color] == 
						Colors.floatToNames [beaker.m_Beaker.GetComponent<Beaker> ().GetColor ()])
                    {
						bufferWish.RemoveAt(cnt);
						cnt--;
						break;
					}
				}
			}
		}

		return bufferWish.Any();			
	 }

	public void BeakerClicked(Vector2 id, GameObject Player, Color color)
	{
		m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().Mix(color);
	}

	public int HowManyColor(Color targetcolor)
	{
        int cnt = 0;
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
}
			                  
			                  


public struct BeakerWrapper
{
	public bool m_IsOccupied;
	public GameObject m_Beaker;
	public bool m_Locked;
	public int m_PlayerID; 
}