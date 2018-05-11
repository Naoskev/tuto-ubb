using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticVerticalSize : MonoBehaviour {


	public float childsHeight = 35f;

	// Use this for initialization
	void Start () {
		this.AdjustSize();
	}

	public void AdjustSize(){
		Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
		size.y = this.transform.childCount * this.childsHeight;
		this.GetComponent<RectTransform>().sizeDelta = size;
	}

}
