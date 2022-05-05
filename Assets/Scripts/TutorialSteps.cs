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

    private long start_time;
    private long end_time;
    private string description;

    SceneStudyManager record;
    bool isRecording = false;

    private static List<ExperimentEventData> currentEvents;

    public bool complete(string answer)
    {
        Debug.Log("current_step: " + current_step.ToString());
        Debug.Log(answer.Length + ", " + correct_answer.Length);
        Debug.Log("answer entered: " + answer);
        Debug.Log("correct_answer: " + correct_answer);

        if (answer.CompareTo(correct_answer) == 0)
        {
            log_data("end", answer);
            end_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            record.startNewSession(start_time, end_time, description, false, end_time - start_time);
            step_complete = true;
            NextStep();
            return true;
        }

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
        record = gameObject.GetComponent<SceneStudyManager>();
        current_step = 0;
        setUpTask();
        StartTutorial();
    }

    public void StartTutorial()
    {
        step_complete = false;
        currentEvents = new List<ExperimentEventData>();
        Steps[0].SetActive(true);
        TextToSpeech tts = Steps[0].transform.GetComponent<TextToSpeech>();
        Transform tran = Steps[0].transform.Find("DescriptionText");
        TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
        Debug.Log("start speaking");
        tts.StartSpeaking(tmp.text);
        record.startRecordingTutorial();
        isRecording = true;

    }

    public void NextStep()
    {
        Steps[current_step].SetActive(false);
        current_step += 1;
        correct_answer = current_step.ToString();
        if (current_step < total_steps)
        {
            Debug.Log("NextStep " + current_step);
            log_data("start", "");

            /*
            if(current_step > 2)
            {
                 Slate.transform.position = Camera.main.transform.position + Slate.transform.position;
            }
            else
            {
                 Steps[current_step].transform.position = Camera.main.transform.position + Steps[current_step].transform.position;
            }
            */

            Steps[current_step].SetActive(true);
            TextToSpeech tts = Steps[current_step].transform.GetComponent<TextToSpeech>();
            Transform tran = Steps[current_step].transform.Find("DescriptionText");
            TextMeshPro tmp = tran.GetComponent<TextMeshPro>();
            tts.StartSpeaking(tmp.text);
            description = tmp.text;
            start_time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        else
        {
            Slate.SetActive(false);
            record.stopRecording();
            isRecording = false;
            SceneManager.LoadScene("UserStudyScene", LoadSceneMode.Single);
        }
    }

    public void skipTutorial()
    {
        Steps[current_step].SetActive(false);
        Slate.SetActive(false);
        Slate.SetActive(false);
        record.stopRecording();
        SceneManager.LoadScene("UserStudyScene", LoadSceneMode.Single);
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
        log_data("entered", input);
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
        log_data("entered", text);
        complete(text);
    }

    public void adjustKeyboard(GameObject g)
    {
        g.transform.position = Camera.main.transform.position + new Vector3( 0.053f, -0.2f, 0.5f);
    }

    private void log_data(string type, string guess)
    {
        ExperimentEventData currentEventData = new ExperimentEventData();
        currentEventData.unixTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        currentEventData.systemTime = System.DateTime.Now.ToString("HH-mm-ss-ff");
        currentEventData.isResponsive = false;
        currentEventData.eventName = type;
        currentEventData.task_number = current_step;

        currentEventData.task_type = Steps[current_step].name;
        currentEventData.object_position = Steps[current_step].transform.position;
        currentEventData.object_scale = Steps[current_step].transform.localScale;

        
        currentEventData.correct_answer = correct_answer;
        currentEventData.guess = guess;
        currentEvents.Add(currentEventData);
    }

    
    public static List<ExperimentEventData> GetExperimentEventData()
    {
        List<ExperimentEventData> eventDataCopy = currentEvents;
        currentEvents = new List<ExperimentEventData>();
        return eventDataCopy;
    }

}
