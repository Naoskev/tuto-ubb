using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteController : MonoBehaviour {

	FurnitureSpriteController fsc;

	Dictionary<Job, GameObject> jobGameObjects = new Dictionary<Job, GameObject>();

	// Use this for initialization
	void Start () {
		this.fsc  = GameObject.FindObjectOfType<FurnitureSpriteController>();

		WorldController.Instance.WorldData.JobQueue.RegisterOnJobCreated(OnJobCreated);
	}

	void OnJobCreated(Job job){

		if(this.jobGameObjects.ContainsKey(job)){
			Debug.Log("Le job "+job+" existe déjà.");
			return;
		}
		GameObject job_go = new GameObject("JOB_"+ job.JobObjectType +"_at_"+job.Tile.X+"_"+job.Tile.Y);
		this.jobGameObjects.Add(job, job_go);

		job_go.transform.position = new Vector3Int(job.Tile.X,job.Tile.Y, 0);
		job_go.transform.SetParent(this.transform, true);

		SpriteRenderer sr = job_go.AddComponent<SpriteRenderer>();
		Furniture prototype = WorldController.Instance.WorldData.getFurniturePrototype(job.JobObjectType);
		sr.sprite = fsc.getFurnitureSprite(prototype.Id, job.Tile, prototype.IsConnected);
		sr.color = new Color(1f, 1f, 1f, 0.25f);
		sr.sortingLayerName = LayerName.JOB.GetDescription();

		job.RegisterCancelledCallback(OnJobEnded);
		job.RegisterCompleteCallback(OnJobEnded);
	}

	void OnJobEnded(Job job){
		this.jobGameObjects[job].GetComponent<SpriteRenderer>().sprite = null;	

	}

	
}
