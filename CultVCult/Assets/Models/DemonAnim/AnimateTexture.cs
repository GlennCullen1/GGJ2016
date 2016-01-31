using UnityEngine;
using System.Collections;

public class AnimateTexture : MonoBehaviour {

	public Sprite[] TextureTable;
	int CurrentTexture;
	float timer;
	public float TimerThreshold;
	public SpriteRenderer Target;
	// Use this for initialization
	void Start () {
		CurrentTexture = 0;
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer >= TimerThreshold) {
			timer = 0;
			CurrentTexture ++;
			if(CurrentTexture>=TextureTable.Length)
			{
				CurrentTexture = 0;
			}
			Target.sprite = TextureTable[CurrentTexture];
		}
	}
}
