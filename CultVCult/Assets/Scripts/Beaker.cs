using UnityEngine;
using System.Collections;

public class Beaker : MonoBehaviour {
	[SerializeField] private Color m_BeakerColor;
	[SerializeField] private Sprite[] m_beakers;
	private SpriteRenderer m_SpriteRenderer;
	[SerializeField]private Vector2 m_GridCoords;
	private int m_liquidVolume = 0;
	private const int MaxLiquidVolume = 3;
	private bool bOverflowing = false;

	void Start () {
		m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		ClearBeaker ();
		m_SpriteRenderer.sprite = m_beakers[0];
		GetComponent<ParticleSystem> ().enableEmission = false;
	}
	
	void Update () 
	{
		m_SpriteRenderer.color = m_BeakerColor;
	}

	private void SetColor(Color color)
	{
			m_liquidVolume++;
			if (m_liquidVolume > MaxLiquidVolume) {
				// set to overflowing for a short bit and then 
				StartCoroutine (AssignOverflowSprite ());
			} else {
				m_SpriteRenderer.sprite = m_beakers [m_liquidVolume];
				m_BeakerColor = color;
			}
	}

	IEnumerator AssignOverflowSprite()
	{
		if (!bOverflowing) {
			bOverflowing = true;
			m_SpriteRenderer.sprite = m_beakers [4];	// 4. overflowing
			yield return new WaitForSeconds (1.5f);

			ClearBeaker ();
			bOverflowing = false;
		}
	}

	public void ClearBeaker()
	{
		m_liquidVolume = 0;
		m_BeakerColor = Color.white;
		m_SpriteRenderer.sprite = m_beakers[m_liquidVolume];

	}

	public Color GetColor()
	{
		return m_BeakerColor;
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
	}

	public void DeSelect()
	{
		GetComponent<ParticleSystem> ().enableEmission = false;
	}
	public void Mix(Color mixer)
	{
		if(m_liquidVolume == 0)
		{
			SetColor(mixer);	
		}
		else if(m_liquidVolume < MaxLiquidVolume)
		{
			SetColor(Colors.MixColors(mixer, m_BeakerColor));
		}
		else
		{
			SetColor (m_BeakerColor);	// it's going to overflow,
		}
	}
}
