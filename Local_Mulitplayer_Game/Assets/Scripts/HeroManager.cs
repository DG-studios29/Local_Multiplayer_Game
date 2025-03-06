using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    public string heroName;
    public Button selectButton;

    // Assigning the button to the hero.
    public void Initialize(int playerIndex)
    {
        selectButton.onClick.AddListener(() => HeroSelectionUI.Instance.OnHeroSelected(playerIndex, heroName));
    }
}
