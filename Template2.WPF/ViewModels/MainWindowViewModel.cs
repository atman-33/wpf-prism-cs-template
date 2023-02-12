﻿using Prism.Commands;
using Prism.Regions;
using System;
using Template2.Infrastructure;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private NavigationParameters _parameters = new NavigationParameters();

        public MainWindowViewModel(IRegionManager regionManager)
        {
            //// 画面遷移用（ナビゲーション）
            _regionManager = regionManager;

            //// 初期画面
            //// ex. _regionManager.RegisterViewWithRegion("ContentRegion", nameof(HomeView));

            //// DelegateCommandメソッドを登録
            WindowContentRendered = new DelegateCommand(WindowContentRenderedExecute);
            Sample001ViewButton = new DelegateCommand(Sample001ViewButtonExecute);
            Sample002ViewButton = new DelegateCommand(Sample002ViewButtonExecute);
            Sample003ViewButton = new DelegateCommand(Sample003ViewButtonExecute);
            Sample004ViewButton = new DelegateCommand(Sample004ViewButtonExecute);
            Sample005ViewButton = new DelegateCommand(Sample005ViewButtonExecute);

            //// 自身をパラメータに格納
            _parameters.Add(nameof(MainWindowViewModel), this);
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// タイトル
        /// </summary>
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// 画面の概要
        /// </summary>
        private string _viewOutline;
        public string ViewOutline
        {
            get { return _viewOutline; }
            set { SetProperty(ref _viewOutline, value); }
        }

        private bool _dbConnectionIsChecked = false;
        public bool DBConnectionIsChecked
        {
            get { return _dbConnectionIsChecked; }
            set { SetProperty(ref _dbConnectionIsChecked, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand WindowContentRendered { get; }

        private void WindowContentRenderedExecute()
        {
            try
            {
                //// DB接続確認
                Factories.Open();
                DBConnectionIsChecked = true;
            }
            catch (Exception e)
            {
                DBConnectionIsChecked = false;
                throw new Exception(e.Message, e) ;
            }

            //// ※注意：App.xaml.cs内のDispatcherUnhandledExceptionでは、
            //// コンストラクタ内の例外処理はキャッチできない。
            //// そのため、コンストラクタ内のDB接続処理はContentRenderedイベントで処理し、
            //// DB接続エラーをキャッチする方が良い。
        }

        public DelegateCommand Sample001ViewButton { get; }

        private void Sample001ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample001View), _parameters);
        }

        public DelegateCommand Sample002ViewButton { get; }

        private void Sample002ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample002View), _parameters);
        }

        public DelegateCommand Sample003ViewButton { get; }

        private void Sample003ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample003View), _parameters);
        }

        public DelegateCommand Sample004ViewButton { get; }

        private void Sample004ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample004PageListView), _parameters);
        }
        public DelegateCommand Sample005ViewButton { get; }

        private void Sample005ViewButtonExecute()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Sample005View), _parameters);
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion
    }
}
