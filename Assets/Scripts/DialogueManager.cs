using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public Text characterName;
    public Text dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines == null || dialogue.dialogueLines.Count == 0)
        {
            Debug.LogWarning("Dialogue is null or has no lines.");
            return;
        }

        isDialogueActive = true;

        anim.Play("NeedCoinPanel");

        if (lines == null)
            lines = new Queue<DialogueLine>();
        else
            lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            if (dialogueLine.character == null)
            {
                Debug.LogWarning("Dialogue line is missing a character.");
                continue;
            }
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }


    public void DisplayNextDialogueLine()
    {
        if(lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach(char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        anim.Play("hide");
    }
}
