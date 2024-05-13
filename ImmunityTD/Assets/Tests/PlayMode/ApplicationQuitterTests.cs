using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ApplicationQuitterTests
{
    private IApplicationQuitter quitter;

    [SetUp]
    public void SetUp()
    {
        quitter = new ApplicationQuitter();
    }

    [TearDown]
    public void TearDown()
    {
        quitter = null;
    }

    [UnityTest]
    public IEnumerator QuitApplication()
    {
        quitter.Quit();

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(ApplicationQuitChecker.IsQuitting);
    }

    private static class ApplicationQuitChecker
    {
        private static bool isQuitting = false;

        public static bool IsQuitting
        {
            get { return isQuitting; }
        }

        [RuntimeInitializeOnLoadMethod]
        static void OnApplicationQuit()
        {
            isQuitting = true;
        }
    }
}
