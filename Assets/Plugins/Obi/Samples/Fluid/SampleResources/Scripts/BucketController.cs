using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BucketController : MonoBehaviour {

	public Obi.ObiEmitter emitter;
	
	// Update is called once per frame
	void Update () {

		if (Keyboard.current.dKey.isPressed){
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(90,-transform.forward),Time.deltaTime*50);
		}else{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity,Time.deltaTime*100);
		}

		if (Keyboard.current.rKey.isPressed){
			emitter.KillAll();
		}

	}
}
