using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Concurrent; // ConcurrentStack
using System.Threading;
using System.Threading.Tasks;


namespace DataInfo
{
    public struct ICloth
    {
        public string num;
        public string shop;
        public ICloth(string n, string s)
        {
            num = n;
            shop = s;
        }
    }


    [System.Serializable]
    public static class GameData
    {
        public static string shopName;
    }


    [System.Serializable]
    public static class User
    {
        public static string email;
        public static string address;
        public static string account;
        public static string name;
        public static Stack<ICloth> basketCloth = new Stack<ICloth>();
        public static Stack<ICloth> heartCloth = new Stack<ICloth>();
    }


    [System.Serializable]
    public static class shopUI
    {
        public static string shopTag;
    }
}


