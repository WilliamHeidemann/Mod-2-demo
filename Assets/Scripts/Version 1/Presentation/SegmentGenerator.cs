using UnityEngine;
using UtilityToolkit.Runtime;
using Version_1.Domain;

namespace Version_1.Presentation
{
    public class SegmentGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _boxPrefab;
        [SerializeField] private Cell _cell;
        [SerializeField] private MonoSocket _monoSocket;
        [SerializeField] private Material[] _materials;

        public MonoSegment GenerateAndInstantiate()
        {
            Segment segment = Generator.Generate();
            
            MonoSegment monoSegment = new GameObject().AddComponent<MonoSegment>();

            var material = _materials.RandomElement();
            foreach (var position in segment.Positions)
            {
                Instantiate(_cell, position.ToVector3(), Quaternion.identity, monoSegment.transform);
                var box = Instantiate(_boxPrefab, position.ToVector3(), Quaternion.identity, monoSegment.transform);
                box.GetComponent<MeshRenderer>().material = material;
            }

            foreach (var socket in segment.Sockets)
            {
                MonoSocket monoSocket = Instantiate(_monoSocket, socket.Position.ToVector3(), Quaternion.identity,
                    monoSegment.transform);
                monoSocket.Initialize(socket.Archetype, socket.Direction.ToSerializedDirection());
            }

            return monoSegment;
        }
    }
}