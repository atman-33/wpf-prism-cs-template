﻿using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Dto.Input;
using Template2.WPF.Services;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class Sample004PageEditingViewModel : ViewModelBase, IDialogAware
    {
        //// 外部接触Repository
        private IPageMstRepository _pageMstRepository;

        /// <summary>
        /// プレビューのViewModel
        /// </summary>
        private Sample004PagePreviewViewModel _pagePreviewViewModel;

        public Sample004PageEditingViewModel(IRegionManager regionManager)
            : this(regionManager, new MessageService(),AbstractFactory.Create())
        {
        }

        public Sample004PageEditingViewModel(
            IRegionManager regionManager,
            IMessageService messageService,
            AbstractFactory factory)
        {
            MainRegionManager = regionManager;  
            
            //// 【補足】
            //// Dialogの場合、DialogのRegionManagerに、本体のRegionManagerを設定する必要あり

            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion(_contentRegionName, nameof(Sample004PagePreviewView));

            _pageMstRepository = factory.CreatePageMst();
            _messageService = messageService;

            //// DelegateCommandメソッドを登録
            ViewLoaded = new DelegateCommand(ViewLoadedExecute);

            CancelButton = new DelegateCommand(CancelButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            DeleteButton = new DelegateCommand(DeleteButtonExecute);

            PreviewButton = new DelegateCommand(PreviewButtonExecute);
            OpenMovieFileButton = new DelegateCommand(OpenMovieFileButtonExecute);
            OpenImageFileButton = new DelegateCommand(OpenImageFileButtonExecute);
            ImagePageNoDownButton = new DelegateCommand(ImagePageNoDownButtonExecute);
            ImagePageNoUpButton = new DelegateCommand(ImagePageNoUpButtonExecute);

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _pageMstRepository = factory.CreatePageMst();

            //// 新規の説明入力レコードを生成
            for (int i = 1; i <= 3; i++)
            {
                NoteEntities.Add(new NoteEntity(string.Empty));
            }
        }

        public string Title => "ページ編集";

        /// <summary>
        /// 新規ページ登録の時はTrue
        /// </summary>
        public bool IsNewPage { get; private set; }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            //// 画面内のリージョンを除去しておかないとエラーが発生してしまう
            _regionManager.Regions.Remove(ContentRegionName);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var dto = parameters.GetValue<Sample004PageEditingViewModelInputDto>(nameof(Sample004PageEditingViewModelInputDto));

            //// 新規ページかどうか
            IsNewPage = dto.IsNewPage;

            if (IsNewPage)
            {
                PageIdText = "（新規のため未採番）";
            }
            else
            {
                var entity = dto.PageMstEntityToEdit;

                //// 編集対象のエンティティ情報を初期値に設定
                PageIdText = entity.PageId.Value.ToString();
                PageNameText = entity.PageName.Value;
                MovieLinkText = entity.MovieLink.Value;
                ImageFolderLinkText = entity.ImageFolderLink.Value;
                ImagePageNoText = entity.ImagePageNo.Value;
                SlideWaitingTimeText = entity.SlideWaitingTime.Value;

                NoteEntities[0] = new NoteEntity(entity.Note1.Value);
                NoteEntities[1] = new NoteEntity(entity.Note2.Value);
                NoteEntities[2] = new NoteEntity(entity.Note3.Value);
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// メイン画面のリージョンマネージャー
        /// ※ダイアログ画面（子Window）からContentRegionを画面遷移する際は必要
        /// </summary>
        private IRegionManager _mainRegionManager;
        public IRegionManager MainRegionManager
        {
            get { return _mainRegionManager; }
            set { SetProperty(ref _mainRegionManager, value); }
        }

        private readonly string _contentRegionName = "PageEditingPagePreviewContentRegion";
        public string ContentRegionName
        {
            get { return _contentRegionName; }
        }

        private string _pageIdText = string.Empty;
        public string PageIdText
        {
            get { return _pageIdText; }
            set { SetProperty(ref _pageIdText, value); }
        }

        private string _pageNameText = string.Empty;
        public string PageNameText
        {
            get { return _pageNameText; }
            set { SetProperty(ref _pageNameText, value); }
        }

        private string _movieLinkText = string.Empty;
        public string MovieLinkText
        {
            get { return _movieLinkText; }
            set { SetProperty(ref _movieLinkText, value); }
        }

        private string _imageFolderLinkText = string.Empty;
        public string ImageFolderLinkText
        {
            get { return _imageFolderLinkText; }
            set { SetProperty(ref _imageFolderLinkText, value); }
        }

        private int? _imagePageNoText = null;
        public int? ImagePageNoText
        {
            get { return _imagePageNoText; }
            set { SetProperty(ref _imagePageNoText, value); }
        }

        private float? _slideWaitingTimeText = null;
        public float? SlideWaitingTimeText
        {
            get { return _slideWaitingTimeText; }
            set { SetProperty(ref _slideWaitingTimeText, value); }
        }

        private ObservableCollection<NoteEntity> _noteEntities = new ObservableCollection<NoteEntity>();
        public ObservableCollection<NoteEntity> NoteEntities
        {
            get { return _noteEntities; }
            set { SetProperty(ref _noteEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        public DelegateCommand ViewLoaded { get; }

        private void ViewLoadedExecute()
        {
            //// 部分ViewのViewModelをプライベート変数に格納
            var view = _regionManager.Regions[_contentRegionName].Views.FirstOrDefault() as Sample004PagePreviewView;
            _pagePreviewViewModel = view.DataContext as Sample004PagePreviewViewModel;

            //// 【補足】
            //// 1つのContentRegionに1つのViewが対応しているため、Views.FirstOrDefaultでOK

            if (!IsNewPage)
            {
                _pagePreviewViewModel.PreviewPageMstEntity = CreateEntity();
                _pagePreviewViewModel.ShowPreviewImmediately = true;
            }
        }

        public DelegateCommand OpenMovieFileButton { get; }
        private void OpenMovieFileButtonExecute()
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "動画ファイルを選択してください",
                IsFolderPicker = false,     //// フォルダ選択モードにしない
            })
            {
                //// ファイルの種類を設定
                cofd.Filters.Add(new CommonFileDialogFilter("mp4", "*.mp4"));
                cofd.Filters.Add(new CommonFileDialogFilter("avi", "*.avi"));
                cofd.Filters.Add(new CommonFileDialogFilter("mov", "*.mov"));

                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                MovieLinkText = cofd.FileName;
            }

            //// 動画プレビュー更新
            _pagePreviewViewModel.PreviewMovie(MovieLinkText);
        }

        public DelegateCommand OpenImageFileButton { get; }
        private void OpenImageFileButtonExecute()
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "画像フォルダを選択してください",
                IsFolderPicker = true,      //// フォルダ選択モードにする
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                ImageFolderLinkText = cofd.FileName;
            }

            //// ページNoを設定
            ImagePageNoText = 1;

            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }
        public DelegateCommand ImagePageNoDownButton { get; }
        private void ImagePageNoDownButtonExecute()
        {
            if (ImagePageNoText <= 1)
            {
                return;
            }

            ImagePageNoText = ImagePageNoText - 1;

            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }
        public DelegateCommand ImagePageNoUpButton { get; }
        private void ImagePageNoUpButtonExecute()
        {
            //// ページNoをインクリしてファイルが無ければ何もしない
            string filePath = PageMstEntity.GetImageFilePath(ImageFolderLinkText, ImagePageNoText + 1);

            if (File.Exists(filePath) == false)
            {
                return;
            }

            ImagePageNoText = ImagePageNoText + 1;

            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }

        public DelegateCommand PreviewButton { get; }
        private void PreviewButtonExecute()
        {
            //// 動画プレビュー更新
            _pagePreviewViewModel.PreviewMovie(MovieLinkText);
            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }

        public DelegateCommand CancelButton { get; }
        private void CancelButtonExecute()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public DelegateCommand SaveButton { get; }
        private void SaveButtonExecute()
        {
            Guard.IsNullOrEmpty(PageNameText, "ページ名称を入力してください。");
            Guard.IsNullOrEmpty(SlideWaitingTimeText, "スライド停止時間を入力してください。");
            var slideWaitingTimeText = Guard.IsFloat(SlideWaitingTimeText.ToString(), "スライド停止時間の入力に誤りがあります。");

            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            if (IsNewPage == true)
            {
                PageIdText = Convert.ToString(_pageMstRepository.GetNextId());
            }

            var entity = CreateEntity();
            _pageMstRepository.Save(entity);

            var dto = new Sample004PageListViewModelInputDto(entity, null);
            var p = new DialogParameters
            {
                { nameof(Sample004PageListViewModelInputDto), dto }
            };

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        public DelegateCommand DeleteButton { get; }
        private void DeleteButtonExecute()
        {
            if (IsNewPage)
            {
                return;
            }

            if (_messageService.Question("削除しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = CreateEntity();
            _pageMstRepository.Delete(entity);

            var dto = new Sample004PageListViewModelInputDto(null, entity);
            var p = new DialogParameters
            {
                { nameof(Sample004PageListViewModelInputDto), dto }
            };

            RequestClose?.Invoke(new DialogResult(ButtonResult.No, p));
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private PageMstEntity CreateEntity()
        {
            return new PageMstEntity(
                    Convert.ToInt32(PageIdText),
                    PageNameText,
                    MovieLinkText,
                    ImageFolderLinkText,
                    ImagePageNoText,
                    (float)SlideWaitingTimeText,
                    NoteEntities[0].Note,
                    NoteEntities[1].Note,
                    NoteEntities[2].Note
                    );
        }

        #endregion
    }
}
