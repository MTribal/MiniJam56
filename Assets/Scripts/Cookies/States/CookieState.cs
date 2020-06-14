using My_Utils;

public class CookieState : State
{
    protected Cookie stateMachine;

    public CookieState(Cookie cookieStateMachine)
    {
        this.stateMachine = cookieStateMachine;
    }
}
