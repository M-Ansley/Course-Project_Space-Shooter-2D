using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private string _sceneName = "Main";

    //private void Start()
    //{
    //    LoadScene();
    //    LoadScene("Main");
    //}

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
