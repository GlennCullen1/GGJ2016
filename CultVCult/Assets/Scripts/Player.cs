using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Color m_PlayerColor;
	public GameObject m_Cursor;
	public float m_Speed = 5;
	public string m_InputName;
	// Use this for initialization
	void Start () {
		m_PlayerColor = Color.magenta;
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.GetComponent<SpriteRenderer> ().color = m_PlayerColor;
		gameObject.transform.Translate (Input.GetAxis (m_InputName + "Horizontal") * m_Speed * Time.deltaTime, Input.GetAxis (m_InputName + "Vertical") * m_Speed * Time.deltaTime,0);
	}
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Beaker") {
			if(Input.GetButtonDown(m_InputName + "Red"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Color.red);
			}
			else if(Input.GetButtonDown(m_InputName + "Blue"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Color.blue);
			}
			else if(Input.GetButtonDown(m_InputName + "Yellow"))
			{
				other.gameObject.GetComponent<Beaker>().Selected(gameObject, Color.yellow);
			}
		}
	}
}
