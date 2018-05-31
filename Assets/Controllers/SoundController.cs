using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	float soundCooldown = 0;

	// Use this for initialization
	void Start () {
		WorldController.Instance.WorldData.RegisterOnTileChanged(onTileChanged);
		WorldController.Instance.WorldData.RegisterOnFurniturePlaced(onFurniturePlaced);		
	}

	
	// Update is called once per frame
	void Update () {
		soundCooldown -= Time.deltaTime;		
	}

	private void onTileChanged(Tile tile){
		if(soundCooldown > 0) return;

		if(tile.Type == TileType.Floor){
			AudioClip ac = Resources.Load<AudioClip>("Sounds/Floor_OnCreated");
			AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
			soundCooldown = 0.1f;	
		}
	}


	private void onFurniturePlaced(Furniture furniture){
		if(soundCooldown > 0) return;

		AudioClip ac = Resources.Load<AudioClip>("Sounds/"+furniture.Id+"_OnCreated");
		if(ac == null){
			Debug.LogError("SoundController -- audio non trouvé pour "+furniture.Id);
			 ac = Resources.Load<AudioClip>("Sounds/Wall_OnCreated");
		}
		AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position);
		soundCooldown = 0.1f;
	}
}
