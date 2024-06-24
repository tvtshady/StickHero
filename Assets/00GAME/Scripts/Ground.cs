using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField]
    GameObject ground, perfectTile, perfectTime;
    [SerializeField]
    private Vector2 minMaxRange;
    
    
    public float GetLocalScaleX()
    {
        return ground.transform.localScale.x;
    }
    public void SetLocalScale(Vector3 scale)
    {
         ground.transform.localScale=scale;
    }

    public void SetRandomSize(Ground currentGround)
    {
        var newScale = this.ground.transform.localScale;
        var allowedScale = this.transform.position.x - currentGround.transform.position.x
            - currentGround.GetLocalScaleX() * 0.5f - this.GetLocalScaleX()*0.5f;
        newScale.x = Mathf.Max(minMaxRange.x, Random.Range(minMaxRange.x, Mathf.Min(allowedScale, minMaxRange.y)));
        this.SetLocalScale(newScale);
        if (currentGround == null ) { Debug.Log("isNull"); }
    }

    public void SetPerfectTime(bool b)
    {
        perfectTime.gameObject.SetActive(b);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag(CONSTANT.returnPooling))
        {
            GroundPooling.instance.ReturnToPool(this);
            perfectTime.gameObject.SetActive(false);
        }
    }
}
