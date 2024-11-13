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

    // Van egy érzésem hogy ez play mode-ban kéne futtatni
    /*[Test]
    public void OnPlayerKilled_Should_Decrease_Player_Health()
    {
        // Mock player létrehozása
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();
        // Spriterendere hozzáadása különben hibára fut
        // playerObject.AddComponent<SpriteRenderer>();
        player.acquireSpriteRenderer();

        player.health = 3;

        var gameManager = new GameObject().AddComponent<GameManager>();
        gameManager.GetType()
            .GetField("player", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameManager, player);

        gameManager.OnPlayerKilled();

        Assert.AreEqual(2, player.health);
    }*/

    [Test]
    public void WeaponUpgrade_Should_Not_Go_Over_Limit()
    {
        var gameManager = new GameObject().AddComponent<GameManager>();

        // Mock player létrehozása
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();

        // Mock weapon template-k létrehozása
        for (int i = 0; i < 3; i++)
        {
            player.upgradeTemplates.Add(new GameObject().AddComponent<Player>());
        }
        player.currentTemplate = -1;

        gameManager.GetType()
            .GetField("player", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameManager, player);


        gameManager.UpgradeWeapons();
        Assert.AreEqual(0, player.currentTemplate);

        gameManager.UpgradeWeapons();
        gameManager.UpgradeWeapons();

        Assert.AreEqual(2, player.currentTemplate);

        // Túltöltés tesztelése
        gameManager.UpgradeWeapons();
        Assert.AreEqual(2, player.currentTemplate);
    }

    /*
    [Test]
    public void GameManager_Should_Set_Score_Correctly()
    {
        var gameManager = new GameObject().AddComponent<GameManager>();
        gameManager.SetScore(100);
        Assert.AreEqual(100, gameManager.score);
    }*/

    [Test]
    public void Heal_Player_Should_Increase_Health()
    {
        // Mock player létrehozása
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();
        player.health = 3;

        var gameManager = new GameObject().AddComponent<GameManager>();
        gameManager.GetType()
            .GetField("player", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameManager, player);
        gameManager.GetType()
            .GetField("maxHealth", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(gameManager, 3);

        Assert.AreEqual(3, player.health);
        
        // Mock sebesulest szenved el
        player.health -= 1;
        gameManager.HealPlayer();

        Assert.AreEqual(3, player.health);
    }
}
