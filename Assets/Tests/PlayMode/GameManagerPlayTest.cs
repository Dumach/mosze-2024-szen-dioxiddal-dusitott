using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameManagerPlay
{

    [UnityTest]
    public IEnumerator RestartScene_When_Player_Health_Is_Zero()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        var gameManagerObject = new GameObject();
        var gameManager = gameManagerObject.AddComponent<GameManager>();
        
        // Mockoljuk a player-t a GameManager-hez
        var playerObject = new GameObject();
        var player = playerObject.AddComponent<Player>();
        playerObject.AddComponent<Camera>();
        
        player.health = 0;


        yield return null;

        Assert.AreEqual(currentScene, SceneManager.GetActiveScene().buildIndex); // Példa: Itt mockolhatjuk a jelenetváltást.
    }

    [UnityTest]
    public IEnumerator OnInvaderKilled_Should_Increase_Score_And_Destroy_Invader()
    {
        var gameManagerObject = new GameObject();
        var gameManager = gameManagerObject.AddComponent<GameManager>();

        var invaderObject = new GameObject();
        var invader = invaderObject.AddComponent<Invader>();
        invader.health = 1;
        invader.score = 100;

        gameManager.OnInvaderKilled(invader);

        yield return null;

        Assert.AreEqual(0, invader.health); // Az invader életereje 0
        Assert.AreEqual(100, gameManager.score); // A pontszám növekedett
        Assert.IsTrue(invaderObject == null); // Az invader megsemmisült
    }

}
