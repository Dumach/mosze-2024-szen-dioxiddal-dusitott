using NUnit.Framework;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class GameManagerEditorTest
{
    [Test]
    public void GameManager_Singleton_Instance_Should_Be_Unique()
    {
        // Elsõ Gamemanager létrehozása
        var firstInstance = new GameObject().AddComponent<GameManager>(); ;
        firstInstance.Awake();

        Assert.IsNotNull(GameManager.Instance);
        Assert.AreEqual(firstInstance, GameManager.Instance);

        // Második mock gamemanager létrehozása
        var secondInstance = new GameObject().AddComponent<GameManager>();

        secondInstance.Awake();

        // A második példánynak üresnek kéne lenni.
        Assert.IsTrue(secondInstance == null);
    }

    [Test]
    public void OnPlayerKilled_Should_Decrease_Player_Health()
    {
        // Mock player létrehozása
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();
        // Spriterendere hozzáadása különben hibára fut
        playerObject.AddComponent<SpriteRenderer>();
        player.acquireSpriteRenderer();

        player.health = 3;

        var gameManager = new GameObject().AddComponent<GameManager>();

        gameManager.OnPlayerKilled();

        Assert.AreEqual(2, player.health);
    }

}
