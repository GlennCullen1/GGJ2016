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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BeakerClicked(Vector2 id)
	{
		if (m_Beakers [(int)id.x, (int)id.y].m_Locked != true) {
			m_Beakers [(int)id.x, (int)id.y].m_Locked = true;
			m_Beakers [(int)id.x, (int)id.y].m_PlayerID = m_LocalId;
			m_Beakers [(int)id.x, (int)id.y].m_Beaker.GetComponent<Beaker>().SetColor(Color.green);
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