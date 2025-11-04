using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DG;
using DG.Tweening;
public enum WindType{sway,noisy}

public class WindEffectMovmentBehaviour : MonoBehaviour
{
    [SerializeField] private WindType windtype ;
    [SerializeField] private float WindSpeed = 1f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float smoothness = 1;
    Material mat;
    //x2+y2=r2
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        transform.GetChild(0).transform.localPosition = new Vector3(0, -radius, 0);
        mat = new Material(transform.GetChild(0).GetComponent<TrailRenderer>().material);
        transform.GetChild(0).GetComponent<TrailRenderer>().material = mat;
        
    }
    void Start()
    {
        if (windtype == WindType.sway)  StartMovingSway(); else StartMovingNoisy();
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     // transform.eulerAngles += new Vector3(0, 0, smoothness)*Time.fixedDeltaTime*WindSpeed;
    // }
    void StartMovingNoisy()

    {
        
    }
        void StartMovingSway()
        {
        {
        transform.DOLocalMoveX(-5, 1).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.DOLocalRotate(new Vector3(0, 0, -360), 1.5f,RotateMode.WorldAxisAdd).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                    transform.DOLocalMoveX(-20, 1)
                        .OnComplete(() => {

                            mat.DOFade(0, 2).OnComplete(()=>Destroy(gameObject));
                           });
                    });
            });
    }
        }

}