using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Referee_OnInteraction(object sender, RefereeEventArgs e)
    {
        switch(e.type)
        {
            case RefereeEventType.Dodge:

                break;
            case RefereeEventType.ShovePush:
            case RefereeEventType.ShoveContact:

                break;
            case RefereeEventType.StartBlock:

                break;
            case RefereeEventType.StartShove:

                break;
            case RefereeEventType.StartPush:

                break;
            case RefereeEventType.ShoveBlock:

                break;
        }
    }
}
