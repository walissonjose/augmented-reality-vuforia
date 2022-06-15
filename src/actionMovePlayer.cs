using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class playerMotion : MonoBehaviour {

	float speed = 5.0f;
	CharacterController objectCharacterController;
	Transform transformCamera;
	Vector3 moveCamRight;
	Vector3 moveOmndirect;
	Vector3 normalZeroPiso = new Vector3(0,0,0);
	float giro = 3.0f;
	float frente = 3.0f;
	Vector3 directionalVector = new Vector3(0,0,0);
	




	public GameObject player;
	public Animation animationJump; 
	public GameObject particleEgg;
	public GameObject particleFeather;
	public GameObject particleStar;
	public GameObject objectFire;

	public AudioClip soundEgg;
	public AudioClip soundFeather;
	public AudioClip soundStar; 
	public AudioClip soundHit;
	public AudioClip somWin;
	public AudioClip somLose;
	public AudioClip soundMergeStar;
	public AudioClip soundPlayerFlight;

//	private float speed = 3.5f;
//	private float giro = 230.0f;
//	private float gravidade = 4f;
	private float Jump = 5.0f;
//	private CharacterController objectCharacterController;
//	private Vector3 directionalVector = new Vector3(0,0,0); 

	 
	private int numberObjects;
	private int countLight;
	private bool pickupStar;


	void Start () { 
		objectCharacterController = GetComponent<CharacterController>(); 
		animationJump = player.GetComponent<Animation>(); 
		transformCamera = Camera.main.transform;
	 }
	
	void Update (){ 

		moveCamRight = Vector3.Scale(transformCamera.forward, new Vector3(1, 0, 1)).normalized;
		moveOmndirect = CrossPlatformInputManager.GetAxis("Vertical")*moveCamRight + CrossPlatformInputManager.GetAxis("Horizontal")*transformCamera.right;
		
//		if(CrossPlatformInputManager.GetButton("Jump"))
//		{
//			if (objectCharacterController.isGrounded == true) { directionalVector.y = Jump; }
//		} 
		
		directionalVector.y -= 3.0f * Time.deltaTime;	
		objectCharacterController.Move(directionalVector * Time.deltaTime);
		objectCharacterController.Move(moveOmndirect * speed * Time.deltaTime);
		
		if (moveOmndirect.magnitude > 1f) moveOmndirect.Normalize();
		moveOmndirect = transform.InverseTransformDirection(moveOmndirect);
		
		moveOmndirect = Vector3.ProjectOnPlane(moveOmndirect, normalZeroPiso);
		giro = Mathf.Atan2(moveOmndirect.x, moveOmndirect.z);
		frente = moveOmndirect.z;
		
		objectCharacterController.SimpleMove(Physics.gravity);
		aplicaRotacao();


//		Vector3 forward = CrossPlatformInputManager.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward) * speed;
// 		transform.Rotate(new Vector3(0,CrossPlatformInputManager.GetAxis("Horizontal") * giro *Time.deltaTime,0));
//
//		objectCharacterController.Move(forward * Time.deltaTime);
//		objectCharacterController.SimpleMove(Physics.gravity);
		
//		if(CrossPlatformInputManager.GetButton("Jump"))
//		{
//			if (objectCharacterController.isGrounded == true) { directionalVector.y = Jump; }
//		} 
 		
		if(CrossPlatformInputManager.GetButton("Jump"))
		{
			if (objectCharacterController.isGrounded == true) {
				directionalVector.y = Jump;
				player.GetComponent<Animation>().Play("JUMP");
				GetComponent<AudioSource>().PlayOneShot(soundPlayerFlight, 0.7F);
			}
		}else
		{
			//if(Input.GetButton("Horizontal") || Input.GetButton("Vertical")  )
				if((CrossPlatformInputManager.GetAxis("Horizontal") != 0.0f) || (CrossPlatformInputManager.GetAxis("Vertical") != 0.0f) )
			{
								if (!animationJump.IsPlaying("JUMP"))
								{	 
									player.GetComponent<Animation>().Play("WALK");
								}
				
				
				
			}else
			{
				if (objectCharacterController.isGrounded == true) 
				{	
					player.GetComponent<Animation>().Play("IDLE");
				}
			}
		}




//		directionalVector.y -= gravidade * Time.deltaTime;	
//	    objectCharacterController.Move(directionalVector * Time.deltaTime);
	}




	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "pickEgg")
		{

			GetComponent<AudioSource>().PlayOneShot(soundEgg
		, 0.7F);

			 Instantiate(particleEgg, other.gameObject.transform.position, Quaternion.identity);
			other.gameObject.SetActive (false); 

			numberObjects++; verificaPickObjetos();
		}

		
		if (other.gameObject.tag == "pickFeather")
		{
			
			GetComponent<AudioSource>().PlayOneShot(soundFeather, 0.7F);
			
			Instantiate(particleFeather, other.gameObject.transform.position, Quaternion.identity);
			other.gameObject.SetActive (false); 
			numberObjects++; verificaPickObjetos();
		}

		
		if (other.gameObject.tag == "pickStar")
		{
			


			if (pickupStar) {

				GetComponent<AudioSource>().PlayOneShot(soundStar, 0.7F);
				
				Instantiate(particleStar, other.gameObject.transform.position, Quaternion.identity);
				other.gameObject.SetActive (false);

				GetComponent<AudioSource>().PlayOneShot(somWin, 0.7F);
				fimDeJogo();
			}
			 
		}
		if (other.gameObject.tag == "Fire")
		{
			 
			InvokeRepeating("mudaEstadoPlayer",0,0.1f);
			GetComponent<AudioSource>().PlayOneShot(soundHit, 0.7F); 
			objectCharacterController.Move(transform.TransformDirection(Vector3.back)*3);

			 
		}
		if (other.gameObject.tag == "Finish")
		{
			GetComponent<AudioSource>().PlayOneShot(somLose, 0.7F);
			fimDeJogo();
			 
		}
	}



	void loseGame()
	{ 
		Invoke("somLose",3);
	}

	void loadLevel()
	{
		Application.LoadLevel("somWin");

	}


	void changeStatePlayer()
	{
		countLight++;
		player.SetActive(!player.activeInHierarchy);


		if (countLight>7) {countLight=0;player.SetActive(true); CancelInvoke();}

	}

	void verifyPickObject()
	{
		if (numberObjects>15)
		{
			pickupStar = true;
			Destroy(objectFire);
			GetComponent<AudioSource>().PlayOneShot(soundMergeStar, 0.7F);
		}

	}
	 
	void applyRotacao()
	{
		float turnSpeed = Mathf.Lerp(180, 360, frente);
		transform.Rotate(0, giro * turnSpeed * Time.deltaTime, 0);
	}
	 
}
