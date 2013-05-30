using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ReduceVolume : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		audio.volume -= Time.deltaTime/5;	
	}
}
