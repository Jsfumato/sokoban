using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static float scale = 2;
    public BoardManager boardScript;
    public int level;

    [Header("Button")]
    public Button btLeft;
    public Button btUp;
    public Button btDown;
    public Button btRight;

    int goals;
    Text winText;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(this);
        }

        winText = GameObject.Find("WinText").GetComponent<Text>();

        DontDestroyOnLoad(this);
        boardScript = GetComponent<BoardManager>();
        goals = boardScript.SetupBoard(level);

        //
        btLeft.onClick.RemoveAllListeners();
        btLeft.onClick.AddListener(() => {
            OnMove(MoveController.Direction.LEFT);
        });
        btRight.onClick.RemoveAllListeners();
        btRight.onClick.AddListener(() => {
            OnMove(MoveController.Direction.RIGHT);
        });
        btUp.onClick.RemoveAllListeners();
        btUp.onClick.AddListener(() => {
            OnMove(MoveController.Direction.UP);
        });
        btDown.onClick.RemoveAllListeners();
        btDown.onClick.AddListener(() => {
            OnMove(MoveController.Direction.DOWN);
        });
    }

    public void CheckWin() {
        int currentGoals = 0;
        GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
        foreach (GameObject crate in crates) {
            if (crate.GetComponent<CrateController>().onGoal) {
                currentGoals += 1;
            }
        }
    }

    public void OnMove(MoveController.Direction dir) {
        if (boardScript.goPlayer == null)
            return;

        boardScript.goPlayer.GetComponent<PlayerController>().MovePlayer(dir);
    }
}