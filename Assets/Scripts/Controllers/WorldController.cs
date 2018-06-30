using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;

public class WorldController : MonoBehaviour {

	private static readonly string QUICK_SAVE = "quick-save001";
	public static WorldController Instance{get; protected set;}
	public World WorldData {get; protected set; }

	private static bool loadWorld = false;

	// Before any game object start
	void OnEnable () {
		if(Instance != null)
			Debug.LogError("There should be only one world controller.");
		WorldController.Instance = this;

		if(loadWorld){
			loadWorld = false;
			LoadWorldFromSaveFile();
		}
		else {
			CreateEmptyWorld();
		}

		int worldMiddleX = WorldData.Width / 2, worldMiddleY = WorldData.Height / 2;
		Camera.main.transform.position = new Vector3(worldMiddleX, worldMiddleY, Camera.main.transform.position.z);

		Debug.Log("World created");	
	}

	void Start(){
		//this.WorldData.SetupPathfindingExample();
	}

	void Update(){
		this.WorldData.Update(Time.deltaTime);
	}
	
	public Tile getTileFromVector(Vector3 coord){
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);
		
		return this.WorldData.getTileAt(x, y);
	}

	public void SaveWorld(){
		PlayerPrefs.SetString(QUICK_SAVE, this.WorldData.SaveWorld());
	}

	public void LoadWorld(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		loadWorld = true;
	}

	public void NewWorld(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	private void LoadWorldFromSaveFile(){
		string saveData = PlayerPrefs.GetString(QUICK_SAVE);
		if(saveData == null || saveData.Length == 0){
			Logger.LogError("Aucune sauvegarde à charger !");
			this.CreateEmptyWorld();
			return;
		}
		try{
			this.WorldData = SaveManager.LoadWorld(saveData);
		}
		catch(System.Exception e){
			Logger.LogError("Echec du chargement de la sauvegarde : "+e.ToString());
			this.CreateEmptyWorld();
			// TODO quitter et avertir l'utilisateur
		}
	}

	private void CreateEmptyWorld(){
		this.WorldData = new World(100, 100);
	}
}
