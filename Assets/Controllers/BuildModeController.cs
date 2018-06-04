using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour {

	private TileType buildMode = TileType.Floor;
	private bool tileBuildMode = true;
	private string furnitureId= null;

	// Use this for initialization
	void Start () {
	}


	public void DoBuild(Tile tile){
		if(this.tileBuildMode){
			tile.Type = this.buildMode;
		}
		else {

			if(WorldController.Instance.WorldData.IsFurniturePositionValid(this.furnitureId, tile) == false
				|| tile.pendingFurnitureJob != null){
				return;
			}
			// version de construction instantannée
			// WorldController.Instance.World.PlaceInstalledObject(this.installedObjectId, tileToChange);
			string localFurnitureId = this.furnitureId;
			Job job = new Job(tile, (finishedJob) => { 
				WorldController.Instance.WorldData.PlaceInstalledObject(localFurnitureId, finishedJob.Tile); 
				finishedJob.Tile.pendingFurnitureJob = null;
			});
			
			tile.pendingFurnitureJob = job;
			job.RegisterCancelledCallback( (theJob) => { theJob.Tile.pendingFurnitureJob = null; } );

			WorldController.Instance.WorldData.JobQueue.Enqueue(job);
		}		
	}

	public void SetBuildMode_InstalledObject(string id){
		this.furnitureId = id;
		this.tileBuildMode = false;
	}
	public void SetBuildMode_Floor(){
		this.tileBuildMode = true;
		this.buildMode = TileType.Floor;
	}
	public void SetBuildMode_Bulldoze(){
		this.tileBuildMode = true;
		this.buildMode = TileType.Empty;
	}

}
