using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour {

	private Dictionary<Character, GameObject> characterGameObjects = new Dictionary<Character, GameObject>();

	private Dictionary<string, Sprite> characterSprites = new Dictionary<string, Sprite>();

	private World _world {get{ return WorldController.Instance.WorldData; } }

	// Use this for initialization
	void Start () {
		Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters");
		foreach (Sprite sprite in sprites)
		{
			this.characterSprites.Add(sprite.name, sprite);
		}

		this._world.RegisterOnCharacterCreated(this.OnCharacterCreated);
		
		_world.CreateCharacter(_world.getTileAt(_world.Width/2, _world.Heigth /2));
	}

	public void OnCharacterCreated(Character character){		
		GameObject character_go = new GameObject("Character_at_"+character.CurrentTile.X+"_"+character.CurrentTile.Y);
		this.characterGameObjects.Add(character, character_go);

		character_go.transform.position = new Vector3Int(character.CurrentTile.X,character.CurrentTile.Y, 0);
		character_go.transform.SetParent(this.transform, true);

		SpriteRenderer sr = character_go.AddComponent<SpriteRenderer>();
		sr.sprite = this.getSprite("p1_front");		
		sr.sortingLayerName = LayerName.CHARACTER.GetDescription();
	}
	
	public Sprite getSprite(string spriteName){
		if(this.characterSprites.ContainsKey(spriteName) == false){
			Debug.LogError("Aucune sprite pour "+spriteName);
			return null;
		}
		
		return this.characterSprites[spriteName];
	}
}
