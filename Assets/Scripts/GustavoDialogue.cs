using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GustavoDialogue : MonoBehaviour
{
    [SerializeField] public Detection_controller detectionArea;
    [SerializeField] public DialogueTrigger dialogueTrigger;

    private void Update()
    {
        if (detectionArea.detectedObjs.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            dialogueTrigger.TriggerDialogue();
        }
    }
}
