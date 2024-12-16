using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCreditsManager : MonoBehaviour
{
    [SerializeField] private Animator creditsAnimator;

    private void Start()
    {
        if (creditsAnimator != null)
        {
            creditsAnimator.SetTrigger("PlayCredits");
        }
    }

    // Back to Main Menu after EndCredits finished
    public void OnEndCreditsFinished()
    {
        SceneManager.LoadScene("Main Menu"); 
    }
}

