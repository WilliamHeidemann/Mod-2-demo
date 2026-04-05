using UnityEngine;
using Version_1.Domain;
using Version_1.Utility;

namespace Version_1.Presentation
{
    [CreateAssetMenu(order = 0)]
    public class SegmentFactoryData : ScriptableObject
    {
        public GameObject Cube;
        public GameObject Socket;
        public SerializedDictionary<Archetype,Material> Materials;
        public MonoSocket MonoSocket;
    }
}