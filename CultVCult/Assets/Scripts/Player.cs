using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour {

	public Color m_PlayerColor;
	public Color m_TargetColor;
	public GameObject m_Cursor;
	public GameObject m_DisplayTarget;
	public GameObject m_ScoreOrb;
	public float m_Speed = 5;
	public string m_InputName;
	public string m_ColorID;
	public int m_Score;
	private GameManager m_GameManager;
	bool m_Frozen;
	// Use this for initialization
	void Start () {
		m_Score = 0;
		m_GameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		//m_PlayerColor = Color.magenta;
		m_Frozen = true;
		NewTarget ();
	}
	
	// Update is called once per frame
	void Update () {

			//gameObject.GetComponent<SpriteRenderer> ().color = m_PlayerColor;
			gameObject.transform.Translate (Input.GetAxis (m_InputName + "Horizontal") * m_Speed * Time.deltaTime, Input.GetAxis (m_InputName + "Vertical") * m_Speed * Time.deltaTime, 0);

		m_DisplayTarget.GetComponent<SpriteRenderer> ().color = m_TargetColor;
	}
	void OnTriggerStay2D(Collider2D other)
	{
	if (!m_Frozen) {
		if (other.tag == "Beaker") {
			if(Input.GetButtonDown(m_InputName + "Red"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Colors.GetRed());
			}
			else if(Input.GetButtonDown(m_InputName + "Blue"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Colors.GetBlue());
			}
			else if(Input.GetButtonDown(m_InputName + "Yellow"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Colors.GetYellow());
			}
		}
	}
	}

	public void GetScore()
	{
		int numberofbeakers = m_GameManager.HowManyColor (m_TargetColor);
		float dir = -1 * Mathf.Sign (m_DisplayTarget.transform.position.x);
	

		for (int count = 1; count <= numberofbeakers; count++) {
			Vector3 pos = m_DisplayTarget.transform.position;
			pos.x += (1+(count+m_Score)*0.2f)*dir;
			Instantiate(m_ScoreOrb,pos,Quaternion.identity);
		}

		m_Score += numberofbeakers;
	}

	public void Freeze()
	{
		m_Frozen = true;
	}

	public void NewTarget()
	{
		m_TargetColor = Colors.GetRandomColor ();
	}

	public void UnFreeze()
	{
		m_Frozen = false;
	}

}
