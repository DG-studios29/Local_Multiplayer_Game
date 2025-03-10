using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    public string heroName;
    public Button selectButton;

    public void Initialize(int playerIndex)
    {
        selectButton.onClick.AddListener(() => HeroSelectionUI.Instance.OnHeroSelected(playerIndex, heroName));
    }
}
