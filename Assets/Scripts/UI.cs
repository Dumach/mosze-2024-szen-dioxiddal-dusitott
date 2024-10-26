using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Text flashText;
    private int maxFlash = 3;
    private string color1;
    private string color2;

    private int flashCount = 0;             // A villanások számolása
    private bool isFlashing = false;        // Villogás folyamatban van-e

    public void startFlashing(Text flashText,int maxflesh, string hexcolor1, string hexcolor2)
    {
        maxFlash = maxflesh;
        this.color1 = hexcolor1;
        this.color2 = hexcolor2;

        if (!isFlashing)
        {
            StartCoroutine(BlinkHighScoreText());
        }
    }

    private IEnumerator BlinkHighScoreText()
    {
        isFlashing = true;

        while (flashCount < maxFlash)
        {
            ColorUtility.TryParseHtmlString(color1, out Color highlightColor);

            flashText.color = highlightColor;
            flashText.text = "New Record";

            yield return new WaitForSeconds(1);

            ColorUtility.TryParseHtmlString(color2, out Color normalColor);
            flashText.color = normalColor;
            flashText.text = "High Score";

            yield return new WaitForSeconds(1);

            flashCount++;
        }

        flashCount = 0;
        isFlashing = false;
    }
}
