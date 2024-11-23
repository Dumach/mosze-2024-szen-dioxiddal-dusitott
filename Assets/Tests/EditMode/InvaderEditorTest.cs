using NUnit.Framework;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class InvaderEditorTest
{
    private GameObject invaderObject;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject for testing and add the script with RotateTo method
        invaderObject = new GameObject();
        invaderObject.AddComponent<Invader>();
        invaderObject.transform.position = Vector3.zero;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        Object.DestroyImmediate(invaderObject);
    }

    [Test]
    public void RotateTo_ShouldFaceCorrectAngle_WhenTargetIsSpecified()
    {
        // Mock target es irany kiszamitasa
        Vector3 targetPosition = new Vector3(1, 1, 0);
        float expectedAngle = Mathf.Atan2(1, 1) * Mathf.Rad2Deg + 90f; 

        invaderObject.GetComponent<Invader>().RotateTo(targetPosition);
        float actualAngle = invaderObject.transform.rotation.eulerAngles.z;

        Assert.AreEqual(expectedAngle, actualAngle, 0.1f, "The object did not rotate to the expected angle.");
    }

}