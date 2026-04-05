using System;
using UnityEngine;
using Version_1.Domain;

namespace Version_1.Presentation
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private RayCaster _rayCaster;
        [SerializeField] private SegmentFactoryData _factoryData;
        
        private void Awake()
        {
            Interactions interactions = new();
            // interactions.OnHoverEnter += _ => print("Hover Enter");
            // interactions.OnHoverExit += () => print("Hover Exit");
            // interactions.OnTryBuild += _ => print("Try Build");
            
            
            _rayCaster.Initialize(interactions);
            Factory factory = new(_factoryData);
            Builder builder = new(interactions, factory);
            Segment initial = Generator.Generate();
            builder.Select(initial);
            builder.TryBuild(Position.Center);
        }
    }
}