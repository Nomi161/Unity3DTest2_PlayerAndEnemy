using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    // ヒットポイント
    public int hp = 5;

    public GameObject fireBallPrefab;       // 火球のプレハブ
    public float shootSpeed = 5.0f;         // 火球の速度

    public bool onBarrier;                  //バリアにあたっているか
    GameObject player;                      //プレイヤー情報

    public float speed = 0.5f;              // スピード
    float axisH;                            //横軸値(-1.0 ~ 0.0 ~ 1.0)
    float axisV;                            //縦軸値(-1.0 ~ 0.0 ~ 1.0)

    Rigidbody rbody;                        //Rigidbody
    Animator animator;                      //Animator
    int frameCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody>();      // Rigidbodyを得る
        animator = GetComponent<Animator>();    //Animatorを得る
        player = GameObject.FindGameObjectWithTag("Player"); //プレイヤー情報を得る

        //        animator.SetBool("Active", true);

    }

    // Update is called once per frame
    void Update()
    {
        //playingモードでないと何もしない
        //if (GameManager.gameState != GameState.playing) return;

        //バリアに触れている時は何もしない
        if (onBarrier) return;

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;

        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;

        // 移動するベクトルを作る
        axisH = Mathf.Cos(rad) * speed;
        axisV = Mathf.Sin(rad) * speed;

    }

    void FixedUpdate()
    {
        //playingモードでないと何もしない
        //if (GameManager.gameState != GameState.playing) return;

        //プレイヤーがいない時は何もしない
        if (player == null) return;

        //バリアに触れている時は何もしない
        if (onBarrier)
        {
            rbody.linearVelocity = Vector3.zero;

            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                //描画機能を有効
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //描画機能を無効
                GetComponent<SpriteRenderer>().enabled = false;
            }

            return;
        }

        // 移動
        transform.LookAt(player.transform);
        rbody.linearVelocity = new Vector3(axisH, axisV).normalized;

        frameCount++;
        if (frameCount >= 30)
        {
            StartCoroutine(AttackEnum());
            frameCount = 0;
        }
    }

    IEnumerator AttackEnum()
    {
        yield return new WaitForSeconds(1);

        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            if (onBarrier) return;

            hp--;
            if (hp > 0)
            {
                onBarrier = true;
                StartCoroutine(Damaged());
            }
            else
            {
                //if (GameManager.gameState == GameState.playing) StartCoroutine(StartEnding());
            }
        }
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(5);
        onBarrier = false;
        //描画機能を有効
        GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator StartEnding()
    {
        //ゲームエンド
        animator.SetTrigger("Dead");
        rbody.linearVelocity = Vector2.zero;
        //GameManager.gameState = GameState.ending;
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("Ending");
    }

    //攻撃　Attackアニメーションにつく
    void Attack()
    {
        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;
        //弾を発射するベクトルを作る
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y- gate.transform.position.y;
            //アークタンジェント２関数で角度（ラジアン）を求める
            float rad = Mathf.Atan2(dy, dx);
            //ラジアンを度に変換して返す
            float angle = rad * Mathf.Rad2Deg;
            //Prefabから弾のゲームオブジェクトを作る（進行方向に回転）
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(fireBallPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            float z = Mathf.Tan(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
            //発射
            Rigidbody rbody = bullet.GetComponent<Rigidbody>();
            rbody.AddForce(v, ForceMode.Impulse);
        }
    }

//    public float projectileSpeed = 10f;
//    void FireProjectile()
//    {
//        GameObject player = GameObject.FindGameObjectWithTag("Player");

//        // 弾を生成
//        //発射口オブジェクトを取得
//        Transform tr = transform.Find("gate");
//        GameObject gate = tr.gameObject;

//        GameObject ullet = Instantiate(fireBallPrefab, gate.transform.position, Quaternion.identity);

//        // ターゲット方向を計算
//        Vector3 direction = (player.transform.position - gate.transform.position).normalized;

//        // Rigidbody に速度を与える
//        Rigidbody rbody = ullet.GetComponent<Rigidbody>();
//        rbody.AddForce(v, ForceMode.Impulse);
////        rb.velocity = direction * projectileSpeed;
//    }

}
