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

    private void Referee_OnInteraction(object sender, InteractionEventArgs e)
    {
        switch(e.type)
        {
            case EventType.Dodge:

                break;
            case EventType.ShovePush:
            case EventType.ShoveContact:

                break;
            case EventType.StartBlock:

                break;
            case EventType.StartShove:

                break;
            case EventType.StartPush:

                break;
            case EventType.ShoveBlock:

                break;
        }
    }
}
