namespace My_Utils
{
    /// <summary>
    /// Store LoadType and TransitionType of all transitions. SceneLoader uses this parameters.
    /// </summary>
    public class TransitionManager : SingletonPermanent<TransitionManager>
    {
        public LoadType LastLoadType { get; private set; }

        public TransitionType LastTransitionType { get; private set; }

        public float LastAnimationRate { get; private set; }

        public void Atualize(LoadType loadType, TransitionType transitionType, float animationRate)
        {
            LastLoadType = loadType;
            LastTransitionType = transitionType;
            LastAnimationRate = animationRate;
        }
    }
}
