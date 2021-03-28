using UnityEngine;
 
public class CharacterMovement : MonoBehaviour
{
    public float animSpeed = 1.5f;				// アニメーション再生速度設定
    public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
    public bool useCurves = true;				// Mecanimでカーブ調整を使うか設定する
    public float useCurvesHeight = 0.5f;		// カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

    public float forwardSpeed = 7.0f;
    public float backwardSpeed = 2.0f;
    public float rotateSpeed = 2.0f;
    public float jumpPower = 3.0f; 
    private CapsuleCollider col;
    private Rigidbody rb;
    private Vector3 velocity;
    private float orgColHight;
    private Vector3 orgVectColCenter;
    private Animator anim;							// キャラにアタッチされるアニメーターへの参照
    private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照
    private float v=1;

    private GameObject cameraObject;	// メインカメラへの参照
    
    static int idleState = Animator.StringToHash ("Base Layer.Idle");
    static int locoState = Animator.StringToHash ("Base Layer.Locomotion");

    Transform player;
    //PlayerHealth playerHealth;
    int playerNumber=1;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
 
    void Start ()
    {
        anim = GetComponent<Animator> ();
        col = GetComponent<CapsuleCollider> ();
        rb = GetComponent<Rigidbody> ();
        orgColHight = col.height;
        orgVectColCenter = col.center;
    }


    /*void FixedUpdate ()
    {
        anim.SetFloat ("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat ("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
        anim.speed = animSpeed;								// Animatorのモーション再生速度に animSpeedを設定する
        currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

        if (currentBaseState.nameHash == locoState) {
            if (useCurves) {
                resetCollider ();
            }
        }
        else if (currentBaseState.nameHash == idleState) {
            if (useCurves) {
                resetCollider ();
            }
        }
    }*/

    void resetCollider ()
    {
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }

    void Awake ()
    {
        //Cari game object with tag player
        GameObject[] res = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < res.Length; i++){
            if (res[i].GetComponent<TankController>().m_PlayerNumber != playerNumber){
                player = res[i].transform;
            }
        } 
        //Mendapatkan componen reference
        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
 
 
    void FixedUpdate ()
    {
        //Pindah ke player position
        //if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth> 0)
        //{
        nav.SetDestination(player.position);
        if (nav.remainingDistance < 3.0f)
        {
            anim.SetFloat ("Speed", 0f);
            anim.SetBool ("Kick", true);
            if (useCurves){
                resetCollider();
            }
        }
        else{
            anim.SetFloat ("Speed", 1.5f);            
            anim.SetBool ("Kick", false);
            if (useCurves){
                resetCollider();
            }
        }
        //}
        /*else //Stop moving
        {
            nav.enabled = false;
        }*/
    }
}