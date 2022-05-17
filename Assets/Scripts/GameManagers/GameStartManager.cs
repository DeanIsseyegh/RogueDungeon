using StarterAssets;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject playerStartCam;
    [SerializeField] private GameObject player;
    [SerializeField] private string playerIntroAnimationName = "Intro";

    private Animator _animator;
    private ThirdPersonController _thirdPersonController;
    private bool _hasPlayerIntroAnimStarted;

    private void Start()
    {
        _animator = player.GetComponent<Animator>();    
        _thirdPersonController = player.GetComponent<ThirdPersonController>();

        playerStartCam.SetActive(true);
        _thirdPersonController.enabled = false;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isPlayerIntroAnimInProgress = currentAnimatorStateInfo.IsName(playerIntroAnimationName);
        if (_hasPlayerIntroAnimStarted)
        {
            if (!isPlayerIntroAnimInProgress)
            {
                EndCutscene();
            }
        }
        else
        {
            _hasPlayerIntroAnimStarted = isPlayerIntroAnimInProgress;
        }
    }

    private void EndCutscene()
    {
        _thirdPersonController.enabled = true;
        this.enabled = false;
        playerStartCam.SetActive(false);
    }
}