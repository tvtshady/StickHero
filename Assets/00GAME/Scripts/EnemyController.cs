using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float posY;
    public enum ENEMY_STATE
    {
        FLY,
        DIE
    }
    Rigidbody2D rig;
    [SerializeField]
    ENEMY_STATE _state;

    public ENEMY_STATE state
    {
        set { _state = value; }
        get { return _state; }
    }

    public void init(ENEMY_STATE state, float gravityScale)
    {
        this.state = state;
        rig.gravityScale = gravityScale;
    }

    public void RandomPosition(Vector2 minMax)
    {
        this.transform.position = new Vector3(Random.Range(minMax.x, minMax.y), posY,transform.position.z);
    }

    // Start is called before the first frame update
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        state = ENEMY_STATE.FLY;
    }

    public void BackToDefault()
    {
        init(ENEMY_STATE.FLY, 0f);
        rig.velocity = Vector3.zero;
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("MainCamera"))
        {
            EnemyPooling.instance.ReturnToPool(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(CONSTANT.weapon))
            if(GameManager.instance.currentPlayerState!=GameManager.PLAYER_STATE.DIE&&state!=ENEMY_STATE.DIE)
            {
                AudioManager.instance.PlaySound(AudioManager.AUDIO_TYPE.HIT);
                init(ENEMY_STATE.DIE, 1f);
                GameManager.instance.score++;
            }


        if(collision.CompareTag(CONSTANT.player))
        {
            if(state!=ENEMY_STATE.DIE)
            {
                AudioManager.instance.PlaySound(AudioManager.AUDIO_TYPE.DIEENEMY);
                StartCoroutine( GameManager.instance.GameOver(1f));
            }
        }    
    }
}
