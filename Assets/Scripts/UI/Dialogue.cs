public class Dialogue
{
    public DialogueType type;
    public string text;
    public bool isDice;
    public int dicePoint;

    public Dialogue(DialogueType dialogueType, string dialogueText, bool dialogueIsDice, int dialogueDicePoint = 0)
    {
        type = dialogueType;
        text = dialogueText;
        isDice = dialogueIsDice;
        dicePoint = dialogueDicePoint;
    }
}