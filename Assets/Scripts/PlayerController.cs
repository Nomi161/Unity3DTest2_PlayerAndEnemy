using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの基礎ステータス")]
    public float playerSpeed = 3.0f;    // プレイヤーの移動スピード
    public float gravity;
    public float speedZ;
    public float speedJump;

    float axisH;    // 横方向の入力状況（左右）
    float axisV;    // 縦方向の入力状況（左右）


    [Header("プレイヤーの角度計算用")]
    public float angleZ = -90f; //プレイヤーの角度計算用

    //コンポーネント
    Rigidbody rbody;                    // 自身のRigidbody
    CharacterController controller;     // 自身のCharacterControllerコントローラ
    Animator anime;                     // 自身のアニメコントローラ

    GameObject enemy;                   //敵情報


    Vector3 moveDirection = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //コンポーネントの取得
        rbody = GetComponent<Rigidbody>();                  // 自身のRigidbody
        controller = GetComponent<CharacterController>();   // 自身のCharacterControllerコントローラ
        anime = GetComponent<Animator>();                   // 自身のアニメコントローラ
        enemy = GameObject.FindGameObjectWithTag("Enemy");  // 敵情報を得る
        //StartCoroutine(LookAtEnu());
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 currentPosition = transform.position;       // 自身の位置情報を得る
        //Debug.Log($"Before x={currentPosition.x} y={currentPosition.y} z={currentPosition.z}");

        //// プレイ中でなければ何もしない
        //if (!(GameManager.gameState == GameState.playing ||
        //    GameManager.gameState == GameState.ending)) return;

        GetMoveInfo();                 // 前後左右の入力値の取得
        angleZ = GetAngle();    // その時の角度を変数angleZに反映
        MoveByKey();
            //        Animation();            // angleZを利用してアニメーション
    }

    public GameObject waterCannonPrefab;    // Instatiate生成する対象オブジェクト
    int shootSpeed = 10;
 
    private void FixedUpdate()
    {

        //// プレイ中でなければ何もしない
        //if (!(GameManager.gameState == GameState.playing ||
        //    GameManager.gameState == GameState.ending)) return;

        //// ダメージフラグが立っている間
        //if (inDamage)
        //{
        //// 点滅演出
        //// Sinメソッドの角度情報にゲーム開始からの経過時間を与える
        //float val = Mathf.Sin(Time.time * 50);

        //if (val > 0)
        //{
        //    // 描画機能を有効
        //    gameObject.GetComponent<SpriteRenderer>().enabled = true;
        //}
        //else
        //{
        //    // 描画機能を無効
        //    gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //}

        //// 入力によるvelocityが入らないようここでリターン
        //return;
        //}
        // プレイヤーの向きを敵に向ける
//!!!        LookAt();

        // 入力状況に応じてPlayerを動かす
        if (axisH != 0 || axisV != 0)
        {
          rbody.linearVelocity = (new Vector3(axisH, 0.0f, axisV)).normalized * playerSpeed;
        }
        //Vector3 newPosition = transform.position;       // 自身の位置情報を得る
        //Debug.Log($"after x={newPosition.x} y={newPosition.y} z={newPosition.z}");

    }

    /// <summary>
    /// キー入力による左右回転と前後左右への移動 
    /// </summary>
    private void MoveByKey()
    {
        // (右回転)
        if ( Input.GetKey(KeyCode.Q) )
        {
            Debug.Log("右回転");

//            transform.Rotate(new Vector3(0, -90,0) * Time.deltaTime);
            transform.Rotate(new Vector3(0, -45, 0) * Time.deltaTime);
        }
        // (左回転)
        else if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("左回転");
//            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
            transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);

        }
        // Wキー(前方移動)
        else if (Input.GetKey(KeyCode.W))
        {
            transform.position += playerSpeed * transform.forward * Time.deltaTime;
        }
        // Sキー(後方移動)
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= playerSpeed * transform.forward * Time.deltaTime;
        }
        // Dキー(右移動)
        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("右移動");
            transform.position += playerSpeed * transform.right * Time.deltaTime;
        }
        // Aキー(左移動)
        else if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("左移動");
            transform.position -= playerSpeed * transform.right * Time.deltaTime;
        }

    }

    /// <summary>
    /// 前後左右の入力値の取得 
    /// </summary>
    private void GetMoveInfo()
    {
        //if (!isViartual)    // ヴァーチャルパッドを触っていないのであれば
        //{
            // axisHとaxisVに入力状況を代入する
            // float GetAxisRow(string axisName)
            // 取得できる値は常に-1,0,1のいずれかです。
            // ・入力が右方向の場合、1が返されます。
            // ・入力が左方向の場合、-1が返されます。
            // ・入力がない、または中央に有る場合、0がかえされます。
            axisH = Input.GetAxisRaw("Horizontal"); // 水平方向の入力を取得（左右）
            axisV = Input.GetAxisRaw("Vertical");   // 垂直方向の入力を取得（前後）
        //}


    }

    //その時のプレイヤーの角度を取得
    public float GetAngle()
    {
        float angle; //returnされる値の準備
        angle = angleZ;
 
        //現在座標の取得
        Vector3 fromPos = transform.position;

        //その瞬間のキー入力値(axisH、axisV)に応じた予測座標の取得
//        Vector3 toPos = new Vector3(fromPos.x + axisH, fromPos.y + axisV, fromPos.z);
        Vector3 toPos = new Vector3(fromPos.x + axisH, 0.0f, fromPos.z + axisV);

//        Debug.Log($"axisH={axisH} axisV={axisV}");

        //もしも何かしらの入力があれば あらたに角度算出
        if (axisH != 0 || axisV != 0)
        {
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;
            float dirZ = toPos.z - fromPos.z;

            //第一引数に高さY、第二引数に底辺Xを与えると角度をラジアン形式で算出（円周の長さで表現）
 //           float rad = Mathf.Atan2(dirY, dirX);
            float rad = Mathf.Atan2(dirZ, dirX);

            //ラジアン値をオイラー値(デグリー）に変換
            angle = rad * Mathf.Rad2Deg;
        }
        //何も入力されていなければ 前フレームの角度情報を据え置き
        else
        {
            angle = angleZ;
        }
        return angle;
    }

    /// <summary>
    /// アニメーション処理
    /// </summary>
    void Animation()
    {
        return;

        //なんらかの入力がある場合
        if (axisH != 0 || axisV != 0)
        {

            //ひとまずRunアニメを走らせる
            anime.SetBool("run", true);

            //angleZを利用して方角を決める　パラメータdirection int型
            //int型のdirection 下：0　上：1　右：2　左：それ以外

            if (angleZ > -135f && angleZ < -45f) //下方向
            {
                anime.SetInteger("direction", 0);
            }
            else if (angleZ >= -45f && angleZ <= 45f) //右方向
            {
                anime.SetInteger("direction", 2);
                transform.localScale = new Vector2(1, 1);
            }
            else if (angleZ > 45f && angleZ < 135f) //上方向
            {
                anime.SetInteger("direction", 1);
            }
            else //左方向
            {
                anime.SetInteger("direction", 3);
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else //何も入力がない場合
        {
            anime.SetBool("run", false); //走るフラグをOFF
        }
    }

}
