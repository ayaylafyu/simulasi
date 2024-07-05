using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLDebugging : MonoBehaviour
{
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            CheckWebGLExtensions();
        }
    }

    void CheckWebGLExtensions()
    {
        Application.ExternalEval(@"
            var result = CheckWebGLExtensions();
            var unityInstance = unityInstance || null;
            if (unityInstance != null) {
                unityInstance.SendMessage('WebGLDebugger', 'OnCheckWebGLExtensionsComplete', result);
            }
        ");
    }

    public void OnCheckWebGLExtensionsComplete(string result)
    {
        string[] supports = result.Split(',');
        bool supportsOESTextureFloat = supports[0] == "True";
        bool supportsEXTColorBufferFloat = supports[1] == "True";

        Debug.Log("OES_texture_float support: " + supportsOESTextureFloat);
        Debug.Log("EXT_color_buffer_float support: " + supportsEXTColorBufferFloat);
    }
}
