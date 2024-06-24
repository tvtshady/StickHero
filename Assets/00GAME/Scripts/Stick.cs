using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private bool _isPerfect,_isGround;

    public bool isPerfect => _isPerfect;
    public bool isGround => _isGround;  

   

    public void SetRayAll(Vector3 root)
    {
        var results = Physics2D.RaycastAll(root, Vector2.down);
        var resultGround = Physics2D.Raycast(root, Vector2.down);
        var resultPerfect = Physics2D.Raycast(root, Vector2.down);

        foreach (var temp in results)
        {
            if (temp.collider.CompareTag(CONSTANT.pefectTile))
            {
                resultPerfect = temp;
            }

            if (temp.collider.CompareTag(CONSTANT.platform))
            {
                resultGround = temp;

            }
        }
        if (!resultGround || !resultGround.collider.CompareTag(CONSTANT.platform))
        {
            _isGround = false;
        }
        else
            _isGround = true;
        if (!resultPerfect || !resultPerfect.collider.CompareTag(CONSTANT.pefectTile))
        {
            _isPerfect = false;
        }
        else
            _isPerfect = true;
    }

    public void BackDefault()
    {
        this.transform.localScale = new Vector3(0.04f,0.04f,1);
        this.transform.localRotation = new Quaternion(0,0,0,0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(CONSTANT.returnPooling))   
        {
            Debug.Log("stick return");
            StickPooling.instance.ReturnToPool(this);
        }
    }

}
