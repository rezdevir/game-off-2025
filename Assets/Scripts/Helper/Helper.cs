using System;
using System.Collections.Generic;

using UnityEngine;

static public class Helper
{
    static public Vector2 EndPoint(Vector2 startingPoint, Vector2 direction, float length)
    {
        var vector = direction * length;
        return EndPoint(startingPoint, vector);

    }
    static public Vector2 EndPoint(Vector2 startingPoint, Vector2 vector)
    {
        return new Vector2(startingPoint.x + vector.x, startingPoint.y + vector.y);
    }
    static public Vector2 EndPoint(Vector2 startingPoint, float angle, float length)
    {
        return EndPoint(startingPoint, Angle2Dir(angle, length), length);
    }
    static public Vector2 Direction(Vector2 pointA, Vector2 pointB)
    {
        return pointB - pointA;
    }
    static public float Magnitude(Vector2 vector)
    {
        return Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y));
    }

    static public Vector2 TangentCurve(Vector2 startingPoint, Vector2 TangentPoint, float x_or_y, bool IsX)
    {
        float m = GradientSlope(startingPoint, TangentPoint);
        //y-b=m(x-a)
        if (IsX)
        {
            //((y-b)+ma)/m=x
            float x = ((x_or_y - TangentPoint.y) + (m * TangentPoint.x)) / m;
            return new Vector2(x, x_or_y);
        }
        else
        {
            //m(x-a)+b
            float y = m * (x_or_y - TangentPoint.x) + TangentPoint.y;
            return new Vector2(x_or_y, y);
        }

    }
    static public float GradientSlope(Vector2 pointA, Vector2 pointB)
    {
        return (pointB.y - pointA.x) / (pointB.x - pointA.y);
    }
    static public float Length(Vector2 vector1, Vector2 vector2)
    {
        var dir = Direction(vector1, vector2);
        return Magnitude(dir);
    }
    static public Vector2 Unit(Vector2 vector)
    {
        var unit_dir = vector / Magnitude(vector);
        return unit_dir;
    }
    static public Vector2 Angle2Dir(float angle, float r)
    {
        float x = Mathf.Cos(angle) * r;
        float y = Mathf.Sin(angle) * r;
        return new Vector2(x, y);
    }
    static public float Dir2Angle(Vector2 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    static public Action FunctionByChance(List<Action> functions)
    {


        return functions[UnityEngine.Random.Range(0, functions.Count )];


    }




    /// <summary>
    /// Return Between 0 ~ 1
    /// </summary>
    /// <param name="Full Cap "></param>
    /// <param name="Now in "></param>
    /// <returns></returns>
    static public float percent(float all, float current)
    {
        return (current / all);
    }

    static public string GetIdString(int length)
    {
        string alphabet =
        "1234567890qwertyuiopasdfghjklasdfghjklzxcvbnm,.QWERTYUIOOOPASDFGHJKLZXCVBNM";
        string ID = "";
        for (int i = 0; i < length; i++)
        {
            ID = ID + alphabet[UnityEngine.Random.Range(0, alphabet.Length - 1)];
        }
        return ID;
    
    }

    // static public float GetId()
    // {

    // }

}
