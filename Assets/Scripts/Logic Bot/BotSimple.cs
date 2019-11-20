﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicBot))]

public class BotSimple : MonoBehaviour
{
    // List consist from waypoint  
    List<Vector3> wayBot;
    Dictionary<Vector3, Vector3> parentPoint;
    BasicBot basicBot;
    Vector3 positionBomb;
    bool bombExplosion = false;

    void Start()
    {
        basicBot = gameObject.GetComponent<BasicBot>();
        wayBot = new List<Vector3>();
    }

    public void Logic()
    {
        if (basicBot.StateBot == StateBot.Die)
        {
            basicBot.BotDie();
            return;
        }

        if (basicBot.StateBot == StateBot.Stop)
        {
            // Choice point for plant boomb 
            wayBot.Clear();
            var pointPlant = ChoicePlantBomb();
            wayBot = FindWayToPoint(pointPlant);
            basicBot.StateBot = StateBot.Moving;
        }

        if (basicBot.StateBot == StateBot.Moving)
        {
            // Move to plant bomb
            if (wayBot.Count == 0)
                basicBot.StateBot = StateBot.PlantBomb;
            else
                basicBot.botMoveToWay(wayBot);
        }

        if (basicBot.StateBot == StateBot.PlantBomb)
        {
            // Plant bomb
            basicBot.PlantBomb();
            positionBomb = transform.position;
            basicBot.StateBot = StateBot.ChoiceWayFromBomb;
        }

        if (basicBot.StateBot == StateBot.ChoiceWayFromBomb)
        {
            // Choice way from plant bomb
            wayBot.Clear();
            ChoiceWayFromBomb();
            basicBot.StateBot = StateBot.MoveFromBomb;
        }

        if (basicBot.StateBot == StateBot.MoveFromBomb)
        {
            if (wayBot.Count == 0)
                basicBot.StateBot = StateBot.Pause;
            else
                basicBot.botMoveToWay(wayBot);
        }

        if (basicBot.StateBot == StateBot.Pause)
        {
            if (BombExplosion())
            {
                basicBot.StateBot = StateBot.Stop;
            }
        }
    }

    Vector3 ChoicePlantBomb()
    {
        var listPossiblePoint = GetListPossiblePoint(true);
        if (listPossiblePoint.Count == 0) GetListPossiblePoint(false);
        return listPossiblePoint[Random.Range(0, listPossiblePoint.Count)];
    }

    // Находим все возможные точки куда может попасть бот
    // Булевская переменная указывает ищем ли мы точки рядом с каробками
    List<Vector3> GetListPossiblePoint(bool nearBox)
    {
        var possiblePoint = new List<Vector3>();
        var enterPointlist = new List<Vector3>();
        var stack = new Stack<Vector3>();
        var startPoint = gameObject.transform.position;
        var currentPoint = startPoint;
        parentPoint = new Dictionary<Vector3, Vector3>();
        stack.Push(currentPoint);

        // DFS
        do
        {
            currentPoint = stack.Pop();
            enterPointlist.Add(currentPoint);

            if (nearBox)
            {
                if (NearBox(currentPoint) && !possiblePoint.Contains(currentPoint) && currentPoint != startPoint)
                {
                    possiblePoint.Add(currentPoint);
                }
            }
            else
            {
                if (currentPoint != startPoint)
                    possiblePoint.Add(currentPoint);
            }
            NextStep(stack, enterPointlist, currentPoint);

        } while (stack.Count != 0);

        return possiblePoint;
    }

    // Проверяем взорвана ли бомба
    bool BombExplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(positionBomb, 0.01f);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Bomb")
                return false;
        }
        return true;
    }

    // Функция определяет есть ли рядом коробка, которую можно разрушить. 
    // Возвращает true если есть 
    bool NearBox(Vector3 currentPoint)
    {
        List<Collider[]> arrayColliders = new List<Collider[]> {
            Physics.OverlapSphere (currentPoint + new Vector3 (1, 0, 0), 0.01f),
            Physics.OverlapSphere (currentPoint + new Vector3 (-1, 0, 0), 0.01f),
            Physics.OverlapSphere (currentPoint + new Vector3 (0, 0, 1), 0.01f),
            Physics.OverlapSphere (currentPoint + new Vector3 (0, 0, -1), 0.01f)
        };

        foreach (Collider[] colliders in arrayColliders)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Box")
                    return true;
            }
        }
        return false;
    }

    // Ищет следующие точки куда может пойти бот из текущей точки
    void NextStep(Stack<Vector3> stack, List<Vector3> enterPointlist, Vector3 currentPoint)
    {
        Collider[] colliders;
        Vector3 point;
        var vectors = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, 1) };

        foreach (var vector in vectors)
        {
            foreach (var i in new int[] { 1, -1 })
            {
                point = currentPoint + vector * i;
                colliders = Physics.OverlapSphere(point, 0.01f);
                if (colliders.Length == 0 && !enterPointlist.Contains(point) && !stack.Contains(point))
                {
                    stack.Push(point);
                    // запоминаем родителя точки
                    parentPoint.Add(point, currentPoint);
                }
            }
        }
    }

    List<Vector3> FindWayToPoint(Vector3 pointPlant)
    {
        List<Vector3> way = new List<Vector3>();
        var point = pointPlant;
        do
        {
            way.Add(point);
            point = parentPoint[point];
        } while (point != transform.position);
        way.Reverse();
        return way;
    }

    // Сущесвует возможность, что бот подорвется на своей бомбе 
    void ChoiceWayFromBomb()
    {
        var listPossiblePoint = GetListPossiblePoint(false);
        Vector3 point;

        do
        {
            wayBot.Clear();
            point = listPossiblePoint[Random.Range(0, listPossiblePoint.Count)];
            wayBot = FindWayToPoint(point);
        } while (!SafePoint(point));
    }

    bool SafePoint(Vector3 point)
    {
        List<Vector3> arrayColliders = new List<Vector3> {
            positionBomb + new Vector3 (1, 0, 0),
            positionBomb + new Vector3 (-1, 0, 0),
            positionBomb + new Vector3 (0, 0, 1),
            positionBomb + new Vector3 (0, 0, -1)
        };

        point.y = positionBomb.y;

        if (!arrayColliders.Contains(point))
            return true;
        else
            return false;
    }
}