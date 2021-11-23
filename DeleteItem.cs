using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DataInfo;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using UnityEngine.Networking;


public class DeleteItem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject idObj;
    public GameObject kindObj;

    string email = "wjdwhdgus222@naver.com";

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Delete Button Click");

            string kind = kindObj.GetComponent<Text>().text;
            string id = idObj.GetComponent<Text>().text;

            Debug.Log(kind);
            Debug.Log(id);

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

            DocumentReference itemRef = db.Collection("users").Document(email).Collection(kind).Document(id);
            itemRef.DeleteAsync();

            Destroy(this.transform.parent.gameObject);
            

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
