using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;

public class FootstepCallback : MonoBehaviour
{

    public UnityEvent<AnimationEvent> OnFootstepEvent;
    public UnityEvent<AnimationEvent> OnLandEvent;

    private void OnFootstep(AnimationEvent animationEvent)
    {
        OnFootstepEvent?.Invoke(animationEvent);
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        OnLandEvent?.Invoke(animationEvent);
    }
}
