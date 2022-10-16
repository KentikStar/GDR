using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    [SerializeField]
    int moveSpeed = 1;

    private Queue<Vector2> queueDirection = new Queue<Vector2>();
    Vector2 direction;

    LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManag.IsAlive){
            CheckTouch();
            Move();
        }
    }

    public void ClearQueue(){
        queueDirection.Clear();
        lineRenderer.positionCount = 0;
    }

    private void CheckTouch(){
        if(Input.touchCount > 0)
            if(Input.GetTouch(0).phase == TouchPhase.Began){
                Vector2 touchPos = Input.GetTouch(0).position;
                touchPos = Camera.main.ScreenToWorldPoint(new Vector2(touchPos.x,touchPos.y));

                queueDirection.Enqueue(touchPos);
            }        
    }

    private void Move(){
        if(queueDirection.TryPeek(out direction)){
            transform.position = Vector3.MoveTowards(transform.position, direction, Time.deltaTime * moveSpeed);
            DrawLine();
            
            if(transform.position == (Vector3)direction)
                queueDirection.Dequeue();
        }
    }

    private void DrawLine(){
        Queue<Vector2> queuePoints;
        queuePoints = GetQueue();

        lineRenderer.positionCount = queuePoints.Count + 1;
        

        int number = 0;
        lineRenderer.SetPosition(number++, transform.position);
        while(queuePoints.Count > 0){
            lineRenderer.SetPosition(number++, queuePoints.Dequeue());
        }
    }

    private Queue<Vector2> GetQueue(){
        Vector2 [] arr = new Vector2[queueDirection.Count];
        queueDirection.CopyTo(arr,0);

        Queue<Vector2> queueArr = new Queue<Vector2>();
        foreach(Vector2 element in arr){
            queueArr.Enqueue(element);
        }
        
        return queueArr;
    }
}
