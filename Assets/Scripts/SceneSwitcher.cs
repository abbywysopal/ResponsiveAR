using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    bool userStudyScene;
    bool tutorialScene;
    bool MRTKEXScene;
    bool designScene;
    bool openLabScene;
    // Start is called before the first frame update
    void Start()
    {
         userStudyScene = false;
         tutorialScene = false;
         MRTKEXScene = false;
         designScene = false;
         openLabScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleUserStudyScene()
    {
        if (userStudyScene)
        {
            SceneManager.UnloadSceneAsync("UserStudyScene");
            userStudyScene = false;
        }
        else
        {
            SceneManager.LoadScene("UserStudyScene", LoadSceneMode.Additive);
            userStudyScene = true;
        }
    }

    public void ToggleDuplicatesAndTables()
    {
        if (designScene)
        {
            SceneManager.UnloadSceneAsync("DuplicatesAndTables");
            designScene = false;
        }
        else
        {
            SceneManager.LoadScene("DuplicatesAndTables", LoadSceneMode.Additive);
            designScene = true;
        }
    }

    
    public void ToggleMRTKExamples()
    {
        if (MRTKEXScene)
        {
            SceneManager.UnloadSceneAsync("MRTKExamples");
            MRTKEXScene = false;
        }
        else
        {
            SceneManager.LoadScene("MRTKExamples", LoadSceneMode.Additive);
            MRTKEXScene = true;
        }
    }

    
    public void ToggleTutorial()
    {
        if (tutorialScene)
        {
            SceneManager.UnloadSceneAsync("Tutorial");
            tutorialScene = false;
        }
        else
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
            tutorialScene = true;
        }
    }


        
    public void ToggleOpenLab()
    {
        if (openLabScene)
        {
            SceneManager.UnloadSceneAsync("OpenLab");
            openLabScene = false;
        }
        else
        {
            SceneManager.LoadScene("OpenLab", LoadSceneMode.Additive);
            openLabScene = true;
        }
    }


    public void UserStudyScene()
    {
        SceneManager.LoadScene("UserStudyScene", LoadSceneMode.Additive);
    }

    public void DuplicatesAndTables()
    {
        SceneManager.LoadScene("DuplicatesAndTables", LoadSceneMode.Additive);
    }

    public void MRTKExamples()
    {
        SceneManager.LoadScene("MRTKExamples", LoadSceneMode.Additive);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
    }


}
