using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class BulletTests
{
    private GameObject bulletGameObject;
    private Bullet bulletScript;
    private GameObject targetGameObject;
    private GameObject towerGameObject;
    private Tower towerScript;

    [SetUp]
    public void SetUp()
    {
        // Set up the bullet
        bulletGameObject = new GameObject("Bullet");
        bulletScript = bulletGameObject.AddComponent<Bullet>();

        // Set up the target
        targetGameObject = new GameObject("Target");
        targetGameObject.AddComponent<BoxCollider2D>(); // Assuming the bullet uses 2D physics
        targetGameObject.transform.position = new Vector3(10, 0, 0); // Place target away from the bullet

        // Set up the mock tower
        towerGameObject = new GameObject("Tower");
        towerScript = towerGameObject.AddComponent<Tower>();
        bulletScript.tower = towerScript;
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
    public IEnumerator Bullet_MovesTowardsTarget_AndGetsDestroyed()
    {
        bulletScript.Seek(targetGameObject);
        float initialDistance = Vector3.Distance(bulletGameObject.transform.position, targetGameObject.transform.position);
        
        // Allow some time for the bullet to move
        yield return new WaitForSeconds(0.1f);

        float newDistance = Vector3.Distance(bulletGameObject.transform.position, targetGameObject.transform.position);

        // Check if the bullet moved closer to the target
        Assert.Less(newDistance, initialDistance);

        // Allow enough time for the bullet to potentially hit the target and get destroyed
        yield return new WaitForSeconds(2f);

        // Check if the bullet got destroyed after hitting the target
        Assert.IsTrue(bulletGameObject == null);
    }

    [UnityTest]
    public IEnumerator Bullet_GetsDestroyedAfterLifetime()
    {
        // Wait for a time slightly longer than the bullet's lifetime
        yield return new WaitForSeconds(bulletScript.lifeTime + 0.1f);

        // Check if the bullet got destroyed after its lifetime
        Assert.IsTrue(bulletGameObject == null);
    }

}
