using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    public TMP_Text PointText;
    public float PointTotal;
    // Start is called before the first frame update
    void Start()
    {
        FindAnyObjectByType<RowingGameLevelManager>().ProvidePoints.AddListener(ChangeText);
        PointText.text = $"You got: 0 points";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeText(float Points)
    {
        PointTotal += Points;
        PointText.text = $"You got: {PointTotal} points!";
    }
}
