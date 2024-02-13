using System.Collections;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerAttackState{
    Attacking,
    Ready
}
public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    

    [Header("Animations")]
    private Animator animator;
    [SerializeField] private AnimationClip swingAnimation;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip swingSound;
    [SerializeField] float swingSoundDelay = .45f;

    [Header("Walk")]
    private Vector3 playerVelocity;
    [SerializeField] private float speed = 5.0f;

    [Header("Jump")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Attack")]
    [SerializeField] private GameObject axe;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackDelay = .6f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask attackLayer;    
    private PlayerAttackState attackState = PlayerAttackState.Ready;

    [Header("Camera/Look")]
    [SerializeField] private Camera cam;
    private float xRotation = 0.0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;


    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection)*speed*Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

        // Rotate player to look horizontally
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }

    public void Attack()
    {
        if (attackState != PlayerAttackState.Ready) return;
        attackState = PlayerAttackState.Attacking;
        AttackAnimation();
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
        audioSource.pitch = Random.Range(.7f, 1.3f);
        
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AttackAnimation()
    {
        Invoke(nameof(PlaySwingSound),swingSoundDelay);
        Animator anim = axe.GetComponent<Animator>();
        anim.SetTrigger("Attack");
    }
    private void ResetAttack()
    {
        attackState = PlayerAttackState.Ready;
    }

    private void PlaySwingSound()
    {
        audioSource.PlayOneShot(swingSound);
    }

    private void AttackRaycast()
    {
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackRange, attackLayer))
        {
            if(hit.transform.TryGetComponent(out HittableObject target)){
                target.TakeDamage(attackDamage, hit.point, transform.position - cam.transform.forward); // Setting hit origin to a meter behind the player here because we were getting some weirdness with detecting which side of the glass the player was on. It seems to be working the way I want it to now. 
            }
;
        }

    }


}
