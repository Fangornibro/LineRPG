using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<Image> characterDisplayImages;
    public List<PlayerSelection> characters;
    [SerializeField] private Sprite lockedCharacter;
    [SerializeField] private TextMeshProUGUI nameText, descriptionText;
    private void Start()
    {
        CharacterDisplay();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneTransition.SwitchToScene("MainMenu");
        }
    }
    public void ChangeSelectionRight()
    {
        List<PlayerSelection> temp = new List<PlayerSelection>();
        for (int i = 1; i < characters.Count; i++)
        {
            temp.Add(characters[i]);
        }
        temp.Add(characters[0]);
        characters = temp;
        CharacterDisplay();
    }

    public void ChangeSelectionLeft()
    {
        List<PlayerSelection> temp = new List<PlayerSelection> { characters[characters.Count - 1] };
        for (int i = 0; i < characters.Count - 1; i++)
        {
            temp.Add(characters[i]);
        }
        characters = temp;
        CharacterDisplay();
    }

    private void CharacterDisplay()
    {
        if (characters[2].isLocked)
        {
            nameText.SetText("Locked");
            descriptionText.SetText("");
        }
        else
        {
            nameText.SetText(characters[2].name);
            descriptionText.SetText(characters[2].description);
        }
        for (int i = 0; i < characterDisplayImages.Count; i++)
        {
            characterDisplayImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(108, 198);
            if (characters[i].isLocked)
            {
                characterDisplayImages[i].sprite = lockedCharacter;
            }
            else
            {
                characterDisplayImages[i].sprite = characters[i].GetComponent<SpriteRenderer>().sprite;
            }
            float iconWidth = characterDisplayImages[i].GetComponent<RectTransform>().sizeDelta.x / characterDisplayImages[i].sprite.rect.width;
            float iconHeight = characterDisplayImages[i].GetComponent<RectTransform>().sizeDelta.y / characterDisplayImages[i].sprite.rect.height;
            characterDisplayImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(characterDisplayImages[i].sprite.rect.width * Mathf.Min(iconWidth, iconHeight), characterDisplayImages[i].sprite.rect.height * Mathf.Min(iconWidth, iconHeight));
        }
    }
}
