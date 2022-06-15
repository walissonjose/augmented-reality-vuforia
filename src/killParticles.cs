using UnityEngine;
using System.Collections;

public class killParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {


		InvokeRepeating("killParticle", 1, 2.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void mataParticle()
	{ Destroy(this.gameObject);
	}

}
