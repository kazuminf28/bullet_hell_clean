using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Checker : MonoBehaviour
{
    [SerializeField] float margin = 0.05f;  // カメラ外の余白（高速スクロール対策）

    private Camera cam; 

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    bool IsOutOfScreen()
    {
        // ワールド座標 → 画面座標へ変換
        Vector3 screenPos = cam.WorldToViewportPoint(transform.position);

        // 0〜1 が画面内
        // margin だけはみ出しても生かす
        if (screenPos.x < -margin || screenPos.x > 1 + margin ||
            screenPos.y < -margin || screenPos.y > 1 + margin)
        {
            return true;
        }

        return false;
    }
    #region //動画のやつ
    // enum Mode
    // {
    //     None,
    //     Render,
    //     RenderOut
    // }
    // private Mode mode;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     mode = Mode.None;
    // }

    // private void OnWillRenderObject()
    // {
    //     if (Camera.current.name == "Main Camera")
    //     {
    //         mode = Mode.Render;
    //     }
    // }

    // private void Dead()
    // {
    //     if (mode == Mode.RenderOut)
    //     {
    //         Destroy(gameObject);
    //     } else if (mode == Mode.Render)
    //     {
    //         mode = Mode.RenderOut;
    //     }
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    #endregion
}
