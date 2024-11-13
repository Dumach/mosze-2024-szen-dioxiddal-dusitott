using NUnit.Framework;
using UnityEngine;

public class ProjectileEditorTest
{
    private GameObject projectileObject;
    private Transform target;
    private Projectile projectile;

    [SetUp]
    public void Setup()
    {
        // Mock lovedek hozzadasa
        projectileObject = new GameObject();
        projectile = projectileObject.AddComponent<Projectile>(); 

        // Mock target hozzadasa
        target = new GameObject().transform;
    }

    [TearDown]
    public void Teardown()
    {
        // Destroy the objects after each test
        Object.DestroyImmediate(projectileObject);
        Object.DestroyImmediate(target.gameObject);
    }

    [Test]
    public void SetDirection_WithTarget_Should_RotateTowards_Target()
    {
        // Mock helyzet allitasa
        projectileObject.transform.position = Vector3.zero;
        target.position = new Vector3(1, 1, 0);

        Vector3 expectedDirection = (target.position - projectileObject.transform.position).normalized;
        float expectedAngle = Mathf.Atan2(expectedDirection.y, expectedDirection.x) * Mathf.Rad2Deg + 90f;

        projectile.SetDirection(target);

        Assert.AreEqual(expectedDirection, projectile.direction, "Direction was not set correctly towards the target.");
        Assert.AreEqual(expectedAngle, projectileObject.transform.rotation.eulerAngles.z, 0.1f, "Rotation angle was not set correctly.");
    }

    [Test]
    public void SetDirection_WithoutTarget_ShouldBe_Upward()
    {
        // Ha nincs target felfele kell mennie a lovedeknek
        projectileObject.transform.position = Vector3.zero;
        Vector3 expectedDirection = Vector3.up; 

        projectile.SetDirection(null);

        Assert.AreEqual(expectedDirection, projectile.direction, "Direction was not set to the default upward direction.");
        Assert.AreEqual(0f, projectileObject.transform.rotation.eulerAngles.z, 0.1f, "Rotation was not set to the default angle for upward direction.");
    }
}
