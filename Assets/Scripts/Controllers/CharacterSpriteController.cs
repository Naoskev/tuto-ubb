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
		
		foreach (Character character in this._world.Characters)
		{
			this.OnCharacterCreated(character);
		}
	}

	public void OnCharacterCreated(Character character){		
		GameObject character_go = new GameObject("Character_at_"+character.CurrentTile.X+"_"+character.CurrentTile.Y);
		this.characterGameObjects.Add(character, character_go);

		character_go.transform.position = this.getCharacterPosition(character);
		character_go.transform.SetParent(this.transform, true);

		SpriteRenderer sr = character_go.AddComponent<SpriteRenderer>();
		sr.sprite = this.getSprite("p1_front");		
		sr.sortingLayerName = LayerName.CHARACTER.GetDescription();

		character.RegisterOnChangeCallback(this.OnCharacterChanged);
	}

	public void OnCharacterChanged(Character character){
		if(this.characterGameObjects.ContainsKey(character) == false){
			Debug.LogError("Character "+character+" n'a pas été ajouté à la collection d'objet Unity");
			return;
		}
		GameObject character_go = this.characterGameObjects[character];
		if(character_go == null){
			Debug.LogError("L'objet unity du character "+character+" est null");
			return;
		}

		character_go.transform.position = this.getCharacterPosition(character);
	}

	private Vector3 getCharacterPosition(Character character){
		return new Vector3(character.X, character.Y, 0);
	}
	
	public Sprite getSprite(string spriteName){
		if(this.characterSprites.ContainsKey(spriteName) == false){
			Debug.LogError("Aucune sprite pour "+spriteName);
			return null;
		}
		
		return this.characterSprites[spriteName];
	}
}
