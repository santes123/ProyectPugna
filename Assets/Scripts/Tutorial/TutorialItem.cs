using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItem : MonoBehaviour
{
    public GameObject floatingText;
    public TutorialSelection tutorialItem;
    public bool completed = false;
    private void Start()
    {
        floatingText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingText.SetActive(true);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //floatingText.transform.LookAt(other.gameObject.transform);
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenInterface();
                completed = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingText.SetActive(false);
        }
    }
    public void OpenInterface()
    {
        FindObjectOfType<TutorialController>().OpenTextPanelAndShowText(tutorialItem);
    }
}
