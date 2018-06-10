using System.Collections;
using System.Collections.Generic;
using System;

public class Job {

	public Tile Tile{get; private set;}

	Action<Job> cbJobComplete;

	Action<Job> cbJobCancelled;

	float jobTime;

	public string JobObjectType {get; protected set;}


	public Job(Tile tile, string objectType, Action<Job> completeCallback,  float jobDuration = 1f){
		this.Tile = tile;
		this.cbJobComplete = completeCallback;
		this.jobTime = jobDuration;
		this.JobObjectType = objectType;
	}

	public void RegisterCompleteCallback(Action<Job> callback){
		this.cbJobComplete += callback;
	}
	public void UnregisterCompleteCallback(Action<Job> callback){
		this.cbJobComplete -= callback;
	}
	public void RegisterCancelledCallback(Action<Job> callback){
		this.cbJobCancelled += callback;
	}
	public void UnregisterCancelledCallback(Action<Job> callback){
		this.cbJobCancelled -= callback;
	}

	public void DoWork(float workTime){
		this.jobTime -= workTime;

		if(jobTime <= 0){
			if(this.cbJobComplete != null){
				this.cbJobComplete(this);
			}
		}
	}

	public void CancelJob(){
		if(this.cbJobCancelled != null){
			this.cbJobCancelled(this);
		}
	}

}
