using UnityEngine;

[CreateAssetMenu(fileName = "TransitionSettings", menuName = "ScriptableObjects/TransitionSettings")]
public class TransitionSettings : ScriptableObject
{
    [Tooltip("The real animation time in seconds.")]
    public float animationDuration;

    [Tooltip("How fast the animation should play.")]
    public float animationRate;
}
