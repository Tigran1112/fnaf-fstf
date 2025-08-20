using System.Collections;
using UnityEngine;

public class Bonnie : N1Base
{
    public GameObject[] flashlight;
    private float timeToRun;

    protected void Update()
    {
        if (!isAttacking)
        {
            CheckFlashlight();
        }
    }

    protected void CheckFlashlight()
    {
        timeToRun += Time.deltaTime;

        if (timeToRun >= 5f)
        {
            foreach (var f in flashlight)
            {
                if (f.activeSelf)
                {
                    isAttacking = true;
                    StartCoroutine(Attack());
                    timeToRun = 0f;
                    return;
                }
                else
                {
                    isAttacking = false;
                    base.GoBack();
                    timeToRun = 0f;
                }
            }
            timeToRun = 0f;
        }
    }

}