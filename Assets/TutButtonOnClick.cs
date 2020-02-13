using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutButtonOnClick : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("TutorialStart") == true)
        {
            anim.Play("New State");
            anim.ResetTrigger("TutorialStart");
        }
    }

    public void ButtonAction() {
        anim.SetTrigger("TutorialStart");

    }

    public void StopAllAnimation()
    {
        var allAnims = FindObjectsOfType<Animation>();
        foreach (var anim in allAnims)
        {
            anim.Stop();
            Debug.Log("Stopping All animations!!!!");
        }
    }
}
