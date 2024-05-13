using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using TMPro;
using Assets.Scripts;

public class BulletTests
{
    private GameObject bulletGameObject;
    private GameObject targetGameObject;
    private GameObject towerGameObject;

    private Bullet bulletScript;
    private Tower towerScript;


    [SetUp]
    public void SetUp()
    {
        // Set up the bullet
        bulletGameObject = new GameObject("Bullet");
        bulletScript = bulletGameObject.AddComponent<Bullet>();

        // Set up the target with a Collider for raycasting
        targetGameObject = new GameObject("Target");
        targetGameObject.AddComponent<BoxCollider2D>();
        targetGameObject.transform.position = new Vector3(10, 0, 0);  // Position target away from the bullet

        // Set up the mock Tower and initialize all required components
        towerGameObject = new GameObject("Tower");
        towerScript = towerGameObject.AddComponent<MockTower>();
        
        // Initialize necessary TextMeshProUGUI components and others required in Tower.Update()
        towerScript.DamageText = new GameObject("DamageText").AddComponent<TextMeshProUGUI>();
        towerScript.AttackSpeedText = new GameObject("AttackSpeedText").AddComponent<TextMeshProUGUI>();
        towerScript.RangeText = new GameObject("RangeText").AddComponent<TextMeshProUGUI>();
        // Add more initializations for other TextMeshProUGUI components and properties as needed...

        // Initialize the range preview GameObject as it's used in Tower.Start() and Tower.Update()
        var rangePreview = new GameObject("RangePreview");
        towerScript.RangePreview = rangePreview;

        // Link the bullet to the Tower
        bulletScript.Tower = towerScript;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(bulletGameObject);
        GameObject.DestroyImmediate(targetGameObject);
        GameObject.DestroyImmediate(towerGameObject);
    }

    [Test]
    public void Seek_AssignsCorrectTarget()
    {
        bulletScript.Seek(targetGameObject);
        // Use reflection to access the private 'target' field in the Bullet class
        var targetField = typeof(Bullet).GetField("target", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var targetValue = targetField.GetValue(bulletScript);
        
        Assert.AreEqual(targetGameObject, targetValue);
    }

    [UnityTest]
    public IEnumerator Bullet_GetsDestroyedAfterLifetime()
    {
        bulletScript.Seek(targetGameObject);

        // Wait for the bullet's lifetime to elapse
        yield return new WaitForSeconds(bulletScript.LifeTime + 0.1f);

        // Check if the bullet GameObject has been destroyed
        Assert.IsTrue(bulletGameObject == null || !bulletGameObject.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator Bullet_DestroysItself_WhenTargetIsNull()
    {
        bulletScript.Seek(targetGameObject);  // Set the target for the bullet
        bulletScript.Seek(null);  // Nullify the target

        // Let some time pass
        yield return new WaitForSeconds(0.1f);

        // Check if the bullet GameObject has been destroyed
        Assert.IsTrue(bulletGameObject == null, "Bullet GameObject should be destroyed when target is null.");
    }

    [UnityTest]
    public IEnumerator Bullet_MovesTowardsTarget_EachFrame()
    {
        bulletScript.Seek(targetGameObject);

        Vector3 initialPosition = bulletGameObject.transform.position;
        yield return null;  // Wait for one frame

        Vector3 newPosition = bulletGameObject.transform.position;

        // Assert the bullet has moved from its initial position towards the target
        Assert.AreNotEqual(initialPosition, newPosition);
        Assert.Less(Vector3.Distance(newPosition, targetGameObject.transform.position), Vector3.Distance(initialPosition, targetGameObject.transform.position));
    }
}

public class MockTower : Tower
{
    public bool DamageDealt { get; private set; } = false;

    public override void DealDamage(GameObject enemy)
    {
        DamageDealt = true;
    }
    
}

public class MockTarget : MonoBehaviour
{
    public bool DamageDealt { get; private set; } = false;

    public void DealDamage(float damage)
    {
        DamageDealt = true;
    }
}
