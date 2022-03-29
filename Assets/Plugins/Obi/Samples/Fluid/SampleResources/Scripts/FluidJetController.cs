using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.InputSystem;

public class FluidJetController : MonoBehaviour {

	ObiEmitter emitter;
	public float emissionSpeed = 10;
	public float moveSpeed = 2;

	// Use this for initialization
	void Start () {
		emitter = GetComponentInChildren<ObiEmitter>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Keyboard.current.wKey.isPressed){
			emitter.speed = emissionSpeed;
		}else{
			emitter.speed = 0;
		}

		if (Keyboard.current.aKey.isPressed){
			transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}

		if (Keyboard.current.dKey.isPressed){
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}

	}
}
