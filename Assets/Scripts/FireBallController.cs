using UnityEngine;

public class FireBallController : MonoBehaviour
{
     public float deleteTime = 3.0f;    //削除する時間指定

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, deleteTime);    //削除設定
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);   //何かに接触したら消す
    }
}
