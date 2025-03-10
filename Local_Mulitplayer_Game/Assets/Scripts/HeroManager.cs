using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> parent of 93b5622 (Sudden death)
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
