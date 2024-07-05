using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "UserSO", menuName ="Scriptable Object/User Findbook")]
public class UserSO : ScriptableObject
{
    [System.Serializable]
    public class User
    {
        public string namaUser;
        [TextArea] public string tentangUser;
        public string kirimPesan;
        public string balasPesan;
    }

    public User[] user;
}
