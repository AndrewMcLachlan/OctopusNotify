﻿using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Moq;
using OctopusNotify.App.Ioc;

namespace OctopusNotify.App.Tests
{
    public class MockContainer : Container
    {
        public Mock<IDeploymentRepositoryAdapter> MockDeploymentRepositoryAdapter { get; set; }

        public bool ThrowsResolutionException { get; set; }

        public MockContainer(Mock<IDeploymentRepositoryAdapter> mockDeploymentRepositoryAdapter)
        {
            MockDeploymentRepositoryAdapter = mockDeploymentRepositoryAdapter;
        }

        public override T Resolve<T>()
        {
            if (ThrowsResolutionException)
            {
                // Try resolving an unregistered type.
                base.Resolve<IEnumerable<T>>();
            }
            return base.Resolve<T>();
        }

        protected override void RegisterTypes()
        {
            UnityContainer.RegisterInstance(MockDeploymentRepositoryAdapter.Object);
        }
    }
}
