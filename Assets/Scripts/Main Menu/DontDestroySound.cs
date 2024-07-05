using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySound : MonoBehaviour
{
    public static DontDestroySound instance;

    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if(musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("dont destroy");
    }
}
