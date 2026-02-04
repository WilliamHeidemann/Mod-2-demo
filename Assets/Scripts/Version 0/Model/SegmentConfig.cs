using UnityEngine;

namespace Model
{
    [CreateAssetMenu(menuName = "Create SegmentConfig", fileName = "SegmentConfig", order = 0)]
    public class SegmentConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Segment _segment;

        public GameObject Prefab => _prefab;
        public Segment Segment => _segment;

        public void Write(GameObject prefab, Segment definition)
        {
            _prefab = prefab;
            _segment = definition;
        }
    }
}