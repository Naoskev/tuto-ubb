﻿using System.Collections;
using System;
using System.Collections.Generic;

public class JobQueue {

	Queue<Job> jobQueue = new Queue<Job>();

	private Action<Job> cbOnJobCreated;

	public JobQueue(){

	}


	public void Enqueue(Job job){
		this.jobQueue.Enqueue(job);

		if(this.cbOnJobCreated){
			this.cbOnJobCreated(job);
		}
	}

	public void RegisterOnJobCreated(Action<Job> callback){
		this.cbOnJobCreated += callback;
	}
	
	public void UnregisterOnJobCreated(Action<Job> callback){
		this.cbOnJobCreated -= callback;
	}
}
