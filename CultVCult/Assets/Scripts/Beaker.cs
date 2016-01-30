using UnityEngine;
using System.Collections;

public class Beaker : MonoBehaviour {
	[SerializeField] private Color m_BeakerColor;
	private SpriteRenderer m_SpriteRenderer;
	[SerializeField]private Vector2 m_GridCoords;
	public GameObject m_BackDrop;
	private int m_liquidVolume = 0;
	private const int MaxLiquidVolume = 3;
	
	// Use this for initialization
	void Start () {
		// m_BeakerColor = new Color(0, 0, 0, 0);
		//m_BeakerColor = Colors.GetRed();
		//Mix(Colors.GetBlue());
		//Mix(Colors.GetRed());
		m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		GetComponent<ParticleSystem> ().enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_SpriteRenderer.color != m_BeakerColor) 
		{
			m_SpriteRenderer.color = m_BeakerColor;
		}
	}

	public Color GetColor()
	{
		return m_BeakerColor;
	}

	public void SetColor(Color color)
	{
		m_liquidVolume++;
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
		Debug.Log ("select");
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
		if(m_liquidVolume == 0)
		{
			SetColor(Mixer);	
		}
		else if(m_liquidVolume < MaxLiquidVolume)
		{
			//Debug.Log (Colors.MixColors(Mixer, m_BeakerColor).ToString());
			SetColor(Colors.MixColors(Mixer, m_BeakerColor));
		}
		else
		{
			// overflow
		}
	}
}
