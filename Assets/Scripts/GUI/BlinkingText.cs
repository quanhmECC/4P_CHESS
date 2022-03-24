using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if((int)_time%2 == 0)
        {
            _text.enabled = false;
        }
        else
        {
            _text.enabled = true;
        }
    }
}
