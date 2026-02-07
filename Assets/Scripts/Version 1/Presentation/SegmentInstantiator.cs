using UnityEngine;
using UnityEngine.Serialization;
using UtilityToolkit.Runtime;

namespace Version_1.Presentation
{
    [CreateAssetMenu(menuName = "SegmentInstantiator")]
    public class SegmentInstantiator : ScriptableObject
    {
        [SerializeField] private GameObject _cellRenderPrefab;
        [SerializeField] private Cell _cell;
        [SerializeField] private MonoSocket _monoSocket;
        [SerializeField] private Material[] _materials;

        public MonoSegment Instantiate(Segment segment)
        {
            MonoSegment monoSegment = new GameObject().AddComponent<MonoSegment>();

            var material = _materials.RandomElement();
            foreach (var position in segment.Positions)
            {
                Instantiate(_cell, position.ToVector3(), Quaternion.identity, monoSegment.transform);
                var box = Instantiate(_cellRenderPrefab, position.ToVector3(), Quaternion.identity, monoSegment.transform);
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