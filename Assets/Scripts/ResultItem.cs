using TMPro;
using UnityEngine;

public class ResultItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ratingText;

    public void SetData(string username, float rating)
    {
        nameText.text = username;
        ratingText.text = $"%{rating:0.00}";
    }
}