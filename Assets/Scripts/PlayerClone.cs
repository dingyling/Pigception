﻿using UnityEngine;
using System.Collections;

public class PlayerClone : MonoBehaviour {

	public GameObject player2;
	public GameObject player3;
	public GameObject player4;
	public Vector3 offset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		player2.transform.position = this.gameObject.transform.position + new Vector3(offset.x,0,0);
		player3.transform.position = this.gameObject.transform.position + new Vector3(0,offset.y,0);
		player4.transform.position = this.gameObject.transform.position + offset;
	}
}
