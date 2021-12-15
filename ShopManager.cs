using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using ButtonManagement;

[FirestoreData]
public struct ShopData
{
	[FirestoreProperty]
	public string name { get; set; }
	[FirestoreProperty]
	public string description { get; set; }
}

[FirestoreData]
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

namespace ShopManagement
{
	public class ShopManager : MonoBehaviour
	{
		public GameObject characterPrefab;
		public GameObject ItemTemplate;
		public Transform spawnPos;

		public Vector3 offset = new Vector3(0, 1, -2);

		public static GameObject myCharacter;

		public GameObject camera;

		public Text ShopName;

		public ShopData shopInfo;
		public ClothData[] clothDataes = new ClothData[100];

		public Button AllProduct;
		public Button SelectProduct;



		public Button AllProductCancel;
		public Button OneProductCancel;


		// Use this for initialization
		bool selectActive = false;


		void Start()
		{
			int count = 0;

			FirestoreDataGet();
			AllProduct.onClick.AddListener(() =>
			{
				GameObject.Find("ShopBox").transform.Find("ShopUI").gameObject.SetActive(true);
				Debug.Log("All product count");
				if(count == 0)
				{
					
				}
				
			});

			SelectProduct.onClick.AddListener(() =>
			{
				if (selectActive == false)
				{
					selectActive = true;
					SelectProduct.GetComponent<Image>().color = Color.yellow;
				}
				else
				{
					selectActive = false;
					SelectProduct.GetComponent<Image>().color = Color.white;
				}
					
			});

			AllProductCancel.onClick.AddListener(() =>
			{
				GameObject.Find("ShopBox").transform.Find("ShopUI").gameObject.SetActive(false);
				GameObject.Find("ItemBox").transform.Find("ItemUI").gameObject.SetActive(false);
				Debug.Log("일반 스페이스 키 누름");
			});

			OneProductCancel.onClick.AddListener(() =>
			{
				GameObject.Find("ShopBox").transform.Find("ShopUI").gameObject.SetActive(false);
				GameObject.Find("ItemBox").transform.Find("ItemUI").gameObject.SetActive(false);
				Debug.Log("일반 스페이스 키 누름");
			});

			characterPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			// instantiate girl character on the spawn position
			myCharacter = Instantiate(characterPrefab, new Vector3(0, 0, 5.57f), new Quaternion(0f, 0f, 0f, 1)) as GameObject;
			GameObject.Find("JoyStick").GetComponent<VirtualJoystick>().controller = myCharacter.GetComponent<CharacterMove>();

			camera = GameObject.FindWithTag("MainCamera");
			camera.GetComponent<MoveCamera>().target = myCharacter.transform;

			
		}

		public float m_DoubleClickSecond = 0.25f;
		private bool m_IsOneClick = false;
		private double m_Timer = 0;


		// Update is called once per frame
		void Update()
		{
			if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond))
			{
				Debug.Log("One Click");
				m_IsOneClick = false;
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (!m_IsOneClick)
				{
					m_Timer = Time.time;
					m_IsOneClick = true; Debug.Log("one Click");
				}
				else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond)) 
				{
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

					Physics.Raycast(ray, out hit);
					GameObject clickObj;


					if (hit.collider != null && selectActive)
					{
						FirestoreDataGet();

						clickObj = hit.collider.gameObject;
						int chknum = 0;
						if (clickObj.tag != null && int.TryParse(clickObj.tag, out chknum))
						{
							GameObject.Find("ItemBox").transform.Find("ItemUI").gameObject.SetActive(true);


							GameObject sItem = GameObject.Find("ItemBox").transform.Find("ItemUI").Find("Item").gameObject;
							int num = int.Parse(clickObj.tag);

							sItem.transform.Find("ItemInfo").Find("name").GetComponent<Text>().text = clothDataes[num].name;
							sItem.transform.Find("ItemInfo").Find("description").GetComponent<Text>().text = clothDataes[num].description;
							sItem.transform.Find("ItemInfo").Find("price").GetComponent<Text>().text = clothDataes[num].price;
							sItem.transform.Find("ItemInfo").Find("buttons").Find("Purchase").transform.tag = num.ToString();
							sItem.transform.Find("ItemInfo").Find("buttons").Find("Heart").transform.tag = num.ToString();

							GameObject itemInfo = sItem.transform.Find("ItemInfo").gameObject;
							Debug.Log(clothDataes[num].image);
							StartCoroutine(LoadImage(clothDataes[num].image, itemInfo));

						}
					}
				}
			}


		}

		void FirestoreDataGet()
		{
			string shopName = "meta_shop";//DataInfo.GameData.shopName; //for test

			FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
			


			// firebase에서 메타버스 상점이 판매중인 옷 정보 가져오기

			Query allCitiesQuery = db.Collection("shops").Document(shopName).Collection("sale").OrderBy("num");
			GameObject shopContents = GameObject.Find("ShopBox").transform.Find("ShopUI").Find("Shop").Find("Scroll Rect").Find("Contents").gameObject;

			string[] cloth_item_image = new string[15];
			GameObject[] saleItems = new GameObject[15];

			allCitiesQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
			{
				QuerySnapshot allCitiesQuerySnapshot = task.Result;
				int count = 0;
				foreach (DocumentSnapshot documentSnapshot in allCitiesQuerySnapshot.Documents)
				{
					Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
					Dictionary<string, object> cltoh_item = documentSnapshot.ToDictionary();

					GameObject saleItem = GameObject.Instantiate(ItemTemplate) as GameObject;
					saleItem.transform.parent = shopContents.transform;
					saleItem.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
					saleItems[count] = saleItem;

					foreach (KeyValuePair<string, object> pair in cltoh_item)
					{
						if (pair.Key == "num")
						{
							/*
							Texture2D texture =new Texture2D(0,0);
							string ImagePath = "/cloth0";//"cloth"+pair.Value.ToString()+".PNG";
							Debug.Log(ImagePath);
							texture = Resources.Load(ImagePath, typeof(Texture2D)) as Texture2D;
							*/
							saleItem.transform.Find("image").GetComponent<RawImage>().texture = GameObject.Find("cloth"+pair.Value.ToString()).GetComponent<RawImage>().texture;

							if (documentSnapshot.Exists)
							{
								Debug.Log(pair.Value.ToString());
								clothDataes[int.Parse(pair.Value.ToString())] = documentSnapshot.ConvertTo<ClothData>();
								saleItem.transform.Find("buttons").Find("Purchase2").transform.tag = pair.Value.ToString();
								saleItem.transform.Find("buttons").Find("Heart2").transform.tag = pair.Value.ToString();
							}
						}
						else if (pair.Key == "image")
						{
							Debug.Log("image if exe");
							

						}
						else if (pair.Key == "texture1" || pair.Key == "texture2" || pair.Key == "type")
						{
							//Debug.Log("텍스쳐값");

						}
						else
						{
							saleItem.transform.Find(pair.Key).GetComponent<Text>().text = pair.Value.ToString();
						}



					}
					count++;

				}
			});

			/*
			for(int i = 0; i < 15; i++)
			{
				Debug.Log("shop image : " + i + " : "+ cloth_item_image[i]);
				Firebase.Storage.StorageReference reference = storage.GetReferenceFromUrl(cloth_item_image[i]);
				StartCoroutine(LoadImage(cloth_item_image[i], saleItems[i]));
				delayTime();
			}
			*/
		}

		IEnumerator delayTime()
		{
			yield return new WaitForSeconds(2.0f);
		}

		IEnumerator LoadImage(string url, GameObject saleItem)
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
				saleItem.transform.Find("image").GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
			}

			
		}



	}
}

