namespace My_Utils
{
    /// <summary>
    /// Base class of all states. Inherit this class to create your states.
    /// </summary>
    public abstract class State
    {
        private bool _shuttingDown; 

        public virtual void Enter()
        {
        }

        public virtual void HandleInput()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Exit()
        {
            if (_shuttingDown) return;
            _shuttingDown = true;
        }
    }
}
