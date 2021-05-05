using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteSaveFileButton : MonoBehaviour {

    public string loadingSceneName;

	public void Delete()
    {
        SaveManager.Instance.Delete();
        SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Single);
    }
}
