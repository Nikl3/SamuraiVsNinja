using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class CharacterEngine : MonoBehaviour
    {
        #region VARIABLES

        [Space]
        [Header("Movement")]
        public float MoveSpeed = 12;
        public float AccelerationTimeGrounded = 0.1f;
        private float velocity_X_Smoothing;
        private Vector2 velocity;

        [Space]
        [Header("Jumping")]
        public float MaxJumpHeight = 6;
        public float MinJumpHeight = 1;
        public float TimeToJumpApex = 0.4f;
        public float AccelerationTimeAirborne = 0.2f;
        private float maxJumpVelocity;
        private float minJumpVelocity;
        private float gravity;

        [Space]
        [Header("Wall Slide")]
        public float WallStickTime = 0.25f;

        public float WallSlideSpeedMax = 3;
        public Vector2 WallJumpClimb = new Vector2(7.5f, 26);
        public Vector2 WallJumpOff = new Vector2(8.5f, 7);
        public Vector2 WallLeap = new Vector2(18, 17);
        private float wallUnstickTime;

        private Vector2 directionalInput;
        
        private Controller2D controller2D;
        private bool isWallSliding;
        private int wallDirection_X;

        private Coroutine iRespawning;
        private bool lockInput;
        private readonly float respawnMoveDuration = 2f;

        private CHARACTER_STATE currentCharacterState;

        #endregion VARIABLES

        #region PROPERTIES

        public SpriteRenderer SpriteRenderer
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            controller2D = GetComponentInChildren<Controller2D>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            gravity = -(2 * MaxJumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
            maxJumpVelocity = Mathf.Abs(gravity) * TimeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * MinJumpHeight);

            //print($"Gravity: {gravity} -- Jump velocity: {jumpVelocity}");

            CameraEngine.Instance.AddTarget(transform);

            ChangeState(CHARACTER_STATE.RESPAWNING);
        }

        private void Update()
        {
            if(lockInput)
            {
                return;
            }

            CalculateVelocity();

            HandleWallSliding();

            controller2D.Move(velocity * Time.deltaTime, directionalInput);

            if(controller2D.Collisions.IsBelow || controller2D.Collisions.IsAbove)
            {
                velocity.y = 0;
            }

            if(velocity != Vector2.zero)
            {
                SpriteRenderer.flipX = controller2D.Collisions.FaceDirection == 1;
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void ChangeState(CHARACTER_STATE newState)
        {
            currentCharacterState = newState;

            switch(currentCharacterState)
            {
                case CHARACTER_STATE.RESPAWNING:

                    if(iRespawning != null)
                    {
                        StopCoroutine(iRespawning);
                        iRespawning = null;
                    }

                    iRespawning = StartCoroutine(IRespawn());

                    break;
                case CHARACTER_STATE.INDESTRUCTIBLE:
                    break;
                default:
                    break;
            }
        }

        private IEnumerator IRespawn()
        {
            InputManager.Instance.LockInputs(true);

            lockInput = true;

            var moveID = LeanTween.move(
                gameObject,
                LevelManager.Instance.GetNearestCharacterSpawnPosition(transform.position),
                respawnMoveDuration).id;

            yield return new WaitWhile(() => LeanTween.isTweening(moveID));

            InputManager.Instance.LockInputs(false);

            lockInput = false;

            iRespawning = null;
        }

        public void OnMoveDown(Vector2 directionalInput)
        {
            this.directionalInput = directionalInput;
        }

        public void OnMoveUp()
        {
            directionalInput = Vector2.zero;
        }

        public void OnAttackInputDown()
        {
            Debug.Log("Attacking");
        }

        public void OnThrowInputDown()
        {
            Debug.Log("Throwing");
        }

        public void OnDashInputDown()
        {
            Debug.Log("Dashing");
        }     

        public void OnJumpInputDown()
        {
            if(isWallSliding)
            {
                if(wallDirection_X == directionalInput.x)
                {
                    velocity.x = -wallDirection_X * WallJumpClimb.x;
                    velocity.y = WallJumpClimb.y;
                }
                else if(directionalInput.x == 0)
                {
                    velocity.x = -wallDirection_X * WallJumpOff.x;
                    velocity.y = WallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirection_X * WallLeap.x;
                    velocity.y = WallLeap.y;
                }
            }

            if(controller2D.Collisions.IsBelow)
            {
                velocity.y = maxJumpVelocity;
            }
        }

        public void OnJumpInputUp()
        {
            if(velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        private void HandleWallSliding()
        {
            wallDirection_X = controller2D.Collisions.IsLeft ? -1 : 1;

            isWallSliding = false;

            if((controller2D.Collisions.IsLeft || controller2D.Collisions.IsRight) && controller2D.Collisions.IsBelow == false
                 && velocity.y < 0)
            {
                isWallSliding = true;

                if(velocity.y < -WallSlideSpeedMax)
                {
                    velocity.y = -WallSlideSpeedMax;
                }

                if(wallUnstickTime > 0)
                {
                    velocity_X_Smoothing = 0;
                    velocity.x = 0;

                    if(directionalInput.x != wallDirection_X && directionalInput.x != 0)
                    {
                        wallUnstickTime -= Time.deltaTime;
                    }
                    else
                    {
                        wallUnstickTime = WallStickTime;
                    }
                }
                else
                {
                    wallUnstickTime = WallStickTime;
                }
            }
        }

        private void CalculateVelocity()
        {
            var targetVelocity_X = directionalInput.x * MoveSpeed;

            velocity.x = Mathf.SmoothDamp(
                velocity.x,
                targetVelocity_X,
                ref velocity_X_Smoothing,
                controller2D.Collisions.IsBelow ? AccelerationTimeGrounded : AccelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}

