﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControllerAdv : MonoBehaviour {

	public bool isAlive = true;

	public float speed = 6;
	//public float maxspeed = 5;
	public float turn = 6f;

	Rigidbody rigid;
	GameObject ark;
	//Collider rigid;

	public Slot slot;
	public enum Slot{
		Player_1,
		Player_2,
		Player_3,
		Player_4,
		Player_5,
		Player_6,
		Player_7,
		Player_8,
		Player_9,
		Player_10
	}

	public Faction faction;
	public enum Faction{
		HOG,
		RAM
	}

	public static List<PlayerControllerAdv> players = new List<PlayerControllerAdv>();

	Charge charge = null;

	// Use this for initialization
	void Awake() {
		//rigid = GetComponent<Rigidbody> (); 
		rigid = GetComponent<Rigidbody>();
		ark = transform.Find("Ark").gameObject;
	}


	Vector2 joyDirL = Vector2.zero;
	Vector2 joyDirR = Vector2.zero;

	void FixedUpdate(){
		Vector3 moveDir = Vector3.forward * joyDirL.x + Vector3.right * joyDirL.y;

		//ROTATION
		if (moveDir.magnitude != 0) {
			Quaternion targetRotation = Quaternion.LookRotation (moveDir, Vector3.up);
			float factor = 1 - Quaternion.Angle( transform.rotation, targetRotation )/360f;
			Quaternion deltaRotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Pow(turn, factor) * Time.deltaTime);

			rigid.MoveRotation (deltaRotation);
			//rigid.MoveRotation (deltaRotation);
		}

		// ark.transform.rotation * test;


		//Vector3 deltaPosition = moveDir * speed * Time.deltaTime;

		float adjustment = 0;
		/*if (rigid is CapsuleCollider) {
			CapsuleCollider c = (CapsuleCollider)rigid;
			adjustment = c.radius;
		}*/

		//Debug.Log (deltaPosition.magnitude);
		//RaycastHit hit;
		//if (!Physics.SphereCast(transform.position,adjustment, deltaPosition.normalized, out hit, deltaPosition.magnitude, Physics.DefaultRaycastLayers , QueryTriggerInteraction.Ignore)){
			//transform.position = transform.position + deltaPosition;
			//rigid.MovePosition (transform.position + deltaPosition);
			//rigid.AddForce(deltaPosition*100, ForceMode.Impulse);
			//if (Physics.Raycast (transform.position, deltaPosition.normalized, out hit, deltaPosition.magnitude + adjustment)) {
			//
		//} 
		/*else {
			Debug.Log ("Found an object - distance: " + hit.distance + " " + hit.collider.gameObject.name);	
		}*/


		rigid.AddForce (moveDir, ForceMode.VelocityChange);
		if (rigid.velocity.magnitude > speed) {
			rigid.velocity = rigid.velocity.normalized * speed;
		}

		if (charge != null) {
			if (charge.time + 0.3 > Time.time) {
				rigid.velocity = charge.direction * 14;
			} else {
				charge = null;
				rigid.velocity = Vector3.zero;
			}
		}

		//ARK
		Vector3 lookDir = Vector3.forward * joyDirR.x + Vector3.right * joyDirR.y;
		if (lookDir.magnitude != 0) {
			ark.gameObject.GetComponent<Renderer>().enabled = true;

			Quaternion targetRotation = Quaternion.LookRotation (lookDir, Vector3.up) * Quaternion.LookRotation (Vector3.up, Vector3.forward);
			float factor = 1 - Quaternion.Angle( ark.transform.rotation, targetRotation )/360f;
			Quaternion deltaArk = Quaternion.Slerp(ark.transform.rotation, targetRotation, Mathf.Pow(turn*2, factor) * Time.deltaTime);

			//Quaternion deltaArk = Quaternion.LookRotation (lookDir, Vector3.up) * Quaternion.LookRotation (Vector3.up, Vector3.forward);
			ark.transform.rotation = deltaArk;
			ark.transform.position = transform.position + deltaArk * new Vector3 (0, 1f, 0.25f);
		} else {
			ark.gameObject.GetComponent<Renderer>().enabled = false;

			Quaternion deltaArk = Quaternion.LookRotation (transform.forward, Vector3.up) * Quaternion.LookRotation (Vector3.up, Vector3.forward);
			ark.transform.rotation = deltaArk;
			ark.transform.position = transform.position + deltaArk * new Vector3 (0, 1f, 0.25f);
		}
	}

	void OnEnable(){
		players.Add (this);
	}

	void OnDisable(){
		players.Remove (this);
	}

	// Update is called once per frame
	void Update () {
		joyDirL = new Vector2 (GetLeftStickY(), GetLeftStickX());
		joyDirR = new Vector2 (GetRightStickY(), GetRightStickX());

		//Debug.DrawLine(transform.position, transform.position + ark.transform.rotation * Vector3.up*20, Color.green);

		if(GetRightBumper()){

			if (charge == null) {
				charge = new Charge(ark.transform.rotation * Vector3.up, Time.time);
			}
			
		}
	}

	int GetControllerID(){

		int id = -1;

		switch(slot){
		case Slot.Player_1:
			id = 1;
			break;

		case Slot.Player_2:
			id = 2;
			break;

		case Slot.Player_3:
			id = 3;
			break;

		case Slot.Player_4:
			id = 4;
			break;

		case Slot.Player_5:
			id = 5;
			break;

		case Slot.Player_6:
			id = 6;
			break;

		case Slot.Player_7:
			id = 7;
			break;

		case Slot.Player_8:
			id = 8;
			break;
		}

		return id;
	}

	bool GetRightBumper(){

		switch (GetControllerID()){
		case 1: return Input.GetKeyDown(KeyCode.Joystick1Button5);
		case 2: return Input.GetKeyDown(KeyCode.Joystick2Button5);
		case 3: return Input.GetKeyDown(KeyCode.Joystick3Button5);
		case 4: return Input.GetKeyDown(KeyCode.Joystick4Button5);
		case 5: return Input.GetKeyDown(KeyCode.Joystick5Button5);
		case 6: return Input.GetKeyDown(KeyCode.Joystick6Button5);
		case 7: return Input.GetKeyDown(KeyCode.Joystick7Button5);
		case 8: return Input.GetKeyDown(KeyCode.Joystick8Button5);
			// Unity does not support buttons for more than 8 joysticks
		default: return false;
		}
	}

	float GetLeftStickX()
	{
		int id = GetControllerID ();
		if (id < 1 || id > 8)
		{
			Debug.Log("Invalid controller number");
			return 0;
		}
		return Input.GetAxis("P" + id + "LeftStickX");
	}

	float GetLeftStickY()
	{
		int id = GetControllerID ();
		if (id < 1 || id > 8)
		{
			Debug.Log("Invalid controller number");
			return 0;
		}
		return Input.GetAxis("P" + id + "LeftStickY");
	}

	float GetRightStickX()
	{
		int id = GetControllerID ();
		if (id < 1 || id > 8)
		{
			Debug.Log("Invalid controller number");
			return 0;
		}
		return Input.GetAxis("P" + id + "RightStickX");
	}

	float GetRightStickY()
	{
		int id = GetControllerID ();
		if (id < 1 || id > 8)
		{
			Debug.Log("Invalid controller number");
			return 0;
		}
		return Input.GetAxis("P" + id + "RightStickY");
	}


}


class Charge{

	public Vector3 direction;
	public float time;

	public Charge(Vector3 direction, float time){
		this.direction =  direction;
		this.time = time;
	}
}