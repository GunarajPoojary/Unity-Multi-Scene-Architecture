using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : DescriptionBaseSO
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
        else
        {
            Debug.LogWarning("A bool event was requested, but nobody picked it up. " +
                "Check why there is no SceneLoader already present, " +
                "and make sure it's listening on this Load Event channel.");
        }
    }
}