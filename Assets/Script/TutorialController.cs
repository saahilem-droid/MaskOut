using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public enum TutorialStep
    {
        MoveLeft,
        MoveRight,
        Jump,
        Masking,
        Done
    }

    [Header("UI")]
    public TMP_Text instructionText;

    [Header("Player")]
    public CubeController2D playerController;
    public PlatformToggleManager maskingSystem;

    [Header("Settings")]
    public float requiredMoveDistance = 1.5f;

    TutorialStep currentStep;

    Vector3 startPos;

    void Start()
    {
        currentStep = TutorialStep.MoveLeft;
        startPos = playerController.transform.position;

        LockPlayer();
        ShowInstruction();
    }

    void Update()
    {
        switch (currentStep)
        {
            case TutorialStep.MoveLeft:
                CheckMoveLeft();
                break;

            case TutorialStep.MoveRight:
                CheckMoveRight();
                break;

            case TutorialStep.Jump:
                CheckJump();
                break;

            case TutorialStep.Masking:
                CheckMasking();
                break;
        }
    }

    // ----------------------------------------------------

    void ShowInstruction()
    {
        switch (currentStep)
        {
            case TutorialStep.MoveLeft:
                instructionText.text = "PRESS A TO MOVE LEFT";
                break;

            case TutorialStep.MoveRight:
                instructionText.text = "PRESS D TO MOVE RIGHT";
                break;

            case TutorialStep.Jump:
                instructionText.text = "PRESS W TO JUMP";
                break;

            case TutorialStep.Masking:
                instructionText.text =
                    "MASK PLATFORMS\nHOLD F\nLEFT CLICK OR DRAG";
                break;

            case TutorialStep.Done:
                instructionText.text = "";
                instructionText.gameObject.SetActive(false);
                break;
        }
    }

    // ----------------------------------------------------

    void LockPlayer()
    {
        playerController.enabled = false;
        if (maskingSystem)
            maskingSystem.enabled = false;
    }

    void UnlockMovementOnly()
    {
        playerController.enabled = true;
    }

    void UnlockMasking()
    {
        if (maskingSystem)
            maskingSystem.enabled = true;
    }

    // ----------------------------------------------------
    // CHECKS
    // ----------------------------------------------------

    void CheckMoveLeft()
    {
        UnlockMovementOnly();

        if (Input.GetKey(KeyCode.A))
        {
            if (playerController.transform.position.x <
                startPos.x - requiredMoveDistance)
            {
                AdvanceStep(TutorialStep.MoveRight);
            }
        }
    }

    void CheckMoveRight()
    {
        UnlockMovementOnly();

        if (Input.GetKey(KeyCode.D))
        {
            if (playerController.transform.position.x >
                startPos.x + requiredMoveDistance)
            {
                AdvanceStep(TutorialStep.Jump);
            }
        }
    }

    void CheckJump()
    {
        UnlockMovementOnly();

        if (Input.GetKeyDown(KeyCode.W))
        {
            AdvanceStep(TutorialStep.Masking);
        }
    }

    void CheckMasking()
    {
        UnlockMasking();

        if (Input.GetKey(KeyCode.F) &&
            Input.GetMouseButtonDown(0))
        {
            AdvanceStep(TutorialStep.Done);
        }
    }

    // ----------------------------------------------------

    void AdvanceStep(TutorialStep next)
    {
        currentStep = next;
        startPos = playerController.transform.position;

        LockPlayer();
        ShowInstruction();
    }
}
