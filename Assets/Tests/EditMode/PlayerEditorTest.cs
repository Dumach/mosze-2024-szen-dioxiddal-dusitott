using NUnit.Framework;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class PlayerEditorTest
{
    private GameObject playerObject;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject for testing and add the script with RotateTo method
        playerObject = new GameObject();
        playerObject.AddComponent<Player>();
        playerObject.transform.position = Vector3.zero;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        Object.DestroyImmediate(playerObject);
    }

    [Test]
    public void beUnkillable_Should_Set_ShieldDuration()
    {
        float shielddur = 5f;

        // Mock player letrehozasa
        var player = playerObject.GetComponent<Player>();
        player.beUnkillable(shielddur);

        Assert.AreEqual(shielddur, player.getShieldDuration());
    }

}