using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialText;

    public Image elara;
    public Image liana;

    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        ShowDialogue("Liana", "No puedo creer que esta sea nuestra última noche juntas…", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
                ShowDialogue("Elara", "Y si lo abrimos ahora?", 0);
          
        }
    }

    public void ShowDialogue(string speakerName, string dialogue, int speakingCharacter)
    {
        // Update the name and dialogue text boxes
        nameText.text = speakerName;
        dialText.text = dialogue;

        // Adjust character brightness based on who is speaking
        switch (speakingCharacter)
        {
            case 0: // Character on the left is speaking
                nameText.gameObject.SetActive(true);
                elara.color = activeColor;
                liana.color = inactiveColor;
                break;

            case 1: // Character on the right is speaking
                nameText.gameObject.SetActive(true);
                elara.color = inactiveColor;
                liana.color = activeColor;
                break;

            default: // Narrator or a character not on screen is speaking
                // Hide the name box if the name is empty, or show it for "Narrator"
                if (string.IsNullOrEmpty(speakerName))
                {
                    nameText.gameObject.SetActive(false);
                }
                else
                {
                    nameText.gameObject.SetActive(true);
                }

                // Set both characters to an active or neutral state. You can change this logic.
                // For a narrator, you might want both to be slightly inactive.
                elara.color = activeColor;
                liana.color = activeColor;
                break;
        }
    }
}
