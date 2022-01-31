using UnityEngine;

public interface IAppStateListener
{
    void AppStart();
    void RenderStateChanged(bool newValue);
    void AppQuit();

    void OnActivityStart(ActivityEventData eventData);
    void OnActivityStop(ActivityEventData eventData);
    void OnStateChanged(ExecutionState executionState);

    void OnMessage(string methodName, object value, SendMessageOptions options);
}