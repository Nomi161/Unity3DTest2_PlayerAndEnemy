using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;  // ターゲットとなるプレイヤーの情報

    public float followSpeed = 5.0f;    // プレイヤーに追いつくスピード

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤー情報を取得
        player = GameObject.FindGameObjectWithTag("Player");

        // スタートした瞬間のカメラの現在地
        // z軸はプレイヤーの位置から-10
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -10
            );

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (player == null) return; // ゲームオーバーの時のエラー回避

        // 目指すべきポイント
        Vector3 nextPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        // 現在ポイント
        Vector3 nowPos = transform.position;

        // 現在地点→目指すべき地点までの補完
        transform.position = Vector3.Lerp(nowPos, nextPos, followSpeed * Time.deltaTime);
    }

}
