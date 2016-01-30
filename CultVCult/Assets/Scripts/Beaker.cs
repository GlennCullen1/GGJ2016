using UnityEngine;
using System.Collections;

public class Beaker : MonoBehaviour {
	[SerializeField] private Color m_BeakerColor;
	private SpriteRenderer m_SpriteRenderer;
	private Vector2 m_GridCoords;
	// Use this for initialization
	void Start () {
		m_BeakerColor = Color.black;
		m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
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

	void OnMouseDown()
	{
		GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().BeakerClicked (m_GridCoords);
	}
}
