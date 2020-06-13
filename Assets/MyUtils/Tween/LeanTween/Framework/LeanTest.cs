using UnityEngine;
using System.Collections;

public class LeanTester : MonoBehaviour
{
    public float timeout = 15f;

    public void Start()
    {
        StartCoroutine(TimeoutCheck());
    }

    IEnumerator TimeoutCheck()
    {
        float pauseEndTime = Time.realtimeSinceStartup + timeout;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        if (LeanTest.testsFinished == false)
        {
            Debug.Log(LeanTest.FormatB("Tests timed out!"));
            LeanTest.Overview();
        }
    }
}

public class LeanTest : object
{
    public static int expected = 0;
    private static int tests = 0;
    private static int passes = 0;

    public static float timeout = 15f;
    public static bool timeoutStarted = false;
    public static bool testsFinished = false;

    public static void Debug(string name, bool didPass, string failExplaination = null)
    {
        Expect(didPass, name, failExplaination);
    }

    public static void Expect(bool didPass, string definition, string failExplaination = null)
    {
        float len = PrintOutLength(definition);
        int paddingLen = 40 - (int)(len * 1.05f);
#if UNITY_FLASH
		string padding = padRight(paddingLen);
#else
        string padding = "".PadRight(paddingLen, "_"[0]);
#endif
        string logName = FormatB(definition) + " " + padding + " [ " + (didPass ? FormatC("pass", "green") : FormatC("fail", "red")) + " ]";
        if (didPass == false && failExplaination != null)
            logName += " - " + failExplaination;
        UnityEngine.Debug.Log(logName);
        if (didPass)
            passes++;
        tests++;

        // Debug.Log("tests:"+tests+" expected:"+expected);
        if (tests == expected && testsFinished == false)
        {
            Overview();
        }
        else if (tests > expected)
        {
            UnityEngine.Debug.Log(FormatB("Too many tests for a final report!") + " set LeanTest.expected = " + tests);
        }

        if (timeoutStarted == false)
        {
            timeoutStarted = true;
            GameObject tester = new GameObject();
            tester.name = "~LeanTest";
            LeanTester test = tester.AddComponent(typeof(LeanTester)) as LeanTester;
            test.timeout = timeout;
#if !UNITY_EDITOR
			tester.hideFlags = HideFlags.HideAndDontSave;
#endif
        }
    }

    public static string PadRight(int len)
    {
        string str = "";
        for (int i = 0; i < len; i++)
        {
            str += "_";
        }
        return str;
    }

    public static float PrintOutLength(string str)
    {
        float len = 0.0f;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == "I"[0])
            {
                len += 0.5f;
            }
            else if (str[i] == "J"[0])
            {
                len += 0.85f;
            }
            else
            {
                len += 1.0f;
            }
        }
        return len;
    }

    public static string FormatBC(string str, string color)
    {
        return FormatC(FormatB(str), color);
    }

    public static string FormatB(string str)
    {
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
		return str;
#else
        return "<b>" + str + "</b>";
#endif
    }

    public static string FormatC(string str, string color)
    {
        return "<color=" + color + ">" + str + "</color>";
    }

    public static void Overview()
    {
        testsFinished = true;
        int failedCnt = (expected - passes);
        string failedStr = failedCnt > 0 ? FormatBC("" + failedCnt, "red") : "" + failedCnt;
        UnityEngine.Debug.Log(FormatB("Final Report:") + " _____________________ PASSED: " + FormatBC("" + passes, "green") + " FAILED: " + failedStr + " ");
    }
}
