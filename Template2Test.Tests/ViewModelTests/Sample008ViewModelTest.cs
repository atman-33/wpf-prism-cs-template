﻿using Moq;
using Prism.Events;
using System.Reflection;
using Template2.Domain.Repositories;
using Template2.WPF.Services;
using Template2.WPF.ViewModels;

namespace Template2Test.Tests.ViewModelTests
{
    [TestClass]
    public class Sample008ViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(
                x => x.GetEvent<MainWindowSetSubTitleEvent>().Publish(It.IsAny<string>())).Callback<string>(
                    subTitle =>
                    {
                        //Assert.AreEqual("サブタイトル名称", subTitle);
                    });

            var taskMstCsvRepositoryMock = new Mock<ITaskMstCsvRepository>();

            var vm = new Sample008ViewModel(eventAggregatorMock.Object, taskMstCsvRepositoryMock.Object);

            var _eventAggregatorInfo = typeof(Sample008ViewModel).GetField("_eventAggregator", BindingFlags.NonPublic | BindingFlags.Instance);
            var _eventAggregator = (IEventAggregator?)_eventAggregatorInfo?.GetValue(vm);
            Assert.IsNotNull(_eventAggregator);
        }
    }
}
