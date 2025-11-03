using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D;

public class Node
{
    public Vector2 Position { get; set; }
    public Vector2 Left_Tangent { get; set; } =  Vector2.zero;
    public Vector2 Right_Tangent { get; set; }=  Vector2.zero;


}

public class OceanManager : MonoBehaviour
{
    [SerializeField] int n_point = 0;
    [SerializeField] float peak = 1;
    [SerializeField] float tilling = 1;
    [SerializeField] float speed = 1;
    [SerializeField] float baseHeight  = 1;
    SpriteShapeController spc;
    List<Vector2> ScreenBorder = new List<Vector2>();
    [SerializeField][Range(0.1f, 1)] float y_multiplier = 0.5f;
    [SerializeField][Range(0.7f, 1.5f)] float x_multiplier = 1.1f;

    [SerializeField] float rate = 0.1f;
    // [SerializeField] Vector2 test 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spc = GetComponent<SpriteShapeController>();

        ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(1 - x_multiplier, 0)));
        ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(1 - x_multiplier, y_multiplier)));
        ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(x_multiplier, y_multiplier)));
        ScreenBorder.Add(Camera.main.ViewportToWorldPoint(new Vector2(x_multiplier, 0)));


    }

    void UpdateBorder()
    {
        ScreenBorder[0] = Camera.main.ViewportToWorldPoint(new Vector2(1 - x_multiplier, y_multiplier));
        ScreenBorder[1] = Camera.main.ViewportToWorldPoint(new Vector2(x_multiplier, y_multiplier));
        ScreenBorder[2] = Camera.main.ViewportToWorldPoint(new Vector2(x_multiplier, 0));
        ScreenBorder[3] = Camera.main.ViewportToWorldPoint(new Vector2(1 - x_multiplier, 0));
    }

    void OceanHandler()
    {
        var spline = spc.spline;
        spline.Clear();
        // spline.
        for (int i = 0; i < n_point + 4; i++)
        {
            // spline.
            Node node = NodeDispatcher(spline, i, n_point);
            spline.InsertPointAt(i, node.Position);
            if(node.Left_Tangent.x!=0)
            {
                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spline.SetRightTangent(i,node.Right_Tangent);
                 spline.SetLeftTangent(i,node.Left_Tangent);
            }
            
            // Vector3 dir = (nextPoint - prevPoint).normalized;
            // float curveStrength = 2f; // Bigger = smoother
            // i, dir * curveStrength);
            // spline.SetLeftTangent(i, -dir * curveStrength);
        }
        spc.BakeCollider();
    }


    Node NodeDispatcher(Spline spline, int i, int n)
    {   // ScreenBorder 
        // ScreenBorder 0 top left corener node 
        // ScreenBorder 1 top right corener node
        // ScreenBorder 2 down right corener node
        // ScreenBorder 3 down left corener node
        if (0 == i)
        {
            Node node = new Node();
            node.Position = ScreenBorder[0];
            return node;

        }
        else if (n + 1 == i)
        {
                       Node node = new Node();
            node.Position = ScreenBorder[1];
            return node;
        }
        else if (n + 2 == i)
        {
                        Node node = new Node();
            node.Position = ScreenBorder[2];
            return node;
        }
        else if (n + 3 == i)
        {
            Node node = new Node();
            node.Position = ScreenBorder[3];
            return node;
        }
        else
        {
            Node node = new Node();
            node.Position = GenerateWave(spline, i, n);
            

            
//      float tangentLength = 0.5f; // adjust this for curve intensity
// node.Left_Tangent = new Vector2(-tangentLength, 0.5f);
// node.Right_Tangent = new Vector2(tangentLength, 0.5f);
            // node.Right_Tangent=
            return node;

         
        }

    }


    Vector2 GenerateWave(Spline spline, int i,int n)
    {
        // Debug.Log("i:" + i);
        // Debug.Log("ScreenBorder[1].x:" + ScreenBorder[1].x);
        // Debug.Log("ScreenBorder[0].x:" + ScreenBorder[0].x);
        float offset = (ScreenBorder[1].x - ScreenBorder[0].x) / (n + 1);
        // Debug.Log("offset:" + offset);
        float x = spline.GetPosition(i - 1).x + offset;
        // Debug.Log("spline.GetPosition(i - 1).x:"+spline.GetPosition(i - 1).x);
        Debug.Log(" Mathf.PerlinNoise(1,1):" + Mathf.PerlinNoise(i,1));

    // float randomOffset = Mathf.PerlinNoise(i * 0.2f, Time.time *rate) * 2f - 1f;
    // float yy = baseHeight 
    // + Mathf.PerlinNoise(Mathf.Sin(i + tilling), 1)
    // * Mathf.Sin(i + tilling)
    // * (peak + randomOffset * 0.5f)
    // + Mathf.PerlinNoise(i, 1);
        // float yy = baseHeight + MathF.Sin(i+ tilling)* UnityEngine.Random.Range(peak - rate, peak + rate);
        float y = baseHeight +( MathF.Sin(i+tilling)*peak);
        return new Vector2(x, y);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBorder();
        OceanHandler();
        MovingWaves();

    }
    void MovingWaves()
    {
        tilling += speed * Time.deltaTime;
    }
}
