using System.Collections.Generic;
using UnityEngine;
using UtilityToolkit.Runtime;
using Version_1.Utility;

namespace Version_1.Presentation
{
    [CreateAssetMenu(menuName = "SegmentInstantiator")]
    public class SegmentInstantiator : ScriptableObject
    {
        [SerializeField] private GameObject _cellRenderPrefab;
        [SerializeField] private GameObject _socketRenderPrefab;
        [SerializeField] private Material[] _materials;
        
        [SerializeField] private Cell _cell;
        [SerializeField] private MonoSocket _monoSocket;

        public MonoSegment Instantiate(Segment segment)
        {
            MonoSegment monoSegment = new GameObject().AddComponent<MonoSegment>();

            Material material = _materials.RandomElement();
            foreach (Position position in segment.Positions)
            {
                Instantiate(_cell, position.ToVector3(), Quaternion.identity, monoSegment.transform);
                GameObject box = Instantiate(_cellRenderPrefab, position.ToVector3(), Quaternion.identity, monoSegment.transform);
            }

            foreach ((Position a, Position b) in segment.Positions.PairsOfNeighbors())
            {
                var halfwayPoint = (a.ToVector3() + b.ToVector3()) / 2f;
                GameObject box = Instantiate(_cellRenderPrefab, halfwayPoint, Quaternion.identity, monoSegment.transform);
            }
            
            foreach (Socket socket in segment.Sockets)
            {
                // Direction is in local space. If the object is rotated
                Quaternion rotation = Quaternion.LookRotation(socket.Direction.Value.ToVector3());
                GameObject socketRender = Instantiate(_socketRenderPrefab, socket.Position.ToVector3(), rotation, monoSegment.transform);
                socketRender.GetComponentInChildren<MeshRenderer>().material = material;
                MonoSocket monoSocket = Instantiate(_monoSocket, socket.Position.ToVector3(), Quaternion.identity,
                    monoSegment.transform);
                monoSocket.Initialize(socket.Archetype, socket.Direction.ToSerializedDirection());
            }

            return monoSegment;
        }
    }
}