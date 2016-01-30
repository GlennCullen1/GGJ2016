using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Color m_PlayerColor;
	public GameObject m_Cursor;
	public float m_Speed = 5;
	public string m_InputName;
	bool m_Frozen;
	// Use this for initialization
	void Start () {
		m_PlayerColor = Color.magenta;
		m_Frozen = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_Frozen) {
			gameObject.GetComponent<SpriteRenderer> ().color = m_PlayerColor;
			gameObject.transform.Translate (Input.GetAxis (m_InputName + "Horizontal") * m_Speed * Time.deltaTime, Input.GetAxis (m_InputName + "Vertical") * m_Speed * Time.deltaTime, 0);
		}
	}
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Beaker") {
			if(Input.GetButtonDown(m_InputName + "Red"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Colors.GetRed());
			}
			else if(Input.GetButtonDown(m_InputName + "Blue"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Colors.GetBlue());
				Debug.Log ("hit");
			}
			else if(Input.GetButtonDown(m_InputName + "Yellow"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Colors.GetYellow());
			}
		}
	}

	public void Freeze()
	{
		m_Frozen = true;
	}

	public void UnFreeze()
	{
		m_Frozen = false;
	}
}
