using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blackOut : MonoBehaviour
{

    public Image brownGround;
    public Image blackScreen;



    float blackScreenAlpha = 1;
    float brownGroundAlpha = 1;


    float roundTImer = 0;
    float roundTImer2 = 0;
    float roundTImer3 = 0;
    float roundTImer4 = 0;
  
    // Update is called once per frame
    void Update()
    {
        roundTImer2 += Time.deltaTime;
        if(roundTImer2 >= 0.2)
        {
            roundTImer += Time.deltaTime;
        }
        blackScreenAlpha = 1 - roundTImer / 2.5f;

        if(blackScreenAlpha <= 0)
        {
            roundTImer3 += Time.deltaTime;
        }
        if(roundTImer3 >= 0.6f)
        {
            roundTImer4 += Time.deltaTime;
        }

        brownGroundAlpha = 1 - roundTImer4 / 3;

        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreenAlpha);
        brownGround.color = new Color(brownGround.color.r, brownGround.color.g, brownGround.color.b, brownGroundAlpha);
        


    }
}
