using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    [Header("UI Elements")]
    public Button heroButton;
>>>>>>> Stashed changes
=======
>>>>>>> parent of 43e7cd5 (Sudden death)
    public string heroName;
    public Button selectButton;

    public void Initialize(int playerIndex)
    {
        selectButton.onClick.AddListener(() => HeroSelectionUI.Instance.OnHeroSelected(playerIndex, heroName));
    }
}
