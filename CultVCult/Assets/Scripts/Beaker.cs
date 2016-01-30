using UnityEngine;
using System.Collections;

public class Beaker : MonoBehaviour {
	[SerializeField] private Color m_BeakerColor;
	private SpriteRenderer m_SpriteRenderer;
	private Vector2 m_GridCoords;
	public GameObject m_BackDrop;
	// Use this for initialization
	void Start () {
		m_BeakerColor = Color.black;
		m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		GetComponent<ParticleSystem> ().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_SpriteRenderer.color != m_BeakerColor) {
			m_SpriteRenderer.color = m_BeakerColor;
		}
	}

	public Color GetColor()
	{
		return m_BeakerColor;
	}

	public void SetColor(Color color)
	{
		m_BeakerColor = color;
	}

	public void SetCoords(Vector2 coords)
	{
		m_GridCoords = coords;
	}

	public Vector2 GetCoords()
	{
		return m_GridCoords;
	}
	public void Selected(GameObject Player, Color color)
	{
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().BeakerClicked (m_GridCoords, Player, color);
	}

	public void Select(Color color)
	{
		GetComponent<ParticleSystem> ().startColor = color;
		GetComponent<ParticleSystem> ().enableEmission = true;
		Debug.Log ("did it");
	}

	public void DeSelect()
	{
		GetComponent<ParticleSystem> ().enableEmission = false;
	}
	public void Mix(Color Mixer)
	{
		SetColor (Mixer);
	}
}
