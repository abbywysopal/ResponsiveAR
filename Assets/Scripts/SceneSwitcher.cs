using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UserStudyScene()
    {
        SceneManager.LoadScene("UserStudyScene", LoadSceneMode.Single);
        SceneManager.LoadScene("OpeningScene", LoadSceneMode.Additive);
    }

    public void DuplicatesAndTables()
    {
        SceneManager.LoadScene("DuplicatesAndTables", LoadSceneMode.Single);
        SceneManager.LoadScene("OpeningScene", LoadSceneMode.Additive);
    }

    public void MRTKExamples()
    {
        SceneManager.LoadScene("MRTKExamples", LoadSceneMode.Single);
        SceneManager.LoadScene("OpeningScene", LoadSceneMode.Additive);
    }

}
