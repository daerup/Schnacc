using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HighscoreView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.LoginView;

namespace Schnacc.UserInterface.PlayareaView
{
    using Infrastructure.Navigation;
    using Infrastructure.ViewModels;
    using Domain.Playarea;

    public class PlayareaViewModel : ViewModelBase, INavigatableViewModel
    {
        private readonly Playarea playarea;
        private Timer renderTimer;
        private Timer movementTimer;
        private readonly int renderSpeedInMilliSeconds = 100;
        private readonly int gameSpeedInMilliSeconds = 300;
        private object State;
        private List<Direction> directionsBuffer = new List<Direction>();
        private object directionLock = new object();
        private int moveCount;
        private Direction lastDirection = Direction.None;
        private DateTime lastDirectionChange;


        private Direction Direction
        {
            get
            {
                lock (this.directionLock)
                {
                    if (this.directionsBuffer.Count <= 0)
                    {
                        return Direction.None;
                    }
                    Direction o = this.directionsBuffer.First();
                    this.directionsBuffer.Remove(o);
                    this.moveCount++;
                    return o;
                }
            }
            set
            {
                if (value == this.lastDirection)
                {
                    return;
                }

                lock (this.directionLock)
                {
                    if (DateTime.Now.Subtract(this.lastDirectionChange) > TimeSpan.FromMilliseconds(500))
                    {
                        this.directionsBuffer.Clear();
                    }
                    this.lastDirectionChange = DateTime.Now;
                    this.directionsBuffer.Add(value);
                    this.lastDirection = value;
                }
            }
        }

        public PlayareaViewModel(INavigationService navigationService, Playarea playarea)
        {
            this.navigationService = navigationService;
            this.HighscoreViewModel = new HighscoreViewModel(navigationService);
            this.GoToLoginView = new RelayCommand(() => this.navigationService.NavigateTo(new LoginViewModel(this.navigationService)));
            this.playarea = playarea;
            this.itemsOnPlayarea = new ObservableCollection<SolidColorBrush>();
            this.InizializePlayarea();
            this.InizializeTimer();
        }

        public INavigationService navigationService { get; set; }

        public int Score => (this.playarea.Snake.Body.Count * 100) - (10 * this.moveCount);
        public bool HighscoresVisible => !string.IsNullOrEmpty(this.navigationService.SessionToken);
        public int NumberOfRows => this.playarea.Size.NumberOfRows;
        public int NumberOfColumns => this.playarea.Size.NumberOfColumns;
        public Position HeadPosition => this.playarea.Snake.Head.Position;
        public Position FoodPosition => this.playarea.Food.Position;

        public ObservableCollection<SolidColorBrush> itemsOnPlayarea { get; private set; }

        public HighscoreViewModel HighscoreViewModel { get; }

        public RelayCommand GoToLoginView { get; }

        private void InizializePlayarea()
        {
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                for (int j = 0; j < this.NumberOfColumns; j++)
                {
                    this.itemsOnPlayarea.Add(Brushes.White);
                }
            }
        }

        private void ClearPlayarea()
        {
            for (var i = 0; i < this.itemsOnPlayarea.Count; i++)
            {
                this.itemsOnPlayarea[i] = Brushes.White;
            }
        }

        private void InizializeTimer()
        {
            this.State = new object();
            this.renderTimer = new Timer(this.OnRenderUpdate, this.State, 0, this.renderSpeedInMilliSeconds);
            this.movementTimer = new Timer(this.OnGameUpdate, this.State, 0, this.gameSpeedInMilliSeconds);
        }

        private void OnGameUpdate(object? state)
        {
            this.playarea.UpdateSnakeDirection(this.Direction);
            this.playarea.MoveSnakeWhenAllowed();
            this.CheckForGameOver();
        }

        private int GetIndexFromPosition(Position p)
        {
            return p.Row * this.NumberOfColumns + p.Column;
        }

        private void OnRenderUpdate(object? state)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                this.ClearPlayarea();
                this.itemsOnPlayarea[GetIndexFromPosition(this.HeadPosition)] = Brushes.Green;
                this.itemsOnPlayarea[GetIndexFromPosition(this.FoodPosition)] = Brushes.Red;

                for (int i = 0; i < this.playarea.Snake.Body.Count; i++)
                {
                    int index = this.GetIndexFromPosition(this.playarea.Snake.Body[i].Position);
                    this.itemsOnPlayarea[index] = Brushes.Green;
                }
            });
            this.OnPropertyChanged(nameof(this.Score));
        }


        private void CheckForGameOver()
        {
            if (this.playarea.CurrentGameState == Game.Over)
            {
                int s = 0;
            }
        }

        public void UpdateSnakeDirectionTo(object sender, KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Right: this.Direction = Direction.Right; break;
                case Key.Left: this.Direction = Direction.Left; break;
                case Key.Up: this.Direction = Direction.Up; break;
                case Key.Down: this.Direction = Direction.Down; break;
                default: return;
            }
        }
    }
}