using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG;
using DG.Tweening;
public enum WindType{sway,noisy}

public class WindEffectMovmentBehaviour : MonoBehaviour
{

    Material mat;
    //x2+y2=r2
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

       
        mat = new Material(transform.GetChild(0).GetComponent<TrailRenderer>().material);
        transform.GetChild(0).GetComponent<TrailRenderer>().material = mat;
        
    }


     public void InitaiteMovingLinear(float dist)
    {
        Debug.Log("InitaiteMovingLinear");
        transform.DOMoveX(transform.position.x -dist, 2).SetEase(Ease.Linear).OnComplete(() => {

                            mat.DOFade(0, 2).OnComplete(()=>Destroy(gameObject));
                           });;
    }
     public  void InitaiteMovingSway(float radius)
    {
             Debug.Log("InitaiteMovingSway");
        transform.GetChild(0).transform.localPosition = new Vector3(0, -radius, 0);
        transform.DOMoveX(transform.position.x -5, 1).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.DOLocalRotate(new Vector3(0, 0, -360), 1.5f,RotateMode.WorldAxisAdd).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                    transform.DOMoveX(transform.position.x -20, 2).SetEase(Ease.Linear)
                        .OnComplete(() => {

                            mat.DOFade(0, 2).OnComplete(()=>Destroy(gameObject));
                           });
                    });
            });
        
        }

}