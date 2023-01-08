﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample001ViewModel : BindableBase, INavigationAware
    {
        /// <summary>
        /// MainWindow
        /// </summary>
        private MainWindowViewModel _mainWindowViewModel;

        //// メッセージボックス
        private IMessageService _messageService;

        //// 外部接触Repository
        private ISampleMstRepository _sampleMstRepository;


        public Sample001ViewModel()
            : this(Factories.CreateSampleMst())
        {
        }

        public Sample001ViewModel(ISampleMstRepository sampleMstRepository)
        {
            //// メッセージボックス
            _messageService = new MessageService();

            //// DelegateCommandメソッドを登録
            SampleMstEntitiesSelectedCellsChanged = new DelegateCommand(SampleMstEntitiesSelectedCellsChangedExecute);
            NewButton = new DelegateCommand(NewButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            DeleteButton = new DelegateCommand(DeleteButtonExecute);

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _sampleMstRepository = sampleMstRepository;

            //// Repositoryからデータ取得
            UpdateSampleMstEntities();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<Sample001ViewModelSampleMst> _sampleMstEntities
            = new ObservableCollection<Sample001ViewModelSampleMst>();
        public ObservableCollection<Sample001ViewModelSampleMst> SampleMstEntities
        {
            get { return _sampleMstEntities; }
            set { SetProperty(ref _sampleMstEntities, value); }
        }

        private Sample001ViewModelSampleMst _sampleMstEntitiesSlectedItem;
        public Sample001ViewModelSampleMst SampleMstEntitiesSlectedItem
        {
            get { return _sampleMstEntitiesSlectedItem; }
            set { SetProperty(ref _sampleMstEntitiesSlectedItem, value); }
        }

        private string _sampleCodeText;
        public string SampleCodeText
        {
            get { return _sampleCodeText; }
            set { SetProperty(ref _sampleCodeText, value); }
        }

        private bool _sampleCodeIsEnabled = false;
        public bool SampleCodeIsEnabled
        {
            get { return _sampleCodeIsEnabled; }
            set { SetProperty(ref _sampleCodeIsEnabled, value); }
        }

        private string _sampleNameText;
        public string SampleNameText
        {
            get { return _sampleNameText; }
            set { SetProperty(ref _sampleNameText, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand SampleMstEntitiesSelectedCellsChanged { get; }
        private void SampleMstEntitiesSelectedCellsChangedExecute()
        {
            if (SampleMstEntitiesSlectedItem == null)
            {
                return;
            }

            SampleCodeText = SampleMstEntitiesSlectedItem.SampleCode;
            SampleNameText = SampleMstEntitiesSlectedItem.SampleName;

            SampleCodeIsEnabled = false;
        }

        public DelegateCommand NewButton { get; }
        private void NewButtonExecute()
        {
            SampleCodeIsEnabled = true;
            SampleNameText = String.Empty;
        }

        public DelegateCommand SaveButton { get; }
        private void SaveButtonExecute()
        {
            Guard.IsNullOrEmpty(SampleNameText, "サンプル名称を入力してください。");

            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new SampleMstEntity(
                SampleCodeText,
                SampleNameText
                );

            _sampleMstRepository.Save(entity);
            _messageService.ShowDialog("保存しました。", "情報", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            UpdateSampleMstEntities();
        }

        public DelegateCommand DeleteButton { get; }
        private void DeleteButtonExecute()
        {
            if (_messageService.Question("「" + SampleNameText + "」を削除しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new SampleMstEntity(
                SampleCodeText,
                SampleNameText
                );

            _sampleMstRepository.Delete(entity);
            _messageService.ShowDialog("削除しました。", "情報", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            UpdateSampleMstEntities();
        }

        #endregion


        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //// 遷移前の画面からパラメータ受け取り
            _mainWindowViewModel = navigationContext.Parameters.GetValue<MainWindowViewModel>("MainWindow");
            _mainWindowViewModel.ViewOutline = "> サンプル001（マスタテーブル編集）";

        }

        private void UpdateSampleMstEntities()
        {
            SampleMstEntities.Clear();

            foreach (var entity in _sampleMstRepository.GetData())
            {
                SampleMstEntities.Add(new Sample001ViewModelSampleMst(entity));
            }
        }

        #endregion    
    }
}