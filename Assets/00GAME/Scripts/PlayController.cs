using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GAME_STATE = GameManager.GAME_STATE;
using PLAYER_STATE = GameManager.PLAYER_STATE;
using AUDIO_TYPE = AudioManager.AUDIO_TYPE;

public class PlayController : MonoBehaviour 
{
    [SerializeField]
    Stick currentStick;
    [SerializeField]
    Ground currentGround, nextGround;
    [SerializeField]
    Transform rotateTranform, endRotateTransform;
    [SerializeField]
    int index = 0;

    [SerializeField] Camera currentCamera;

    [SerializeField]
    private Vector3 startPos,  stickPos;

    [SerializeField]
    private Vector2 minMaxRange, spawnRange;

    [SerializeField]
    float maxScale = 10, _speed = 2,_speedMove=0.5f;
    float cameraOffsetX;
    [SerializeField]
    Vector3 playerPositionOri,groundPosOri,groundScaleOri;

    float halfHeight, halfWidth;

    Rigidbody2D rig;
    private void Awake()
    {
        stickPos =  StickPositionStart(currentGround, StickPooling.instance.getT());
        cameraOffsetX = currentCamera.transform.position.x - this.transform.position.x;
        halfHeight = currentCamera.orthographicSize;
        halfWidth = currentCamera.aspect * halfHeight;
    }

    private void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GAME_STATE.INPUT)
            if (Input.GetMouseButton(0))
            {
                GameManager.instance.currentPlayerState = PLAYER_STATE.GROW;
                GameManager.instance.currentState = GAME_STATE.GROWING;
                AudioManager.instance.PlaySound(AUDIO_TYPE.GROW);
                ScaleStick();
            }
        if (GameManager.instance.currentState == GAME_STATE.GROWING)
            if (Input.GetMouseButton(0))
            {
                ScaleStick();
                AudioManager.instance.PlaySound(AUDIO_TYPE.GROW);
            }
            else
            {
                GameManager.instance.currentPlayerState = PLAYER_STATE.KICK;
                AudioManager.instance.PlaySound(AUDIO_TYPE.KICK);
                StartCoroutine(FallStick());
            }
        if(GameManager.instance.currentState == GAME_STATE.NONE&& GameManager.instance.currentPlayerState != PLAYER_STATE.DIE)
        if (Input.GetMouseButtonDown(0))
        {
                StartCoroutine(Fighting(0.42f));
        }

    }
        
    
    IEnumerator Fighting(float t)
    {
        GameManager.instance.currentPlayerState = PLAYER_STATE.FIGHT;
        yield return new WaitForSeconds(t);
        if(GameManager.instance.currentPlayerState == PLAYER_STATE.FIGHT)
        GameManager.instance.currentPlayerState = PLAYER_STATE.RUN;
    }

    IEnumerator FallStick()
    {
        GameManager.instance.currentState = GAME_STATE.NONE;
        yield return new WaitForSeconds( 0.2f);
        AudioManager.instance.PlaySound(AUDIO_TYPE.FALL);
        var x = Rotate(currentStick.transform, rotateTranform, 0.4f);
        AudioManager.instance.PlaySound(AUDIO_TYPE.WOODHIT);
        yield return x;
        
            Vector3 root = new Vector3(currentStick.transform.position.x + currentStick.transform.localScale.y, this.transform.position.y, this.transform.position.z);
        currentStick.SetRayAll(root);

        if (currentStick.isPerfect)
        {
            Debug.Log("Perfect");
            GameManager.instance.score += 1;
            nextGround.SetPerfectTime(true);
            AudioManager.instance.PlaySound(AUDIO_TYPE.PERFECT);
        }


        Vector3 target = currentStick.transform.position + new Vector3(currentStick.transform.localScale.y, 0, 0);
        target.y = this.transform.position.y;
        x = Move(this.transform, target, currentStick.transform.localScale.y / cameraOffsetX* _speedMove);
        GameManager.instance.currentPlayerState = PLAYER_STATE.RUN;
        
        yield return x;

       
        if (!currentStick.isGround)
        {
            //GameManager.instance.currentPlayerState = PLAYER_STATE.IDLE;
            //GameManager.instance.currentPlayerState = PLAYER_STATE.DIE;

            //AudioManager.instance.PlaySound(AUDIO_TYPE.DIE);
            StartCoroutine( GameManager.instance.GameOver(1f));
            x = Rotate(currentStick.transform, endRotateTransform, 0.5f);
            yield return x;
            Debug.Log("GameOver");
            //GameOver();
            //GameManager.instance.currentState = GAME_STATE.GAMEOVER;
        }
        else
        if(GameManager.instance.currentPlayerState!= PLAYER_STATE.DIE) 
        {
            GameManager.instance.score++;
            AudioManager.instance.PlaySound(AUDIO_TYPE.SCORE);
            //UpdateScore();

            Debug.Log("Score");
            target = this.transform.position;
            target.x = nextGround.transform.position.x + nextGround.GetLocalScaleX() * 0.5f - 0.35f;
            x = Move(this.transform, target, GetTimeMove(this.transform,target));
            yield return x;
            GameManager.instance.currentPlayerState = PLAYER_STATE.IDLE;

            target = currentCamera.transform.position;
            target.x = this.transform.position.x + cameraOffsetX;
            x = Move(currentCamera.transform, target, 0.5f);
            yield return x;

            
            CreatePlatform();
            GameManager.instance.currentState = GAME_STATE.INPUT;
            Vector3 stickPosition = StickPositionStart(currentGround, currentStick);
            currentStick = StickPooling.instance.GetObjectFromPool();
            currentStick.BackDefault();
            currentStick.transform.position = stickPosition;
            currentStick.gameObject.SetActive(true);
        }

    }

    float GetTimeMove(Transform begin, Vector3 target)
    {
        float time= Mathf.Abs(target.x-begin.position.x )/ cameraOffsetX * _speedMove;
        return time;
    }

    public void ScaleStick()
    {
        Vector3 tmp = currentStick.transform.localScale;
        tmp.y += _speed * Time.deltaTime;
        if (tmp.y > maxScale)
            tmp.y = maxScale;
        currentStick.transform.localScale = tmp;
    }

    Vector3 StickPositionStart(Ground currentGround, Stick currentStick)
    {
        float x=currentGround.transform.position.x+ currentGround.GetLocalScaleX() * 0.5f-0.02f;
        Vector3 stickPosition =  new Vector3(x , 0, 0);
        stickPosition.y = currentStick.transform.position.y;
        stickPosition.z = currentStick.transform.position.z;
        return stickPosition;
    }


    public void CreateStartObjects()
    {
        RemovePooling();
        currentCamera.transform.position = new Vector3(cameraOffsetX+playerPositionOri.x, currentCamera.transform.position.y, currentCamera.transform.position.z);
        
        rig.gravityScale = 0f;
        rig.velocity = Vector3.zero;
        currentGround= GroundPooling.instance.GetObjectFromPool();
        currentGround.transform.position = groundPosOri;
        currentGround.SetLocalScale(groundScaleOri);
        currentGround.gameObject.SetActive(true);
        this.transform.position = playerPositionOri;
        nextGround = null;
        startPos = new Vector3(0,0,0);
        Vector3 tempDistance = new Vector3(Random.Range(spawnRange.x, spawnRange.y) + currentGround.GetLocalScaleX() * 0.5f + 0.5f, 0, 0);
        startPos += tempDistance;
        currentStick = StickPooling.instance.GetObjectFromPool();

        currentStick.BackDefault();
        currentStick.transform.position = stickPos;
        currentStick.gameObject.SetActive(true);
    }

    public void RemovePooling()
    {
        GroundPooling.instance.ReturnAllPooling();
        StickPooling.instance.ReturnAllPooling();
        EnemyPooling.instance.ReturnAllPooling();

    }

    public void CreatePlatform()
    {
        Ground currentPlatform = GroundPooling.instance.GetObjectFromPool();
        if (nextGround != null)
            currentGround = nextGround;
        nextGround = currentPlatform;
        nextGround.transform.position = GroundPooling.instance.getT().transform.position + startPos;
        float x= (nextGround.transform.position.x + nextGround.GetLocalScaleX() * 0.5f) > (this.halfWidth + currentCamera.transform.position.x)
            ? (this.halfWidth + currentCamera.transform.position.x) - nextGround.GetLocalScaleX() * 0.5f- 0.1f
            : nextGround.transform.position.x;
        nextGround.transform.position = new Vector3(x, nextGround.transform.position.y);
        nextGround.SetRandomSize(currentGround);
        nextGround.gameObject.SetActive(true);
        float tmp = nextGround.transform.position.x  - (currentGround.transform.position.x );
        Debug.Log(tmp);
        if (tmp>3.5f)
        {

            EnemyController e = EnemyPooling.instance.GetObjectFromPool();
            e.BackToDefault();
            e.RandomPosition(new Vector2(currentGround.transform.position.x + currentGround.GetLocalScaleX() + 1f, nextGround.transform.position.x - nextGround.GetLocalScaleX() - 0.5f));

            e.gameObject.SetActive(true);
        }
            


        Vector3 tempDistance = new Vector3(Random.Range(spawnRange.x, spawnRange.y) + currentGround.GetLocalScaleX() * 0.5f+0.5f, 0, 0);
        startPos += tempDistance;
    }



    IEnumerator Move(Transform currentTransform, Vector3 target, float time)
    {
        
        {
            var passed = 0f;
            var init = currentTransform.transform.position;
            while (passed < time)
            {
                if (GameManager.instance.currentPlayerState != PLAYER_STATE.DIE)
                //
                {
                    passed += Time.deltaTime;
                    var normalized = passed / time;
                    var current = Vector3.Lerp(init, target, normalized);
                    currentTransform.position = current;
                }
                else
                {
                    passed = time;
                }
                yield return null;
            }
        }

    }
    IEnumerator Rotate(Transform currentTransform, Transform target, float time)
    {
        var passed = 0f;
        var init = currentTransform.transform.rotation;
        while (passed < time)
        {
            passed += Time.deltaTime;
            var normalized = passed / time;
            var current = Quaternion.Slerp(init, target.rotation, normalized);
            currentTransform.rotation = current;
            yield return null;
        }
    }

    
}
