using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DataInfo;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.Networking;

public class ListHeartBtn : MonoBehaviour
{
    // Start is called before the first frame update

    public struct ClothData
    {
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string description { get; set; }
        [FirestoreProperty]
        public long num { get; set; }
        [FirestoreProperty]
        public string price { get; set; }
        [FirestoreProperty]
        public string image { get; set; }
    }

    public GameObject DialogBox;

    void Start()
    {
        DialogBox = GameObject.Find("Dialog Box").gameObject;

        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            heartButton();
        });
    }

    public void heartButton()
    {
        GameObject btn = EventSystem.current.currentSelectedGameObject;
        Debug.Log(btn.tag);

        ICloth cloth = new ICloth(btn.tag, GameData.shopName);
        User.basketCloth.Push(cloth);

        string clothName = "cloth" + btn.tag.ToString();
        string shopName = "meta_shop";//GameData.shopName;

        ClothDataGet("wjdwhdgus222@naver.com", "heart", shopName, clothName);

        DialogChange("찜한목록 성공", "선택하신 상품이 찜한목록에 넣어졌습니다. \n해당 상품은 드레스룸에서 입어본 모습을 확인할 수 있습니다.");

        GameObject.Find("ShopUI").SetActive(false);

        DialogBox.GetComponent<CanvasGroup>().alpha = 1;

    }

    public void DialogChange(string title, string content)
    {

        DialogBox.transform.Find("Header").Find("Title").GetComponent<Text>().text = title;
        DialogBox.transform.Find("Content").Find("Text").GetComponent<Text>().text = content;

    }

    public void ClothDataGet(string email, string kind, string shopName, string clothName)
    {
        Debug.Log(email + kind + shopName + clothName);

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        DocumentReference newRef = db.Collection("users").Document(email).Collection(kind).Document(clothName);
        Dictionary<string, object> newCloth = new Dictionary<string, object>
            {
                    {"name" ,""  },
                    {"description","" },
                    {"price","" },
                    {"num",0 },
                    {"image","" },
                    {"kind",kind }
            };
        newRef.SetAsync(newCloth);

        DocumentReference docRef = db.Collection("shops").Document(shopName).Collection("sale").Document(clothName);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> cloth = snapshot.ToDictionary();

                DocumentReference userRef = db.Collection("users").Document(email).Collection(kind).Document(clothName);


                foreach (KeyValuePair<string, object> pair in cloth)
                {
                    Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                                { pair.Key, pair.Value }
                        };

                    userRef.UpdateAsync(updates).ContinueWithOnMainThread(task2 => {
                        Debug.Log(
                                "Updated the Capital field of the new-city-id document in the cities collection.");
                    });
                    Debug.Log("new firebase upload");

                }
                Dictionary<string, object> shopUpdates = new Dictionary<string, object>
                        {
                                { "shopName", shopName }
                        };

                userRef.UpdateAsync(shopUpdates).ContinueWithOnMainThread(task3 => {
                    Debug.Log(
                            "Updated the Capital field of the new-city-id document in the cities collection.");
                });
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
