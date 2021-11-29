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

public class itemDress : MonoBehaviour
{
    public Renderer UpBody;
    public Renderer DownBody;
    public Renderer Hair;
    public Renderer Glass;


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            //firebase 주소 가져오기

            //주소로 이미지 다운로드

            // 텍스쳐 적용

            Debug.Log("item click됨");

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            //FirebaseStorage storage = FirebaseStorage.DefaultInstance;

            string clothId = this.transform.Find("id").GetComponent<Text>().text;
            string kind = this.transform.Find("kind").GetComponent<Text>().text;

            string email = "wjdwhdgus222@naver.com";//DataInfo.User.email;

            Debug.Log(clothId);
            Debug.Log(kind);

            DocumentReference docRef = db.Collection("users").Document(email).Collection(kind).Document(clothId);

            string clothType =null;
            string texture1Addr = null;
            string texture2Addr = null;

            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                    Dictionary<string, object> clothData = snapshot.ToDictionary();
                    Debug.Log("firebase data get success");

                    clothType = clothData["type"].ToString();

                    Debug.Log(clothType);

                    if (clothType == "upbody")
                    {
                        Debug.Log("upbody image download start" + texture1Addr);

                        Material[] upBodys = GameObject.Find("upBody_hood").GetComponent<Renderer>().materials;

                        upBodys[1].mainTexture = GameObject.Find(clothId + "_arms").GetComponent<RawImage>().texture;
                        upBodys[0].mainTexture = GameObject.Find(clothId + "_body").GetComponent<RawImage>().texture;
                        /*
                        StartCoroutine(LoadImage(texture1Addr, clothType + "_arms"));
                        delayTime();
                        StartCoroutine(LoadImage(texture2Addr, clothType + "_body"));
                        */
                    }
                    else if (clothType == "downbody")
                    {
                        Debug.Log("downbody image download start" + texture1Addr);

                        Material[] downBody = GameObject.Find("boy_pants_basic").GetComponent<Renderer>().materials;
                        downBody[0].mainTexture = GameObject.Find(clothId).GetComponent<RawImage>().texture;
                        /*
                        StartCoroutine(LoadImage(texture1Addr, clothType));
                        */
                    }
                    else if (clothType == "cap")
                    {
                        Debug.Log("cap image download start" + texture1Addr);
                        Material[] cap = GameObject.Find("hat_baseball").GetComponent<Renderer>().materials;
                        cap[0].mainTexture = GameObject.Find(clothId).GetComponent<RawImage>().texture;

                        /*
                        StartCoroutine(LoadImage(texture1Addr, clothType));
                        */
                    }

                }
                else
                {
                    Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                }
            });

            

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator delayTime()
    {
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator LoadImage(string url, string type)
    {
        Debug.Log("Load Image start");

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if(type == "upbody_body")
            {
                Debug.Log("up body change");
                Material[] upBodys= GameObject.Find("upBody_hood").GetComponent<Renderer>().materials;
                upBodys[0].mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                
            }else if(type == "upbody_arms")
            {
                Debug.Log("up arms change");
                Material[] upBodys = GameObject.Find("upBody_hood").GetComponent<Renderer>().materials;
                upBodys[1].mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }else if(type == "downbody")
            {
                Debug.Log("up arms change");
                Material[] downBody = GameObject.Find("boy_pants_basic").GetComponent<Renderer>().materials;
                downBody[0].mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            }
            else if(type == "cap")
            {
                Debug.Log("up arms change");
                Material[] cap = GameObject.Find("hat_baseball").GetComponent<Renderer>().materials;
                cap[0].mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }
           
        }


    }
    
}
