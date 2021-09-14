using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public int riches;
    public Text richesText;

    void Start()
    {
        
    }

    void Update()
    {
        richesText.text = ("" + riches);
    }
}
