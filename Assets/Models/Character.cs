using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

	float X {
		get{
			return Mathf.Lerp(CurrentTile.X, destinationTile.X, currentDistanceMovedPercentage);
		}
	}
	float Y {
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




	public Character(Tile startTile, float movementSpeed = 2f){
		this.CurrentTile = this.destinationTile = startTile;
		this.movementSpeed = movementSpeed;

	}


	public void Update(float deltaTime){
		if(this.CurrentTile == this.destinationTile) return;

		float distanceBetweenTiles = Mathf.Sqrt( Mathf.Pow(destinationTile.X - CurrentTile.X, 2f) +  Mathf.Pow(destinationTile.Y - CurrentTile.Y, 2f));
		float distanceToTravelThisFrame = deltaTime * movementSpeed;
		float distanceToTravelAsPercentage = distanceToTravelThisFrame / distanceBetweenTiles;

		this.currentDistanceMovedPercentage += distanceToTravelAsPercentage;

		if(this.currentDistanceMovedPercentage >= 1){
			this.CurrentTile = this.destinationTile;
			this.currentDistanceMovedPercentage = 0;

			// FIXME? conservation du mouvement ?
		}
	}

	public void SetDestination(Tile tile){
		if(false){
			// TODO
			Logger.LogError("Un personnage doit avoir comme destination une case adjacente");
		}

		this.destinationTile = tile;
	}
}
