using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloseMessageTutorial : MonoBehaviour
{
    public void ClosePanel()
    {
        FindObjectOfType<TutorialController>().ClosePanelText();
    }
}
