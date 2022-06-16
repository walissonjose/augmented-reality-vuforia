using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class moveBolinha : MonoBehaviour {
	
	public float speed;
	
	private Rigidbody rb;
	public Text countText;
	public Text winText;
	 
	private int count;


	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		count = 0;
		SetCountText ();
		winText.text = "";
	}
	
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		
		rb.AddForce (movement * speed);

		if (Input.GetButton("Jump"))
		{
			rb.AddForce (new Vector3 (0,50.0f,0));
		}

	}


	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "pickItem")
		{
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();
		}
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= 5)
		{
			winText.text = "You Win!";
		}
	}
}