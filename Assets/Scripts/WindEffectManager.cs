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


        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
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
