using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum StateBot
{
    Moving,
    Stop,
    PlantBomb,
    MoveFromBomb,
    ChoiceWayFromBomb,
    Die,
    Pause
};

[RequireComponent(typeof(CharacterController))]

public class BasicBot : MonoBehaviour
{
    enum StateMove
    {
        moveToPoint,
        nextPoint
    };
    CharacterController characterController;
    [SerializeField] float healthBot = 1f;
    [SerializeField] float speedBot = 1.5f;
    [SerializeField] GameObject prefabBomb;
    StateBot stateBot = StateBot.Stop;
    
    public StateBot StateBot{
        get{
            return stateBot;
        }
        set{
            stateBot = value;
        }
    }

    StateMove stateMove = StateMove.moveToPoint;
    
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    public void botMoveToWay(List<Vector3> way)
    {
        if (stateMove == StateMove.moveToPoint)
        {
            botMoveToPoint(way[0]);
        }

        if (stateMove == StateMove.nextPoint)
        {
            way.RemoveAt(0);
            stateMove = StateMove.moveToPoint;
        }
    }

    public void BotDie()
    {
        StateBot = StateBot.Die;
        Destroy(gameObject);
    }

    public void PlantBomb()
    {
        Vector3 coordinateBobmb = AroundCoordinateBomb();
        GameObject bomb = Instantiate(prefabBomb, coordinateBobmb, Quaternion.identity);
    }

    void botMoveToPoint(Vector3 point)
    {
        var vectorMove = GetVectorMove(point, transform.position);

        if (ArounPoint(transform.position, point))
        {
            point.y = 0;
            transform.position = point + new Vector3(0, transform.position.y, 0);

            stateMove = StateMove.nextPoint;
        }
        else
        {
            characterController.Move(vectorMove * speedBot * Time.deltaTime);
        }
    }

    // Метод вычисляет еденичный вектор движения бота
    Vector3 GetVectorMove(Vector3 point, Vector3 objectPosition)
    {
        var vector = point - objectPosition;

        vector.x = vector.x != 0 ? vector.x / Mathf.Abs(vector.x) : 0;
        vector.z = vector.z != 0 ? vector.z / Mathf.Abs(vector.z) : 0;

        return vector;
    }

    // Определяем достигли ли мы нужной точки с определенной погрешностью
    bool ArounPoint(Vector3 objectPosition, Vector3 point)
    {
        var distanceCorrection = 0.1f;
        var distance = objectPosition - point;
        if (Mathf.Abs(distance.x) < distanceCorrection && Mathf.Abs(distance.z) < distanceCorrection)
            return true;
        else
            return false;
    }

    Vector3 AroundCoordinateBomb()
    {
        Vector3 coordinateBobmb = new Vector3(0, 0, 0);

        coordinateBobmb.x = Mathf.Round(transform.position.x);
        coordinateBobmb.y = transform.position.y;
        coordinateBobmb.z = Mathf.Round(transform.position.z);

        return coordinateBobmb;
    }
    
}
