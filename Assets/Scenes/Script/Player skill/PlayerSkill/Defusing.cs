using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Defusing : MonoBehaviour
{
    [SerializeField] Timer time;

    [SerializeField] bool isCanDefuse = false;
    [SerializeField] GameObject defuseUIButton;

    [SerializeField] float timer = 0;
    [SerializeField] float duration;
    [SerializeField] GameObject defuseProgressBarParent;
    [SerializeField] Image defuseProgressBarFill;

    [SerializeField] int timeReward;

    LookAtMouse lookAtMouse;

    private void Start()
    {
        lookAtMouse = FindObjectOfType<LookAtMouse>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isCanDefuse && time.durationInSeconds >= timeReward)
        { 
            StartCoroutine(StartDefuse());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCanDefuse = true;
            defuseUIButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCanDefuse = false;
            defuseUIButton.SetActive(false);
            ResetDefuseTime();
        }
    }

    //void StartDefuse()
    //{
    //    defuseProgressBarParent.SetActive(true);
    //    while(timer <= duration)
    //    {
    //        timer += Time.deltaTime;
    //        defuseProgressBarFill.fillAmount = timer / duration;
    //    }
        
    //    if(timer >= duration)
    //    {
    //        timer = duration;
    //        time.durationInSeconds -= timeReward;
    //    }
    //}

    IEnumerator StartDefuse()
    {
        while (timer <= duration)
        {
            defuseProgressBarParent.SetActive(true);
            timer++;
            defuseProgressBarFill.fillAmount = timer / duration;

            lookAtMouse.enabled = false;
            if (timer >= duration)
            {
                timer = duration;
                time.durationInSeconds -= timeReward;
                this.gameObject.SetActive(false);
                lookAtMouse.enabled = true;
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(1);
        }
    }

    void ResetDefuseTime()
    {
        StopAllCoroutines();
        timer = 0;
        defuseProgressBarFill.fillAmount = timer / duration;
        defuseProgressBarParent.SetActive(false);
        lookAtMouse.enabled = true;
    }
}
