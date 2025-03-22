using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    public Button heroButton;
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
