﻿using Prism.Events;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Collections;
using Template2.WPF.Services;
using Template2.WPF.ViewModelEntities;

namespace Template2.WPF.ViewModels
{
    public class Sample011ViewModel : ViewModelBase
    {
        private IWorkerGroupMstRepository _workerGroupMstRepository;

        public Sample011ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, AbstractFactory.Create())
        {
        }

        public Sample011ViewModel(IEventAggregator eventAggregator, AbstractFactory factory)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル011（ドラッグ＆ドロップでDataGrid並び替え）");

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workerGroupMstRepository = factory.CreateWorkerGroupMst();

            //// Repositoryからデータ取得
            _workerGroupMstCollection = new WorkerGroupMstCollection(_workerGroupMstRepository);
            WorkerGroupMstCollection.LoadData();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private WorkerGroupMstCollection _workerGroupMstCollection;
        public WorkerGroupMstCollection WorkerGroupMstCollection
        {
            get { return _workerGroupMstCollection; }
            set { SetProperty(ref _workerGroupMstCollection, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Timer
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 


        #endregion
    }
}
