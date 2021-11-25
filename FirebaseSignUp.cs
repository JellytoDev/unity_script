using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using DataInfo;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;


public class FirebaseSignUp : MonoBehaviour
{

    public InputField userName;
    public InputField userAddress;
    public InputField userAccount;
    public Button ContinueBtn;


    // Start is called before the first frame update
    void Start()
    {
        ContinueBtn.onClick.AddListener(() =>
        {
            string email = DataInfo.User.email;
            SignUp(email, userName.text, userAddress.text, userAccount.text);

            DataInfo.User.address = userAddress.text;
            DataInfo.User.account = userAccount.text;
            DataInfo.User.name = userName.text;

            NextScene();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUp(string email, string userName, string userAddress,string userAccount)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        DocumentReference userDataRef = db.Collection("users").Document(email);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
                { "userName", userName },
                { "userAddress",userAddress },
                { "userAccount", userAccount }
        };

        userDataRef.UpdateAsync(updates).ContinueWithOnMainThread(task => {
            Debug.Log(
                    "Updated the Capital field of the new-city-id document in the cities collection.");
        });
        // You can also update a single field with: cityRef.UpdateAsync("Capital", false);
    }

    public void NextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene2");
    }
}
