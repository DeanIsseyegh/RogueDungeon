using UnityEngine;

public class FloorSignSpell : MonoBehaviour
{
    public float Lifetime { private get; set; } = 3f;
    private float _timer;

    // private void Awake()
    // {
    //     _timer = Lifetime;
    // }
    //
    // private void Update()
    // {
    //     _timer -= Time.deltaTime;
    //     if (_timer <= 0)
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}
