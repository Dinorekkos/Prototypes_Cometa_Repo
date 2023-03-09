using TMPro;
using Unity.Netcode;

public class ScoreUI : NetworkBehaviour
{
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public PongGameManager gameManager;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameManager.Player1Score.OnValueChanged += OnPlayer1ScoreUpdate;
        gameManager.Player2Score.OnValueChanged += OnPlayer2ScoreUpdate;
    }

    public void OnPlayer1ScoreUpdate(int prevValue, int newValue)
    {
        player1ScoreText.text = "" + newValue;
    }

    public void OnPlayer2ScoreUpdate(int prevValue, int newValue)
    {
        player2ScoreText.text = "" + newValue;
    }
}
