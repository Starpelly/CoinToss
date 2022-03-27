using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LootLocker.Requests;
using UnityEngine.EventSystems;
using EndlessGames;

public class LeaderboardController : MonoBehaviour
{
    public TMP_InputField MemberID, PlayerScore;
    public GameObject PlayerScoreTemplate;
    public int ID;
    int maxScores = 100;
    [Space(20)]
    public GameObject RectHolder, InputHolder;
    public int currentScore;
    public bool savedScore;
    private int openTimes;
    public EventSystem eventSystem;
    public Button leaderboardButton;

    public static LeaderboardController instance;

    private void Start()
    {
        instance = this;
        LootLockerSDKManager.StartSession("Player", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failed");
            }
        });

        // Tooltip.AddTooltip(leaderboardButton.gameObject, "Leaderboards");
    }

    private void Update()
    {
        EndlessGames.DSGuy.instance.enabled = !eventSystem.IsPointerOverGameObject();
    }

    public void Show()
    {
        if (!RectHolder.activeInHierarchy)
        {
            RectHolder.SetActive(true);
            // InputHolder.SetActive(true);

            if (savedScore == true || openTimes == 0)
            {
                if (savedScore == true)
                    savedScore = false;

                ShowScores();
            }
            openTimes++;
        }
        else
        {
            RectHolder.SetActive(false);
        }
    }

    public void CloseInputField()
    {
        InputHolder.SetActive(false);
    }

    public void ShowScores()
    {
        for (int i = 1; i < PlayerScoreTemplate.transform.parent.childCount; i++)
            Destroy(PlayerScoreTemplate.transform.parent.GetChild(i).gameObject);

        LootLockerSDKManager.GetScoreList(ID, maxScores, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++)
                {
                    GameObject playerScoreTemplate = Instantiate(PlayerScoreTemplate.gameObject, PlayerScoreTemplate.transform.parent);
                    playerScoreTemplate.transform.GetChild(0).GetComponent<TMP_Text>().text = (scores[i].rank.ToString());
                    playerScoreTemplate.transform.GetChild(1).GetComponent<TMP_Text>().text = (scores[i].member_id);
                    playerScoreTemplate.transform.GetChild(2).GetComponent<TMP_Text>().text = (scores[i].score.ToString());
                    playerScoreTemplate.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Failed");
            }
        });
    }

    public void SubmitScore()
    {
        if (MemberID.text.Length > 0)
        {
            LootLockerSDKManager.SubmitScore(MemberID.text, currentScore, ID, (response) =>
            {
                if (response.success)
                {
                    InputHolder.SetActive(false);
                    savedScore = true;
                }
                else
                {
                    Debug.Log("Failed");
                }
            });
        }
    }
}
