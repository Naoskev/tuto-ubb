using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character {

	public float X {
		get{
			return Mathf.Lerp(CurrentTile.X, nextTile.X, currentDistanceMovedPercentage);
		}
	}
	public float Y {
		get{
			return Mathf.Lerp(CurrentTile.Y, nextTile.Y, currentDistanceMovedPercentage);
		}
	}

// Varie de 0 à 1 et indique la distance parcourure entre deux tuiles
	float currentDistanceMovedPercentage = 0;

// Vitesse du personnage (en tuile par seconde)
	float movementSpeed;


	public Tile CurrentTile {get; protected set;}
	Tile nextTile;
	
	Tile destinationTile;
	Path_AStar pathfinding;

	Job currentJob;
    Action<Character> cbOnChanged;


	public Character(Tile startTile, float movementSpeed = 2f){
		this.CurrentTile = this.nextTile = this.destinationTile = startTile;
		this.movementSpeed = movementSpeed;

	}

	private void Update_DoWork(float deltaTime){
		if(this.currentJob == null){
			this.currentJob = WorldController.Instance.WorldData.JobQueue.Dequeue();
			if(this.currentJob != null){
				this.destinationTile = this.currentJob.Tile;
				this.currentJob.RegisterCancelledCallback(this.OnJobEnded);
				this.currentJob.RegisterCompleteCallback(this.OnJobEnded);
			}
		}


		if(this.CurrentTile == this.destinationTile){
			if(this.currentJob != null && this.CurrentTile.IsNeighbourWith(this.currentJob.Tile, false)){
				this.currentJob.DoWork(deltaTime);
			}
		}
	}

	private void Update_DoMovement(float deltaTime){
		if(this.destinationTile == this.CurrentTile){
			this.pathfinding = null;
			return;
		}

		if(this.pathfinding == null){
			this.pathfinding = new Path_AStar(WorldController.Instance.WorldData, this.CurrentTile, this.destinationTile);
			if(this.pathfinding.HasPath == false){
				this.currentJob.CancelJob();
				this.currentJob = null;
				this.pathfinding = null;
				this.nextTile = this.destinationTile = this.CurrentTile;
				return;
			}
		}
		if(this.CurrentTile == this.nextTile){
			this.nextTile = this.pathfinding.GetNextTile();		
			if(this.nextTile == destinationTile){
				this.destinationTile = this.nextTile = this.CurrentTile;
				return;
			}	
		}

		
		float distanceBetweenTiles = Mathf.Sqrt( Mathf.Pow(nextTile.X - CurrentTile.X, 2f) +  Mathf.Pow(nextTile.Y - CurrentTile.Y, 2f));
		float distanceToTravelThisFrame = deltaTime * movementSpeed;
		float distanceToTravelAsPercentage = distanceToTravelThisFrame / distanceBetweenTiles;

		this.currentDistanceMovedPercentage += distanceToTravelAsPercentage;

		if(this.cbOnChanged != null){
			this.cbOnChanged(this);
		}

		if(this.currentDistanceMovedPercentage >= 1){
			this.CurrentTile = this.nextTile;
			this.currentDistanceMovedPercentage = 0;

			// FIXME? conservation du mouvement ?
		}
	}

	public void Update(float deltaTime){
		Update_DoWork(deltaTime);

		Update_DoMovement(deltaTime);
	}

	public void SetDestination(Tile tile){
		if(this.CurrentTile.IsNeighbourWith(tile, true) == false){			
			Logger.LogError("Un personnage doit avoir comme destination une case adjacente");
		}

		this.nextTile = tile;
	}

	private void OnJobEnded(Job job){
		if(job != this.currentJob){
			Logger.LogError("Le job "+job+" n'a pas été désaloué au personnage "+this);
			return;
		}

		this.currentJob.UnregisterCancelledCallback(this.OnJobEnded);
		this.currentJob.UnregisterCompleteCallback(this.OnJobEnded);

		this.currentJob = null;
	}


    public void RegisterOnChangeCallback(Action<Character> callback){
        this.cbOnChanged += callback;
    }
    public void UnregisterOnChangeCallback(Action<Character> callback){
        this.cbOnChanged -= callback;
    }
}
