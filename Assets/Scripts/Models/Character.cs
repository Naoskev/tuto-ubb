using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character {

	public float X {
		get{
			return Mathf.Lerp(CurrentTile.X, destinationTile.X, currentDistanceMovedPercentage);
		}
	}
	public float Y {
		get{
			return Mathf.Lerp(CurrentTile.Y, destinationTile.Y, currentDistanceMovedPercentage);
		}
	}

// Varie de 0 à 1 et indique la distance parcourure entre deux tuiles
	float currentDistanceMovedPercentage = 0;

// Vitesse du personnage (en tuile par seconde)
	float movementSpeed;


	public Tile CurrentTile {get; protected set;}
	Tile destinationTile;

	Job currentJob;
    Action<Character> cbOnChanged;


	public Character(Tile startTile, float movementSpeed = 2f){
		this.CurrentTile = this.destinationTile = startTile;
		this.movementSpeed = movementSpeed;

	}


	public void Update(float deltaTime){
		if(this.currentJob == null){
			this.currentJob = WorldController.Instance.WorldData.JobQueue.Dequeue();
			if(this.currentJob != null){
				this.destinationTile = this.currentJob.Tile;
				this.currentJob.RegisterCancelledCallback(this.OnJobEnded);
				this.currentJob.RegisterCompleteCallback(this.OnJobEnded);
			}
		}


		if(this.CurrentTile == this.destinationTile){
			if(this.currentJob != null && this.currentJob.Tile == this.CurrentTile){
				this.currentJob.DoWork(deltaTime);
			}
			return;
		}

		float distanceBetweenTiles = Mathf.Sqrt( Mathf.Pow(destinationTile.X - CurrentTile.X, 2f) +  Mathf.Pow(destinationTile.Y - CurrentTile.Y, 2f));
		float distanceToTravelThisFrame = deltaTime * movementSpeed;
		float distanceToTravelAsPercentage = distanceToTravelThisFrame / distanceBetweenTiles;

		this.currentDistanceMovedPercentage += distanceToTravelAsPercentage;

		if(this.cbOnChanged != null){
			this.cbOnChanged(this);
		}

		if(this.currentDistanceMovedPercentage >= 1){
			this.CurrentTile = this.destinationTile;
			this.currentDistanceMovedPercentage = 0;

			// FIXME? conservation du mouvement ?
		}
	}

	public void SetDestination(Tile tile){
		if(this.CurrentTile.IsNeighbourWith(tile, true) == false){			
			Logger.LogError("Un personnage doit avoir comme destination une case adjacente");
		}

		this.destinationTile = tile;
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
