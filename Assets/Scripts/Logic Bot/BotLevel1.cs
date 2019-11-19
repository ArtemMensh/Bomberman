using UnityEngine;

[RequireComponent (typeof(BotSimple))]

public class BotLevel1 : MonoBehaviour
{

    BotSimple bot;
    // Start is called before the first frame update
    void Start()
    {
        bot = gameObject.GetComponent<BotSimple>();
    }

    // Update is called once per frame
    void Update()
    {
       bot.Logic();
    }
}
