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
using System.Windows.Media;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.ViewModels
{
    class ToolbarViewModel : INotifyPropertyChanged
    {
        private MainViewModel mvm;
        private Brush drawBGColor;
        private Brush fillBGColor;
        private Brush rectBGColor;
        private Brush backBGColor;
        private Brush defaultBGColor;
        private Brush frontBGColor;

        public Brush DefaultBGColor
        {
            get
            {
                return defaultBGColor;
            }
            set
            {
                defaultBGColor = value;
                OnPropertyChanged("DefaultBGColor");
            }

        }
        public Brush BackBGColor
        {
            get
            {
                return backBGColor;
            }
            set
            {
                backBGColor = value;
                OnPropertyChanged("BackBGColor");
            }

        }
        public Brush FrontBGColor
        {
            get
            {
                return frontBGColor;
            }
            set
            {
                frontBGColor = value;
                OnPropertyChanged("FrontBGColor");
            }

        }
        public Brush DrawBGColor
        {
            get
            {
                return drawBGColor;
            }
            set
            {
                drawBGColor = value;
                OnPropertyChanged("DrawBGColor");
            }

        }
        public Brush FillBGColor
        {
            get
            {
                return fillBGColor;
            }
            set
            {
                fillBGColor = value;
                OnPropertyChanged("FillBGColor");
            }
        }
        public Brush RectBGColor
        {
            get
            {
                return rectBGColor;
            }

            set
            {
                rectBGColor = value;
                OnPropertyChanged("RectBGColor");
            }
        }

        public ICommand ClickDrawCommand { get; set; }
        public ICommand ClickFillCommand { get; set; }
        public ICommand ClickRectCommand { get; set; }

        public ICommand ClickBackCommand { get; set; }
        public ICommand ClickDefaultCommand { get; set; }
        public ICommand ClickFrontCommand { get; set; }

        public ToolbarViewModel(MainViewModel mvm)
        {
            this.mvm = mvm;

            mvm.SelectedTool = Tool.DRAW;
            DrawBGColor = new SolidColorBrush(Colors.White);
            FillBGColor = new SolidColorBrush(Colors.LightGray);
            RectBGColor = new SolidColorBrush(Colors.LightGray);

            DefaultBGColor = new SolidColorBrush(Colors.White);
            BackBGColor = new SolidColorBrush(Colors.LightGray);
            FrontBGColor = new SolidColorBrush(Colors.LightGray);

            ClickDrawCommand = new BaseCommand(OnClickDraw, obj => true);
            ClickFillCommand = new BaseCommand(OnClickFill, obj => true);
            ClickRectCommand = new BaseCommand(OnClickRect, obj => true);

            ClickBackCommand = new BaseCommand(OnClickBack, obj => true);
            ClickDefaultCommand = new BaseCommand(OnClickDefault, obj => true);
            ClickFrontCommand = new BaseCommand(OnClickFront, obj => true);
        }

        private void OnClickDraw(object param)
        {
            mvm.SelectedTool = Tool.DRAW;
            DrawBGColor = new SolidColorBrush(Colors.White);
            FillBGColor = new SolidColorBrush(Colors.LightGray);
            RectBGColor = new SolidColorBrush(Colors.LightGray);
        }

        private void OnClickFill(object param)
        {
            mvm.SelectedTool = Tool.FILL;
            DrawBGColor = new SolidColorBrush(Colors.LightGray);
            FillBGColor = new SolidColorBrush(Colors.White);
            RectBGColor = new SolidColorBrush(Colors.LightGray);
        }

        private void OnClickRect(object param)
        {
            mvm.SelectedTool = Tool.RECT;
            DrawBGColor = new SolidColorBrush(Colors.LightGray);
            FillBGColor = new SolidColorBrush(Colors.LightGray);
            RectBGColor = new SolidColorBrush(Colors.White);
        }

        private void OnClickBack(object param)
        {
            mvm.SelectedLayer = Layer.BACK;
            BackBGColor = new SolidColorBrush(Colors.White);
            DefaultBGColor = new SolidColorBrush(Colors.LightGray);
            FrontBGColor = new SolidColorBrush(Colors.LightGray);
        }
        private void OnClickDefault(object param)
        {
            mvm.SelectedLayer = Layer.DEFAULT;
            BackBGColor = new SolidColorBrush(Colors.LightGray);
            DefaultBGColor = new SolidColorBrush(Colors.White);
            FrontBGColor = new SolidColorBrush(Colors.LightGray);
        }
        private void OnClickFront(object param)
        {
            mvm.SelectedLayer = Layer.FRONT;
            BackBGColor = new SolidColorBrush(Colors.LightGray);
            DefaultBGColor = new SolidColorBrush(Colors.LightGray);
            FrontBGColor = new SolidColorBrush(Colors.White);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}