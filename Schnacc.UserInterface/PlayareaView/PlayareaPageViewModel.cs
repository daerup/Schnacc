using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using Schnacc.Database;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HighscoreView;
using Schnacc.UserInterface.HomeMenuView;
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
        private readonly Database.Database database;
        private Timer renderTimer;
        private Timer movementTimer;
        private readonly int renderSpeedInMilliSeconds = 50;
        private readonly int gameSpeedInMilliSeconds;
        private object State;
        private List<Direction> directionsBuffer = new List<Direction>();
        private object directionLock = new object();
        private int moveCount;
        private Direction lastDirection = Direction.None;
        private DateTime lastDirectionChange;
        private int slowMotionTicks = 0;
        private bool slowMotionIsActive =>  !slowMotionTicks.Equals(0);


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
            this.database = new Database.Database(this.navigationService.SessionToken);
            this.HighscoreViewModel = new HighscoreViewModel(navigationService, this.database);
            this.GoToLoginView = new RelayCommand(this.NavigateToLoginView);
            this.GoToMenuView = new RelayCommand(this.NavigateToMenuView);
            this.playarea = playarea;
            this.gameSpeedInMilliSeconds = this.CalculateGameSpeed();
            this.itemsOnPlayarea = new ObservableCollection<SolidColorBrush>();
            this.InizializePlayarea();
            this.InizializeTimers();
        }

        private void NavigateToLoginView()
        {
            this.navigationService.NavigateTo(new LoginPageViewModel(this.navigationService));
        }

        public INavigationService navigationService { get; set; }

        public int Score => (this.playarea.Snake.Body.Count * 100) - (10 * this.moveCount);
        public bool HighscoresAreVisible => !string.IsNullOrEmpty(this.navigationService.SessionToken);
        public int NumberOfRows => this.playarea.Size.NumberOfRows;
        public int NumberOfColumns => this.playarea.Size.NumberOfColumns;
        public Position HeadPosition => this.playarea.Snake.Head.Position;
        public Position FoodPosition => this.playarea.Food.Position;

        public ObservableCollection<SolidColorBrush> itemsOnPlayarea { get; private set; }

        public HighscoreViewModel HighscoreViewModel { get; }

        public RelayCommand GoToLoginView { get; }
        public RelayCommand GoToMenuView { get; }

        public void UpdateSnakeDirectionTo(object sender, KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Right: this.Direction = Direction.Right; break;
                case Key.Left: this.Direction = Direction.Left; break;
                case Key.Up: this.Direction = Direction.Up; break;
                case Key.Down: this.Direction = Direction.Down; break;
                case Key.Space: this.ActivateSlowMotion(); break;
                default: return;
            }
        }

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

        private void InizializeTimers()
        {
            this.State = new object();
            this.renderTimer = new Timer(this.OnRenderUpdate, this.State, 0, this.renderSpeedInMilliSeconds);
            this.movementTimer = new Timer(this.OnGameUpdate, this.State, 0, this.gameSpeedInMilliSeconds);
        }

        private void OnGameUpdate(object? state)
        {
            this.CheckForGameOver();
            this.playarea.UpdateSnakeDirection(this.Direction);
            this.playarea.MoveSnakeWhenAllowed();

            if (this.slowMotionTicks.Equals(0) == false)
            {
                this.slowMotionTicks -= 1;
            }
            else
            {
                this.movementTimer?.Change(this.CalculateGameSpeed(), this.CalculateGameSpeed());
            }
        }

        private int GetIndexFromPosition(Position p)
        {
            return p.Row * this.NumberOfColumns + p.Column;
        }

        private void OnRenderUpdate(object? state)
        {
            App.Current?.Dispatcher.Invoke(delegate
            {
                this.ClearPlayarea();
                this.itemsOnPlayarea[GetIndexFromPosition(this.HeadPosition)] = !this.slowMotionIsActive ? Brushes.Green : Brushes.Gray;
                this.itemsOnPlayarea[GetIndexFromPosition(this.FoodPosition)] = !this.slowMotionIsActive ? Brushes.Red : Brushes.DarkSlateGray;

                for (int i = 0; i < this.playarea.Snake.Body.Count; i++)
                {
                    int index = this.GetIndexFromPosition(this.playarea.Snake.Body[i].Position);
                    this.itemsOnPlayarea[index] = !this.slowMotionIsActive ? Brushes.Green : Brushes.Gray;
                }
            });
            this.OnPropertyChanged(nameof(this.Score));
        }


        private void CheckForGameOver()
        {
            if (this.playarea.CurrentGameState == Game.Over && this.navigationService.EmailIsVerified)
            {
                Highscore newHighscore = new Highscore(this.navigationService.Username, this.Score);
                this.database.WriteHighscore(newHighscore);
                this.movementTimer.Dispose();
            }
        }

        private int CalculateGameSpeed()
        {
            return 80000 / Math.Max(this.playarea.Size.NumberOfColumns, this.playarea.Size.NumberOfRows) / ((7 + (this.playarea.Snake.Body.Count / 3)) * 3);
        }

        private void ActivateSlowMotion()
        {
            if (this.slowMotionIsActive == false && this.playarea.CurrentGameState.Equals(Game.Running))
            {
                this.movementTimer?.Change(0, (int)(this.CalculateGameSpeed() * 2.5));
                this.slowMotionTicks = 5;
                this.moveCount += 20; 
            }
        }

        private void NavigateToMenuView()
        {
            if (string.IsNullOrEmpty(this.navigationService.SessionToken))
            {
                this.navigationService.NavigateTo(new HomeMenuPageViewModel(this.navigationService));
            }
            else
            {
                this.navigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.navigationService));
            }
        }
    }
}