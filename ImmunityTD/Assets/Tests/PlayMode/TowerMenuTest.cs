using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TowerMenuTest
{
    private GameObject towerMenuGameObject;
    private TowerMenu towerMenuScript;
    private GameObject towerMenuCanvas;

    [SetUp]
    public void Setup()
    {
        towerMenuGameObject = new GameObject();
        towerMenuScript = towerMenuGameObject.AddComponent<TowerMenu>();
        towerMenuCanvas = new GameObject();
        towerMenuScript.towerMenuCanvas = towerMenuCanvas;
    }

    [UnityTest]
    public IEnumerator OnMouseDown_NoCanvas_AssignErrorLogged_Test()
    {
        towerMenuScript.towerMenuCanvas = null;
        Debug.unityLogger.logEnabled = false;
        towerMenuScript.OnMouseDown(); 
        yield return null;
        Debug.unityLogger.logEnabled = true;
        LogAssert.Expect(LogType.Error, "Tower menu canvas is not assigned to the tower prefab!");
    }

    [UnityTest]
    public IEnumerator RemoveTower_Test()
    {
        Slot slot = new GameObject().AddComponent<Slot>();
        TowerMenu.currentSlot = slot;
        towerMenuScript.RemoveTower();
        yield return null;
        Assert.IsNull(towerMenuGameObject);
        Assert.IsTrue(slot.gameObject.activeSelf);
    }
}
