using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	private Vector3 posAtual;
	private Vector3 posAnterior;
	private Vector3 posAnteriorAnterior;
	private Vector3 speed;
	public bool isDead = false;

	private float thrust = 7.5f;

	public Touch[] toques;

	void Start (){
		posAtual = transform.position;
		posAnterior = transform.position;
		posAnteriorAnterior = transform.position;
		Time.timeScale = 2;
		Time.fixedDeltaTime = 0.04f;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2(20,0));
	}

	void /*Fixed ? */Update () {
		
		//DESLIGAR A GRAV DE TODOS OS PLANETAS, depois:

		if (Input.touchCount > 0) {
			toques = Input.touches;
			for (int i = 0; i < toques.Length; ++i) {
				Touch toque = toques[i];
				Vector2 posDesteToque = TouchHandler (toque.position);
				RaycastHit2D acertouPlaneta = CastRay (posDesteToque);
				if (acertouPlaneta != null && acertouPlaneta.transform.gameObject.tag == "planet") {
					Gravity script = acertouPlaneta.collider.gameObject.GetComponent<Gravity> ();

					if (toque.phase == TouchPhase.Began || toque.phase == TouchPhase.Stationary || toque.phase == TouchPhase.Moved) {
//					if (acertou != null && acertou.transform.gameObject.tag == "planet")
						script.TurnOn ();
					}
				}
//				else if (toque.phase == TouchPhase.Stationary || toque.phase == TouchPhase.Moved) {
//					if (acertou != null && acertou.transform.gameObject.tag == "planet") {
//						script.TurnOn ();
//					}
//					else
//						script.TurnOff (); //ISTO NÃO DESLIGA NADA PORQUE É NULL, TEM DE DESLIGAR O ANTERIOR, GUARDAR NUMA VAR!!
//										   //ou então desligar tudo sempre
//				}
//				else if (toque.phase == TouchPhase.Ended || toque.phase == TouchPhase.Canceled) {
//					script.TurnOff ();
//				}
			}
		}
		if (Input.GetMouseButtonDown (0)) {
			Vector2 posDesteToque = TouchHandler (Input.mousePosition);
			RaycastHit2D acertouPlaneta = CastRay (posDesteToque);
			if (acertouPlaneta != null && acertouPlaneta.transform.gameObject.tag == "planet") {
				Gravity script = acertouPlaneta.collider.gameObject.GetComponent<Gravity> ();
				script.TurnOn ();
			}
		}
		/*
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase == TouchPhase.Began || Input.GetTouch (i).phase == TouchPhase.Stationary || Input.GetTouch (i).phase == TouchPhase.Moved) {

					toques[i] = 

				//if: ray toca no collider de um planeta (pelo menos um planeta)
					//ver qual é o planeta mais prox
					//~~~~if: varios
						//ver qual é o planeta mais prox
					//ativar a grav nesse planeta
			}
		}*/
		//NÃO FAZ PARTE DO GAMEPLAY FINAL
		if (Input.GetKey ("up")) {
			Thrust ();
		}
	}

	Vector2 TouchHandler(Vector2 screenPos){
		Vector3 posToque = Camera.main.ScreenToWorldPoint (screenPos);
		Vector2 posToque2D = new Vector2 (posToque.x, posToque.y);
		return posToque2D;
		/*
		RaycastHit2D infoHit = Physics2D.Raycast (posToque2D, Camera.main.transform.forward);
		if (infoHit.collider != null && infoHit.transform.gameObject.tag == "planet") {
			infoHit.collider.gameObject.GetComponent<Gravity> ().TurnOn ();
		}
		*/
	}

	RaycastHit2D CastRay(Vector2 pos/*, Touch toque, bool soLiga*/){
		RaycastHit2D hit = Physics2D.Raycast (pos, Camera.main.transform.forward);
//		if (hit != null && hit.transform.gameObject.tag == "planet")
			return hit;
//		else
//			return;
		/*
		if (acertou.collider != null){
			//ligar
			if(soLiga)
				return;
		}
		else {
			//desliga
		}
		*/
	}

	void FixedUpdate () {

		if (!isDead) {
			//virar jogador na direção do movimento atual
			posAnteriorAnterior = posAnterior;
			posAnterior = posAtual;
			posAtual = transform.position;

			Vector3 dir = transform.position - posAnteriorAnterior;
			if (dir != new Vector3 (0, 0, 0)) {
				float angle = Mathf.Atan2 (-dir.x, dir.y) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.AngleAxis (angle, Vector3.forward), 5f);
			}
			//aumentar a velocidade ~~do jogador~~ do jogo (para afetar a gravidade)
			//GetComponent<Rigidbody2D>().AddForce(transform.up * 1.001f);
			Time.timeScale = Mathf.Min (5, Time.timeScale * 1.001f);
			Time.fixedDeltaTime = Mathf.Min (0.10f, Time.fixedDeltaTime * 1.001f);

		}
	}

	void Thrust () {
		//aplicar força na direção do movimento atual
		GetComponent<Rigidbody2D> ().AddForce (transform.up * thrust);
	}

	void OnCollisionEnter2D(Collision2D other){
		if (isDead)
			return;
		isDead = true;
	}
}
