using UnityEngine;
using Version_1.Domain;

namespace Version_1.Presentation
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private RayCaster _rayCaster;
        [SerializeField] private SegmentFactoryData _factoryData;
        [SerializeField] private RotationInputReader _rotationInputReader;
        
        private void Awake()
        {
            Interactions interactions = new();
            _rotationInputReader.Initialize(interactions);
            _rayCaster.Initialize(interactions);
            
            Factory factory = new(_factoryData);
            Builder builder = new(interactions, factory);
            Segment initial = Generator.CenterCube;
            builder.Select(initial);
            builder.TryBuild(Position.Center);
        }
    }
}