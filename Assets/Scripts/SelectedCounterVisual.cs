using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Show()
    {
        visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        visualGameObject.SetActive(false);
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.onSelectedCounterChangedEventArgs e)
    {
        // Handle the event when the selected counter changes
        if (e.selectedCounter != null)
        {
            // If the selected counter is the same as this clear counter, show the visual
            if (e.selectedCounter == clearCounter)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        else
        {
            // If there is no selected counter, hide the visual
            Hide();
        }
    }
}
