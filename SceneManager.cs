using UnityEngine;
using System.Collections;
using DataInfo;

public class SceneManager : MonoBehaviour {

	public GameObject characterPrefab;
	public Transform spawnPos;

	public GameObject playerCharacter;
	public GameObject camera;
	// Use this for initialization


	void Start () {
		characterPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		
		// instantiate girl character on spawn position
		playerCharacter = Instantiate( characterPrefab, spawnPos.position, spawnPos.rotation ) as GameObject;
		Debug.Log(DataInfo.User.email);
		StartCoroutine( "LoadSavedDresses" );

		GameObject.Find("JoyStick").GetComponent<VirtualJoystick>().controller = playerCharacter.GetComponent<CharacterMove>();

		camera = GameObject.FindWithTag("MainCamera");
		camera.GetComponent<MoveCamera>().target = playerCharacter.transform;

	}

	IEnumerator LoadSavedDresses() {
		yield return null;
		// load saved data and change body parts.
		playerCharacter.GetComponent<Character>().LoadSavedDresses();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
