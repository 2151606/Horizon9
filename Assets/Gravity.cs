using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Vector2 distanceFromPlayer;

    private float distanceFromPlayerF;

    private float gravityForce = 9.8f;

	private Collider2D thePlayer;

	private bool gravityOn = false;

	private bool useDistFromPlayer = true;

	private bool easyOrbits = false;

	private float gravityScale = 1;


    private Vector2 forceToApply;
    private float dotProduct;
    private Vector2 planetDirection;

	[SerializeField] [Range(0f, 1f)] private float inAngleThreshold = 0.20f;
	[SerializeField] [Range(0f, -1f)] private float outAngleThreshold = -0.20f;

    [SerializeField] [Range(0f, 5f)] private float inGravityScale = 0.6f;
    [SerializeField] [Range(0f, 5f)]private float outGravityScale = 2.2f;
    [SerializeField] [Range(0f, 5f)]private float normalGravityScale = 1f;

	private PlayerScript playerscript;

	void Start(){
		Physics.queriesHitTriggers = true;
		GameObject Player = GameObject.FindWithTag ("Player");
		thePlayer = Player.GetComponent<Collider2D>();
		playerscript = Player.GetComponent<PlayerScript> ();
	}

    void FixedUpdate()
    {
		if (gravityOn) {
			//test
			//if (player.tag == "Player") {
			//thePlayer = player;



			//aplicar força gravítica ao character do jogador

			distanceFromPlayer = transform.position - thePlayer.transform.position;

			distanceFromPlayerF = Vector2.Distance (transform.position, thePlayer.transform.position);

			// get the rotation angle relative to the planet orbit?
			planetDirection = transform.position - thePlayer.transform.position;
			dotProduct = Vector3.Dot (thePlayer.transform.up.normalized, planetDirection.normalized);

			// v = 1: the two vectors face the exact same direction
			//                0 < v < 1: the two vectors face roughly the same direction
			//            v = 0: the two vectors are perpendicular
			//            -1 < v < 0: the two vectors face roughly opposite directions
			//            v = -1: the two vectors face exactly opposite directions

			if (easyOrbits) {
				// The player is tending towards the planet surface = lower grav
				// The player is tending away from the planet surface = higher grav
				// Result: easier to enter a stable orbit
				if (dotProduct > inAngleThreshold) {
					gravityScale = inGravityScale;
				} else if (dotProduct < outAngleThreshold) {
					gravityScale = outGravityScale;
				}
				gravityScale = normalGravityScale;
			}


			//	    if(Vector2.)

			//realistico

			if(useDistFromPlayer)
				forceToApply = (gravityForce * gravityScale / Mathf.Pow (distanceFromPlayerF, 2f)) * distanceFromPlayer;
			else
				forceToApply = (gravityForce * gravityScale / Mathf.Pow (2, 2f)) * distanceFromPlayer;


			//melhor gameplay
			//forceToApply = (Mathf.Pow(gravityForce, 2f) / Mathf.Pow(distanceFromPlayerF, 2f)) * distanceFromPlayer;

			//excelente gameplay
			//forceToApply = new Vector2(10f / distanceFromPlayer.x, 10f / distanceFromPlayer.y);

			//forceToApply = 1f / distanceFromPlayerF * distanceFromPlayer;

			//forceToApply = -distanceFromPlayer;

			//print (forceToApply);

			thePlayer.GetComponent<Rigidbody2D> ().AddForce (forceToApply);

			//

			//}
		}

    }

	void Update(){
		
	}

    private void OnDrawGizmos()
    {
		//remember to activate gizmos first
		if (gravityOn)
        {
            float angle = Vector3.Dot(thePlayer.transform.up.normalized, planetDirection.normalized);
            Handles.Label(new Vector3(0f, 1.5f, 0f), angle.ToString());
            Handles.Label(new Vector3(0f, 2f, 0f), thePlayer.transform.rotation.eulerAngles.ToString());
            Gizmos.DrawLine(thePlayer.transform.position, transform.position);
        }
    }
	/*
	void OnMouseOver(){
		if (!playerscript.isDead) {
			if (Input.GetMouseButton (0))
				gravityOn = true;
			else
				gravityOn = false;
		} else
			gravityOn = false;
	}
	void OnMouseExit(){
		gravityOn = false;
	}
*/
    void OnTriggerStay2D(Collider2D player)
    {
		    }

	public void TurnOn(){
		gravityOn = true;
	}

	public void TurnOff(){
		gravityOn = false;
	}
}