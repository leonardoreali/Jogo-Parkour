using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class Player : MonoBehaviour
{
    public float drag_grounded;
    public float drag_inair;

    public DetectObs detectVaultObject; //checa o vault object
    public DetectObs detectVaultObstruction; //checa se tem alguma coisa em frente ao player 
    public DetectObs detectClimbObject; //checa o objeto de escalada
    public DetectObs detectClimbObstruction; //checa se tem alguma coisa em frente ao player para escalar


    public DetectObs DetectWallL; //detecta a parede a esquerda
    public DetectObs DetectWallR; //detecta a parede a direita

    public Animator cameraAnimator;

    public float WallRunUpForce;
    public float WallRunUpForce_DecreaseRate;

    private float upforce;

    public float WallJumpUpVelocity;
    public float WallJumpForwardVelocity;
    public float drag_wallrun;
    public bool WallRunning;
    public bool WallrunningLeft;
    public bool WallrunningRight;
    private bool canwallrun; // confirma que o player só pode correr na parede uma vez antes de cair no solo 
                             // pode ser modificado para correr na parede dupla com um powerup(se eu resolver isso)

    public bool IsParkour;
    private float t_parkour;
    private float chosenParkourMoveTime;

    private bool CanVault;
    public float VaultTime; //quanto tempo leva o vault
    public Transform VaultEndPoint;

    private bool CanClimb;
    public float ClimbTime; //quanto tempo leva para escalar
    public Transform ClimbEndPoint;

    private RigidbodyFPS rbfps;
    private Rigidbody rb;
    private Vector3 RecordedMoveToPosition; //a posição do ponto final do vaul no espaço para mover o player 
    private Vector3 RecordedStartPosition; // posição do player antes de pular

    void Start()
    {
        rbfps = GetComponent<RigidbodyFPS>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rbfps.Grounded)
        {
            rb.drag = drag_grounded;
            canwallrun = true;
        }
        else
        {
            rb.drag = drag_inair;
        }
        if (WallRunning)
        {
            rb.drag = drag_wallrun;

        }
        //vault

        //se detecta um objeto vault e não há parede na frente,
        //então o jogador pode pressionar espaço e ir para frente como se tivesse planando 
        if (detectVaultObject.Obstruction && !detectVaultObstruction.Obstruction && !CanVault && !IsParkour && !WallRunning
            && (Input.GetKey(KeyCode.Space) || !rbfps.Grounded) && Input.GetAxisRaw("Vertical") > 0f) 
        {
            CanVault = true;
        }

        if (CanVault)
        {
            CanVault = false; 
            rb.isKinematic = true; //evita que a fisica pare o vault
            RecordedMoveToPosition = VaultEndPoint.position;
            RecordedStartPosition = transform.position;
            IsParkour = true;
            chosenParkourMoveTime = VaultTime;

            cameraAnimator.CrossFade("Vault", 0.1f);
        }

        //climb
        if (detectClimbObject.Obstruction && !detectClimbObstruction.Obstruction && !CanClimb && !IsParkour && !WallRunning
            && (Input.GetKey(KeyCode.Space) || !rbfps.Grounded) && Input.GetAxisRaw("Vertical") > 0f)
        {
            CanClimb = true;
        }

        if (CanClimb)
        {
            CanClimb = false; 
            rb.isKinematic = true; 
            RecordedMoveToPosition = ClimbEndPoint.position;
            RecordedStartPosition = transform.position;
            IsParkour = true;
            chosenParkourMoveTime = ClimbTime;

            cameraAnimator.CrossFade("Climb", 0.1f);
        }


        //Movimento do Parkour
        if (IsParkour && t_parkour < 1f)
        {
            t_parkour += Time.deltaTime / chosenParkourMoveTime;
            transform.position = Vector3.Lerp(RecordedStartPosition, RecordedMoveToPosition, t_parkour);

            if (t_parkour >= 1f)
            {
                IsParkour = false;
                t_parkour = 0f;
                rb.isKinematic = false;

            }
        }


        //Corrida na parede
        //se tiver uma parede a esquerda e o jogador não tiver no chão o parkour não funciona(climb/vault)
        if (DetectWallL.Obstruction && !rbfps.Grounded && !IsParkour && canwallrun) 
        {
            WallrunningLeft = true;
            canwallrun = false;
            upforce = WallRunUpForce; 
        }

        //se tiver uma parede a direita e o jogador não tiver no chão o parkour não funciona(climb/vault)
        if (DetectWallR.Obstruction && !rbfps.Grounded && !IsParkour && canwallrun) 
        {
            WallrunningRight = true;
            canwallrun = false;
            upforce = WallRunUpForce;
        }

        //aqui a camera vira de acordo com a posição
        if (WallrunningLeft && !DetectWallL.Obstruction || Input.GetAxisRaw("Vertical") <= 0f || rbfps.relativevelocity.magnitude < 1f) 
        {
            WallrunningLeft = false;
            WallrunningRight = false;
        }
        if (WallrunningRight && !DetectWallR.Obstruction || Input.GetAxisRaw("Vertical") <= 0f || rbfps.relativevelocity.magnitude < 1f) 
        {
            WallrunningLeft = false;
            WallrunningRight = false;
        }

        if (WallrunningLeft || WallrunningRight)
        {
            WallRunning = true;
            rbfps.Wallrunning = true;
        }
        else
        {
            WallRunning = false;
            rbfps.Wallrunning = false;
        }

        if (WallrunningLeft)
        {
            cameraAnimator.SetBool("WallLeft", true);
        }
        else
        {
            cameraAnimator.SetBool("WallLeft", false);
        }
        if (WallrunningRight)
        {
            cameraAnimator.SetBool("WallRight", true);
        }
        else
        {
            cameraAnimator.SetBool("WallRight", false);
        }

        if (WallRunning)
        {

            rb.velocity = new Vector3(rb.velocity.x, upforce, rb.velocity.z); //define a velocidade de y enquanto corre
            upforce -= WallRunUpForce_DecreaseRate * Time.deltaTime; 

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = transform.forward * WallJumpForwardVelocity + transform.up * WallJumpUpVelocity; //curva ao pular
                WallrunningLeft = false;
                WallrunningRight = false;
            }
            if (rbfps.Grounded)
            {
                WallrunningLeft = false;
                WallrunningRight = false;
            }
        }


    }

}
