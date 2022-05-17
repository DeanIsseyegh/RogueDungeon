using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [field: SerializeField] public ChoiceIcon Icon { get; set; }
    [field: SerializeField] public ChoiceTitle Title { get; set; }
    [field: SerializeField] public ChoiceDescription Description { get; set; }
    [field: SerializeField] public ChoiceStats Stats { get; set; }
}
