using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
<<<<<<< HEAD
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
=======
    public Button heroButton;
>>>>>>> main
    public string heroName;

    public void Initialize()
    {
        heroButton.onClick.AddListener(() => SelectHero());
    }

    void SelectHero()
    {
        HeroSelectionUI.Instance.OnHeroSelected(heroName);
    }
}
