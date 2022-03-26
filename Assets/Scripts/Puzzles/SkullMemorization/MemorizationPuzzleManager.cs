using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

public class MemorizationPuzzleManager : MonoBehaviour
{

    [SerializeField] public GameObject startPosition;
    [SerializeField] public GameObject skullPrefab;
    [SerializeField] public int numOfColumns = 3;
    [SerializeField] public int skullsPerRow = 3;
    [SerializeField] public float spacingInRow = 2;
    [SerializeField] public float spacingInColumn = 2;
    [SerializeField] public int skullsToMemorize = 3;
    [SerializeField] public float timeBetweenSkullLighting = 1.5f;
    [SerializeField] public float initialDelay = 5f;

    private List<GameObject> _skulls;
    private List<Skull> _skullsPartOfGame;

    private State _state;
    public bool IsGameWon { get; private set; }

    private enum State
    {
        STARTING,
        SKULS_LIGHTING_UP,
        PLAYING,
        FAILED,
        SUCCESS,
        FINISHED
    }

    void Start()
    {
        _skulls = new List<GameObject>();
        Vector3 currentPos = startPosition.transform.position;
        for (int z = 0; z < numOfColumns; z++)
        {
            for (int x = 0; x < skullsPerRow; x++)
            {
                GameObject createdSkull = Instantiate(skullPrefab, currentPos, Quaternion.identity);
                _skulls.Add(createdSkull);
                currentPos -= new Vector3(spacingInRow, 0, 0);
            }
            currentPos -= new Vector3(0, spacingInColumn, 0);
            currentPos.x = startPosition.transform.position.x;
        }

        _state = State.STARTING;
    }

    void Update()
    {
        switch (_state)
        {
            case State.STARTING:
                Debug.Log("Starting");
                MarkAllSkullsNotReady();
                DecideSkullsToBeHit();
                LightSkullsUp(_skullsPartOfGame);
                _state = State.SKULS_LIGHTING_UP;
                break;
            
            case State.SKULS_LIGHTING_UP:
                Debug.Log("Skulls lighting up");
                bool isAllSkullsLitUp = _skullsPartOfGame.All(it => it.HasLitUp);
                if (isAllSkullsLitUp)
                {
                    RegisterAllSkulls();
                    MakeAllSkullsHittable();
                    MarkAllSkullsReady();
                    _state = State.PLAYING;
                }
                break;

            case State.PLAYING:
                break;
            
            case State.SUCCESS:
                StartCoroutine(FlashGameFinished());
                _state = State.FINISHED;
                break;
            
            case State.FAILED:
                Debug.Log("Failed");
                _skullsPartOfGame.Clear();
                MarkAllSkullsNotReady();
                UnmarkAllSkulls();
                MakeAllSkullsUnHittable();
                _state = State.STARTING;
                break;
            
            case State.FINISHED:
                IsGameWon = true;
                this.enabled = false;
                break;
        }

    }

    private void MarkAllSkullsFinished()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().MarkFinished());
    }

    private void MakeAllSkullsUnHittable()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().MakeUnHittable());
    }

    private void UnmarkAllSkulls()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().Unmark());
    }

    private void RegisterAllSkulls()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().Register(this));
    }

    private void MakeAllSkullsHittable()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().MakeHittable());
    }

    private void MarkAllSkullsReady()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().MarkReady());
    }

    private void MarkAllSkullsNotReady()
    {
        _skulls.ForEach(it => it.GetComponent<Skull>().MarkUnready());
    }

    private void DecideSkullsToBeHit()
    {
        List<GameObject> skullsNotPartOfGame = new List<GameObject>(_skulls);
        _skullsPartOfGame = new List<Skull>();
        for (int i = 0; i < skullsToMemorize; i++)
        {
            int randomIndx = Random.Range(0, skullsNotPartOfGame.Count);
            GameObject randomSkull = skullsNotPartOfGame[randomIndx];
            skullsNotPartOfGame.Remove(randomSkull);
            Skull skull = randomSkull.GetComponent<Skull>();
            _skullsPartOfGame.Add(skull);
        }
    }

    private void LightSkullsUp(List<Skull> skullsPartOfGame)
    {
        StartCoroutine(LightUpSkulls(initialDelay, timeBetweenSkullLighting, skullsPartOfGame));
    }

    private IEnumerator LightUpSkulls(float initialDelay, float timeBetweenSkullLighting, List<Skull> skulls)
    {
        yield return new WaitForSeconds(initialDelay);
        for (var i = 0; i < skulls.Count; i++)
        {
            Skull skull = skulls[i];
            skull.LightUp();
            yield return new WaitForSeconds(timeBetweenSkullLighting);
        }
        yield return new WaitForSeconds(timeBetweenSkullLighting);
        skulls.ForEach(it => it.HasLitUp = true);
    }
    
    private IEnumerator FlashGameFinished()
    {
        yield return new WaitForSeconds(0.5f);
        for (var i = 0; i < 4; i++)
        {
            MarkAllSkullsReady();
            yield return new WaitForSeconds(0.25f);
            MarkAllSkullsNotReady();
            yield return new WaitForSeconds(0.25f);
        }
        MarkAllSkullsFinished();
    }

    public void NotifySkullHit(Skull hitSkull)
    {
        if (_state != State.PLAYING) return;
        if (_skullsPartOfGame.First().Equals(hitSkull))
        {
            _skullsPartOfGame.Remove(hitSkull);
            hitSkull.MarkSuccessfulHit();
            if (_skullsPartOfGame.IsNullOrEmpty())
                _state = State.SUCCESS;
        }
        else
        {
            _state = State.FAILED;
        }
    }
    
}
