using UnityEngine;

public class Shooter : MonoBehaviour
{
    PlayerController playerCnt;             // プレイヤーオブジェクト

    public GameObject waterCannonPrefab;    // Instatiate生成する対象オブジェクト
    public float shootSpeed;                // ウオーターキャノンの速度
    public float shootDelay;                // 発射間隔
    bool inAttack; //攻撃中ならtrue

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーオブジェクトの取得
        playerCnt = GetComponent<PlayerController>(); //コンポーネント取得     
    }

    // Update is called once per frame
    void Update()
    {
        // スペースキーをおしたらお札を投擲
        if (Input.GetButtonDown("Jump")) Shoot();
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        //if (inAttack || (GameManager.bill <= 0)) return;

        //発射口オブジェクトを取得
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        //生成 ※ウオーターキャノン、プレイヤーの位置、プレイヤーと同じ角度
        GameObject wCannon = Instantiate(waterCannonPrefab, gate.transform.position, Quaternion.identity);
        Rigidbody rb = wCannon.GetComponent<Rigidbody>();
        rb.AddForce(tr.forward * shootSpeed);

        ////SoundManager.instance.SEPlay(SEType.Shoot); //お札を投げる音

        //GameManager.bill--; //お札の数を減らす
        //inAttack = true; //攻撃中

        ////プレイヤーの角度を入手
        //float angleZ = playerCnt.angleZ;
        //Debug.Log($"PlayerAngleZ={angleZ}");

        ////Rotationが扱っているQuaternion型として準備
        //// オイラー角からQuaternionを作成する
        //Quaternion q = Quaternion.Euler(0, 0, angleZ);

        ////生成 ※ウオーターキャノン、プレイヤーの位置、プレイヤーと同じ角度
        //GameObject obj = Instantiate(waterCannonPrefab, transform.position, q);

        ////生成したウオーターキャノンオブジェクトのRigidbodyの情報を取得
        //Rigidbody rbody = obj.GetComponent<Rigidbody>();

        ////生成したウオーターオブジェクトが向くべき方角を入手
        //float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); //角度に対する底辺 X軸の方向
        //float z = Mathf.Sin(angleZ * Mathf.Deg2Rad); //角度に対する高さ Y軸の方向
        //float y = Mathf.Tan(angleZ * Mathf.Deg2Rad); //角度に対する奥行 Z軸の方向

        ////角度を分解したxとyをもとに方向データとして整理
        //Vector3 v = (new Vector3(z, 0.0f, x)).normalized * shootSpeed;

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
