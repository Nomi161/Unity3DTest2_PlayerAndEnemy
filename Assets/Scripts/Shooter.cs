using UnityEngine;

/// <summary>
/// 何かを発射するスクリプト
/// </summary>
public class Shooter : MonoBehaviour
{
    PlayerController playerCnt;             // プレイヤーコントローラインスタンス

    public GameObject waterWeapon;          // Instatiate生成する対象オブジェクト
    public float shootSpeed;                // ウオーターウエポンの速度
    public float shootDelay;                // 発射間隔
    bool inAttack; //攻撃中ならtrue

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーコントローラインスタンスの取得
        playerCnt = GetComponent<PlayerController>(); //コンポーネント取得     
    }

    // Update is called once per frame
    void Update()
    {
        // スペースキーをおしたら武器を発射する
        if (Input.GetButtonDown("Jump")) Shoot();
    }

    private void FixedUpdate()
    {
    }

    /// <summary>
    /// ウオーターウエポンを発射する処理
    /// </summary>
    public void Shoot()
    {
        Debug.Log("Shoot");
        //if (inAttack || (GameManager.bill <= 0)) return;

        // 発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        // 発射口の位置にウオーターウエポンを生成する
        GameObject weapon = Instantiate(waterWeapon, gate.transform.position, Quaternion.identity);

        // プレイヤーの向いている方向にウエポンを発射する
        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * shootSpeed; // プレイヤーの正面に発射
//!!!        rb.AddForce(transform.forward * shootSpeed);

        //SoundManager.instance.SEPlay(SEType.Shoot); //お札を投げる音

        //GameManager.bill--; //お札の数を減らす
        //inAttack = true; //攻撃中

        ////プレイヤーの角度を入手
        //float angleZ = playerCnt.angleZ;
        //Debug.Log($"PlayerAngleZ={angleZ}");

        ////Rotationが扱っているQuaternion型として準備
        //// オイラー角からQuaternionを作成する
        //Quaternion q = Quaternion.Euler(0, 0, angleZ);

        ////生成 ※ウオーターウエポン、プレイヤーの位置、プレイヤーと同じ角度
        //GameObject obj = Instantiate(waterWeapon, transform.position, q);

        ////生成したウオーターウエポンオブジェクトのRigidbodyの情報を取得
        //Rigidbody rbody = obj.GetComponent<Rigidbody>();

        ////生成したウオーターウエポンオブジェクトが向くべき方角を入手
        //float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); // 角度に対する底辺 X軸の方向
        //float z = Mathf.Sin(angleZ * Mathf.Deg2Rad); // 角度に対する高さ Y軸の方向
        //float y = Mathf.Tan(angleZ * Mathf.Deg2Rad); // 角度に対する奥行 Z軸の方向

        ////角度を分解したxとyをもとに方向データとして整理
        //Vector3 v = Vector3.zero;
        //if (angleZ < 0)
        //{
        //    v = (new Vector3(x, 0.0f, z)).normalized * shootSpeed * -1;
        //}
        //else
        //{
        //    v = (new Vector3(x, 0.0f, z)).normalized * shootSpeed;
        //}

        ////AddForceで指定した方角に飛ばす
        //rbody.AddForce(v, ForceMode.Impulse);

        //時間差で攻撃中フラグを解除
        Invoke("StopAttack", shootDelay);
    }

    void StopAttack()
    {
        inAttack = false; //攻撃中フラグをOFFにする
    }
}
