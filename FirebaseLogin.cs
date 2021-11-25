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
using System;


public class FirebaseLogin : MonoBehaviour
{

    public InputField userEmail;
    public InputField userPassword;
    public Button LoginButton;

    FirebaseAuth auth;


    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        LoginButton.onClick.AddListener(() =>
        {
            Login(userEmail.text, userPassword.text);
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Login(string email, string password)
    {
        Debug.Log(auth);
        Debug.Log(email);
        Debug.Log(password);

        bool loginSuccess = false;
        bool SignSuccess = false;
        bool newSIgnUp = false;


        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log(email + " 로 로그인 하셨습니다.");

                Firebase.Auth.FirebaseUser newUser = task.Result;

                Debug.LogFormat("사용자 로그인 성공: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
                loginSuccess = true;
                DataInfo.User.email = email;

                //NextScene();
            }
            else
            {
                Debug.Log(" 로그인에 실패하였습니다. ");

                loginSuccess = false;



            }

            if (!loginSuccess)
            {
                SignSuccess = true;
                // 기존 회원 정보 없을시 가입 진행
                auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task2 => {
                    if (task2.IsCanceled)
                    {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        return;
                    }
                    if (task2.IsFaulted)
                    {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task2.Exception);
                        return;
                    }

                    

                    // Firebase user has been created.
                    Firebase.Auth.FirebaseUser newUser2 = task2.Result;
                    Debug.LogFormat("사용자 회원가입 성공: {0} ({1})",
                        newUser2.DisplayName, newUser2.UserId);

                    DataInfo.User.email = email;

                    //NewSignUpScene();
                });

                FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
                DocumentReference docRef = db.Collection("users").Document(email);
                Dictionary<string, object> user = new Dictionary<string, object>
                {
                    { "email", email },
                };
                docRef.SetAsync(user).ContinueWithOnMainThread(task2 => {
                    Debug.Log("Added data to the alovelace document in the users collection.");
                });
            }
            else
            {
                FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
                DocumentReference docRef = db.Collection("users").Document(email);
                docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    DocumentSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                        Dictionary<string, object> user = snapshot.ToDictionary();
                        foreach (KeyValuePair<string, object> pair in user)
                        {
                            if(pair.Key == "userAddress")
                            {
                                DataInfo.User.address = pair.Value.ToString();
                            }
                            if (pair.Key == "userName")
                            {
                                DataInfo.User.name = pair.Value.ToString();
                            }
                            if (pair.Key == "userAccount")
                            {
                                DataInfo.User.account = pair.Value.ToString();
                            }
                            Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                        }
                    }
                    else
                    {
                        Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                    }
                });
            }
        });

        Debug.Log(SignSuccess);

        if (loginSuccess)
        {
            NextScene();

        }
        else if(SignSuccess)
        {
            Debug.Log("SignSUccess ");
            NewSignUpScene();
        }
        

    }


    public void NextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene2");
    }

    public void NewSignUpScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SignUp Scene");
    }
}
