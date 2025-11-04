using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffectManager : MonoBehaviour
{
    [SerializeField] GameObject WindPrefab;
    [SerializeField] float WindRate = 2f;

    [SerializeField] Vector2 Y_Range;
    [SerializeField] Vector2 X_Range;
    // [SerializeField][Range(0.1f, 1)] float y_multiplier = 0.5f;
    // [SerializeField][Range(0.7f, 1.5f)] float x_multiplier = 1.1f;
    // List<Vector2> ScreenBorder = new List<Vector2>();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(1 - x_multiplier, 0)));
        // ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(1 - x_multiplier, y_multiplier)));
        // ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(x_multiplier, y_multiplier)));
        // ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(x_multiplier, 0)));
        StartCoroutine(Intiator());
    }

    IEnumerator Intiator()
    {
        yield return new WaitForSeconds(WindRate);
        GenerateWind();
        StartCoroutine(Intiator());

    }
    void GenerateWind()
    {
        float x = UnityEngine.Random.Range(X_Range.x, X_Range.y);
        float y = UnityEngine.Random.Range(Y_Range.x, Y_Range.y);
        Vector2 pos = new Vector2(x, y);
        GameObject obj = Instantiate(WindPrefab, pos, transform.rotation, transform);
        List<Action> fun = new List<Action>();
        fun.Add(() => obj.GetComponent<WindEffectMovmentBehaviour>().InitaiteMovingLinear(  UnityEngine.Random.Range(5f, 20f)));
        fun.Add(() => obj.GetComponent<WindEffectMovmentBehaviour>().InitaiteMovingLinear(  UnityEngine.Random.Range(5f, 20f)));
        fun.Add(() => obj.GetComponent<WindEffectMovmentBehaviour>().InitaiteMovingSway(
            UnityEngine.Random.Range(0.5f, 2.5f)

        ));
        Helper.FunctionByChance(fun).Invoke();

    }
    

}
