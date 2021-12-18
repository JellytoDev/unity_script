using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour {

	public GameObject characterPrefab;
	public Transform spawnPos;

	public static GameObject myCharacter;

	// Use this for initialization
	void Start () {

		characterPrefab.transform.localScale = new Vector3(1, 1, 1);
	// instantiate girl character on the spawn position
		myCharacter = Instantiate( characterPrefab, spawnPos.position, spawnPos.rotation ) as GameObject;
		myCharacter.transform.position = new Vector3(1, 0, 10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
