using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingJump : MonoBehaviour
{
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //rb = GetComponent<Rigibody>();    //gameObjectは省略可能

        //重力係数の調整
        Physics.gravity = new Vector3(0, -7, 0);
    }
    
    private Vector3 clickPosition;
    [SerializeField] 
    private float jumpPower = 10;

    private bool isCanJump;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
        }

        if (isCanJump && Input.GetMouseButtonUp(0))
        {
            //クリックした座標と話した座標の差分を取得
            Vector3 dist = clickPosition - Input.mousePosition;
            //クリックとリリースが同じ座標ならば無視
            if (dist.sqrMagnitude == 0) { return; }
            //差分を標準化し、jumpPowerを掛け合わせた値を移動量とする
            rb.velocity = dist.normalized * jumpPower;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("衝突した");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("接触中");
        //衝突している点の情報が複数格納されている
        ContactPoint[] contacts = collision.contacts;
        //0番目の衝突情報から、衝突している点の邦船を取得
        Vector3 otherNormal = contacts[0].normal;
        //上方向を示すベクトル。長さは１
        Vector3 upVector = new Vector3(0, 1, 0);
        //上方向と法線の内積。二つのベクトルは長さが１なので、cosΘの結果がdotUN変数に入る
        float dotUN = Vector3.Dot(upVector, otherNormal);
        //内積値に逆三角形関数arccosをかけて角度を算出。それを度数法へ変換する。これで角度が算出できた。
        float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;
        //２つのベクトルがなす角度が４５度より小さければ再びジャンプ可能とする
        if (dotDeg <= 45)
        {
            isCanJump = true;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("離脱した");
        isCanJump = false;
    }
}
