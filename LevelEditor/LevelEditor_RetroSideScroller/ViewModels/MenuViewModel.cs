using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Commands;
using ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels;
using System.Windows;
using ProjetJeuxVideo_LevelEditor_Metroidvania.Views;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    class MenuViewModel : INotifyPropertyChanged
    {
        public ICommand commandNewLevel { get; set; }
        public ICommand commandLoad { get; set; }
        public ICommand commandSave { get; set; }
        public ICommand commandSaveAs { get; set; }
        public ICommand commandQuit { get; set; }
        public ICommand commandLevelSize { get; set; }
        public ICommand commandLevelTileset { get; set; }
        public ICommand commandLevelAnimations { get; set; }

        private MainViewModel mvm;
        private bool aLevelIsLoaded;
        public bool ALevelIsLoaded
        {
            get
            {
                return aLevelIsLoaded;
            }
            set
            {
                aLevelIsLoaded = value;
                OnPropertyChanged("ALevelIsLoaded");
            }
        }
        private bool canSave;
        public bool CanSave
        {
            get
            {
                return canSave;
            }
            set
            {
                canSave = value;
                OnPropertyChanged("CanSave");
            }
        }
        public MenuViewModel(MainViewModel mvm)
        {
            this.mvm = mvm;
            commandNewLevel = new BaseCommand(GenerateNewLevel, obj => true);
            commandLoad = new BaseCommand(LoadFromFile, obj => true);
            commandSave = new BaseCommand(Save, obj => true);
            commandSaveAs = new BaseCommand(SaveAs, obj => true);
            commandQuit = new BaseCommand(Quit, obj => true);
            commandLevelSize = new BaseCommand(ChangeLevelSize, obj => true);
            commandLevelTileset = new BaseCommand(ManageLevelTileset, obj => true);
            commandLevelAnimations = new BaseCommand(ManageAnimations, obj => true);
            CanSave = false;
            ALevelIsLoaded = false;
        }

        private void GenerateNewLevel(object obj)
        {
            bool confirm = true;
            if (mvm.FileModified)
            {
                MessageBoxResult result = MessageBox.Show("Save current project before loading?", "New level", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Save(false);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        confirm = false;
                        break;
                }
            }

            if (confirm)
            {
                mvm.CurrentControl = new InitialSettingViewModel(mvm);
            }
        }

        private void LoadFromFile(object obj)
        {
            bool confirm = true;
            if (mvm.FileModified)
            {
                MessageBoxResult result = MessageBox.Show("Save current project before loading?", "Load", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Save(false);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        confirm = false;
                        break;
                }
            }

            if (confirm)
            {
                mvm.Load();
            }
        }

        private void Save(object obj)
        {
            Save(true);
        }
        private void Save(bool overriteCurrent)
        {
            mvm.Save(overriteCurrent);
        }

        private void SaveAs(object obj)
        {
            Save(false);
        }

        private void Quit(object obj)
        {
            throw new NotImplementedException();
        }

        private void ChangeLevelSize(object obj)
        {
            ChangeLevelSizeWindow clsvm;
            clsvm = new ChangeLevelSizeWindow((LevelViewModel)(((EditorViewModel)mvm.CurrentControl).LevelControl));
            clsvm.Show();
        }

        private void ManageLevelTileset(object param)
        {
            ChangeLevelTilesetWindow cltw;
            cltw = new ChangeLevelTilesetWindow((TilesetViewModel)((EditorViewModel)mvm.CurrentControl).TilesetControl);
            cltw.Show();
        }

        private void ManageAnimations(object param)
        {
            AnimationMakerWindow amw;
            amw = new AnimationMakerWindow((EditorViewModel)mvm.CurrentControl);
            //AnimationManagerWindow amw;
            //amw = new AnimationManagerWindow((EditorViewModel)mvm.CurrentControl);
            amw.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
