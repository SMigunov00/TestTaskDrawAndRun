using UnityEngine;
using UnityEngine.UI;

public class GameState: MonoBehaviour
{
    [SerializeField] private string _winText = "You win!";
    [SerializeField] private string _loseText = "You lose :(";
    [SerializeField] private Text _winTextUI, _loseTextUI;
    [SerializeField] private GameObject _victoryPanel, _losePanel;

   public void CrossingFinishLine()
    {
        _victoryPanel.SetActive(true);
        _winTextUI.text = _winText; 
        Time.timeScale = 0;
    }

    public void Died()
    {
        _losePanel.SetActive(true);
        _loseTextUI.text = _loseText;
        Time.timeScale = 0;
    }
}
