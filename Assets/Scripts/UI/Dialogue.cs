public class Dialogue
{
    public DialogueType type;
    public string text;
    public bool isPlaySound;

    public Dialogue(DialogueType dialogueType, string dialogueText, bool dialogueIsPlaySound)
    {
        type = dialogueType;
        text = dialogueText;
        isPlaySound = dialogueIsPlaySound;
    }
}