using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class InputHandlerTest
{
    private GameObject inputHandlerGameObject;
    private InputHandler inputHandlerScript;

    [SetUp]
    public void Setup()
    {
        inputHandlerGameObject = new GameObject();
        inputHandlerScript = inputHandlerGameObject.AddComponent<InputHandler>();
    }

    [UnityTest]
    public IEnumerator OnClick_NoRayHit_Test()
    {
        inputHandlerScript.OnClick(new InputAction.CallbackContext());

        yield return null;

        Assert.IsNull(InputHandler.prevSlot);
    }
}
