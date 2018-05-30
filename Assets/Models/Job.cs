using System.Collections;
using System.Collections.Generic;
using System;

public class Job {

	Tile tile;

	Action<Job> cbJobComplete;

	Action<Job> cbJobCancelled;

	float jobTime;


	public Job(Tile tile, Action<Job> completeCallback,  float jobDuration = 1f){
		this.tile = tile;
		this.cbJobComplete = completeCallback;
		this.jobTime = jobDuration;
	}

	public void RegisterCompleteCallback(Action<Job> callback){
		this.cbJobComplete += callback;
	}
	public void RegisterCancelledCallback(Action<Job> callback){
		this.cbJobCancelled += callback;
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
