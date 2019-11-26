using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    enum LogicBot
    {
        SimpleBot,
        MiddleBot,
        HardBot
    }

    [Header("Bot params")]
    [SerializeField] float healthBot = 1f;
    [SerializeField] float speedBot = 1f;
    [SerializeField] LogicBot logicBot = LogicBot.SimpleBot;

    [Header("Bomb params")]
    [SerializeField] GameObject prefabBomb;
    [SerializeField] public int powerExplosion = 1; // How many block destroy for one explosion
    [SerializeField] public float timeExplosion = 2f;
    [SerializeField] public int radiusExplosion = 2;

    MonoBehaviour logic;
    void Start()
    {
        switch (logicBot)
        {
            case LogicBot.SimpleBot:
                logic = gameObject.AddComponent<BotSimple>();

                break;
            case LogicBot.MiddleBot:

                break;
            case LogicBot.HardBot:

                break;

        }
        var bomb = prefabBomb.GetComponent<Bomb>();
        bomb.powerExplosion = powerExplosion;
        bomb.radiusExplosion = radiusExplosion;
        bomb.timeExplosion = timeExplosion;
    }

    // Update is called once per frame
    void Update()
    {
        switch (logicBot)
        {
            case LogicBot.SimpleBot:
                ((BotSimple)logic).SetParams(healthBot, speedBot, prefabBomb, radiusExplosion, powerExplosion, timeExplosion);

                break;
            case LogicBot.MiddleBot:

                break;
            case LogicBot.HardBot:

                break;

        }

    }
}
