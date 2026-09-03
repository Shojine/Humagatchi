using TMPro;
using UnityEngine;

public class OptionButton : MonoBehaviour
{ 
    [SerializeField] private GameObject moneys;
    [SerializeField] private GameObject hours;
    [SerializeField] private GameObject mood;
    [SerializeField] private GameObject moodArrow;

    [SerializeField] private TMP_Text optionText;
    [SerializeField] private TMP_Text moneysText;
    [SerializeField] private TMP_Text hoursText;

    private ActionOption currentAction;

    void Start()
    {
        PickNewAction();
    }

    public void PickNewAction()
    {
        ActionOption newAction = ResourceManager.Instance.GetAction();

        optionText.text = newAction.name;

        moneys.SetActive(false);
        hours.SetActive(false);
        mood.SetActive(false);

        switch (newAction.type)
        {
            case "purchase":
                moneys.SetActive(true);
                moneysText.text = newAction.cost.ToString();
                break;
            case "activity":
                hours.SetActive(true);
                hoursText.text = $"{newAction.cost} hrs";
                break;
            case "action":
                mood.SetActive(true);
                if (newAction.changes.happinessChange > 0)
                {
                    if (moodArrow.transform.rotation.x != 180) moodArrow.transform.Rotate(Vector3.left, 180);
                }
                else
                {
                    if (moodArrow.transform.rotation.x != 0) moodArrow.transform.Rotate(Vector3.left, 180);
                }
                break;
        }

        if (currentAction != null) ResourceManager.Instance.FreeAction(currentAction);
        currentAction = newAction;
    }

    public void OnClick()
    {
        if (currentAction.type == "purchase")
        {
            if (GameManager.Instance.RequestTotalMoney() >= currentAction.cost)
            {
                AudioManager.Instance.PlayPurchaseSFX();
                GameManager.Instance.AddMoney(-currentAction.cost);
            }
            else
            {
                AudioManager.Instance.PlayFailSFX();
                return;
            }
        }
        else if (currentAction.type == "activity")
        {
            AudioManager.Instance.PlayActivityClick();
            GameManager.Instance.AddTime(0, currentAction.cost, 0);
        }
        else AudioManager.Instance.PlayActionClick();

        HumanStateManager.Instance.ChangeHumanState(currentAction.changes);
        PickNewAction();
    }
}
