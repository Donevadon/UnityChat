using System;
using MonoBehaviorClient;
using Receivers;
using Server;
using UnityEngine;
using UserComponent.MessageSystem;
using UserComponent.MessageSystem.MessageSetting;
using UserComponent.UserSystem;
using Zenject;

namespace Infrastructure
{
    public class Bootstrap : MonoInstaller
    {
        [SerializeField] private InputMessageSettings messageSettings;
        [SerializeField] private Async async;
    
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            var receiver = new Receiver();
            Container.Bind<IConnectionWindow>().FromInstance(messageSettings);
            Container.Bind<IConnection>().To<ConnectionServer>().FromNew().AsSingle();
            Container.Bind<IReceiver>().To<Receiver>().FromInstance(receiver);
            Container.Bind<IMessageReceived>().To<Receiver>().FromInstance(receiver);
            Container.Bind<IUserReceived>().To<Receiver>().FromInstance(receiver);
            Container.Bind<IMonoBehaviorAsync<Action>>().FromInstance(async);
        }
    }
}