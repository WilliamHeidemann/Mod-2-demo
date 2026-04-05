using UnityEngine;

namespace Version_1.Presentation
{
    [CreateAssetMenu(order = 0)]
    public class SegmentFactoryData : ScriptableObject
    {
        public GameObject Cube;
        public GameObject Socket;
        public Material Material;
        public MonoSocket MonoSocket;
    }
}