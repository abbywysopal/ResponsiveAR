using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Audio;
using UnityEngine.SceneManagement;
using System.Threading;
//using UnityEngine.CoreModule;

public class TutorialSteps : MonoBehaviour
{

    [SerializeField]
    List<GameObject> Steps;

    [SerializeField] GameObject Slate;

    private static bool step_complete;
    private int current_step;
    private int total_steps;
    public string correct_answer;

    public bool complete(string answer)
    {
        Debug.Log("current_step: " + current_step.ToString());
        Debug.Log(answer.Length + ", " + correct_answer.Length);
        Debug.Log("answer entered: " + answer);
        Debug.Log("correct_answer: " + correct_answer);

        if (answer.CompareTo(correct_answer) == 0)
        {
            step_complete = true;
            NextStep();
            return true;
        }

        Debug.Log("INCORRECT");

        return false;
    }

    public bool touchpad_enter(string entered_num)
    {
        return complete(entered_num);
    }

    // Start is called before the first frame update
    void Start()
    {
        Slate.SetActive(false);
        current_step = 0;
        setUpTask();
        StartTutorial();
    }

    public void StartTutorial()
    {
        step_complete = false;
        Steps[0].SetActive(true);
        TextToSpeech tts = Steps[0].transform.GetComponent<TextToSpeech>();
        Transform tran = Steps[0].transform.Find("DescriptionText");
        TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
        Debug.Log("start speaking");
        tts.StartSpeaking(tmp.text);

    }

    public void NextStep()
    {
        Steps[current_step].SetActive(false);
        current_step += 1;
        correct_answer = current_step.ToString();
        if (current_step < total_steps)
        {
            Debug.Log("NextStep " + current_step);
            Steps[current_step].SetActive(true);
            TextToSpeech tts = Steps[current_step].transform.GetComponent<TextToSpeech>();
            Transform tran = Steps[current_step].transform.Find("DescriptionText");
            TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
            tts.StartSpeaking(tmp.text);
        }
        else
        {
            Slate.SetActive(false);
            SceneManager.LoadScene("UserStudyScene", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Tutorial");
        }
    }

    public void setUpTask()
    {
        total_steps = Steps.Count;
        foreach(GameObject g in Steps)
        {
            g.SetActive(false);
        }

    }

    public void EnterAnswer(TextMeshProUGUI text)
    {
        string input = text.text.Substring(0, text.text.Length - 1).ToLower();
        input = input.Trim();
        complete(input);
    }

    public void closeKeyboard(GameObject keyboard)
    {
        if (step_complete)
        {
            keyboard.SetActive(false);
        }
    }

    public void EnterAnswer(string text)
    {
        Debug.Log("pressed text: " + text);
        complete(text);
    }

    public void adjustKeyboard(GameObject g)
    {
        g.transform.position = Camera.main.transform.position + new Vector3( 0.053f, -0.2f, 0.5f);
    }

}
