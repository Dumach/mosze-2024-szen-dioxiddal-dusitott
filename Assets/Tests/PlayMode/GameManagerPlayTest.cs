using System;
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
    public IEnumerator OnInvaderKilled_Should_Increase_Score_And_Destroy_Invader()
    {
        // You have to load a scene otherwise play mode test fails
        SceneManager.LoadScene("test");

        yield return null;

        var gameManagerObj = GameObject.Find("GameManager");
        var invaderObj = GameObject.Find("Invader_01");        

        var gamemanagerComp = gameManagerObj.GetComponent<GameManager>();
        var invaderComp = invaderObj.GetComponent<Invader>();

        gamemanagerComp.OnInvaderKilled(invaderComp);

        yield return null;

        Assert.AreEqual(0, invaderComp.health); 
        Assert.AreEqual(10, gamemanagerComp.score); 
        Assert.IsTrue(invaderObj == null); 
    }

}
