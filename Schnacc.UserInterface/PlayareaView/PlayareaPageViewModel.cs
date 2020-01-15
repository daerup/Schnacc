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
    using System.Windows;
    using Infrastructure.Navigation;
    using Infrastructure.ViewModels;
    using Domain.Playarea;
    using Infrastructure.Controls;

    public class PlayareaViewModel : ViewModelBase, INavigatableViewModel
    {
        private readonly Playarea playarea;
        private readonly Database.Database database;
        private Timer renderTimer;
        private Timer movementTimer;
        private readonly int renderSpeedInMilliSeconds = 50;
        private readonly int gameSpeedInMilliSeconds;
        private object state;
        private readonly List<Direction> directionsBuffer = new List<Direction>();
        private readonly object directionLock = new object();
        private int moveCount;
        private Direction lastDirection = Direction.None;
        private DateTime lastDirectionChange;
        private readonly int difficultyLevel;
        private int slowMotionTicks = 0;
        private bool highscoreIsWritten = false;

        private bool isAllowedToWriteHighscore => !this.highscoreIsWritten && this.NavigationService.EmailIsVerified;
        private bool SlowMotionIsActive =>  !slowMotionTicks.Equals(0);

        private SolidColorBrush SnakeColor => !this.SlowMotionIsActive ? Brushes.MediumSeaGreen : new SolidColorBrush(Color.FromRgb(57, 255, 20));
        private SolidColorBrush FoodColor => !this.SlowMotionIsActive ? Brushes.IndianRed : new SolidColorBrush(Color.FromRgb(250, 3, 251));
        private SolidColorBrush PlayareaColor => /*Brushes.White;//!this.slowMotionIsActive ? new SolidColorBrush(Color.FromRgb(221,221,221)) :*/ new SolidColorBrush(Color.FromRgb(100, 100, 100));


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

        public PlayareaViewModel(INavigationService navigationService, Playarea playarea, int difficultyLevel)
        {
            this.NavigationService = navigationService;
            this.database = new Database.Database(this.NavigationService.SessionToken);
            this.HighscoreViewModel = new HighscoreViewModel(navigationService, this.database);
            this.GoToLoginView = new RelayCommand(this.NavigateToLoginView);
            this.GoToMenuView = new RelayCommand(this.NavigateToMenuView);
            this.playarea = playarea;
            this.difficultyLevel = difficultyLevel;
            this.gameSpeedInMilliSeconds = this.CalculateGameSpeed();
            this.InizializePlayarea();
            this.InizializeTimers();
        }


        private void NavigateToLoginView()
        {
            this.NavigationService.NavigateTo(new LoginPageViewModel(this.NavigationService));
        }

        public INavigationService NavigationService { get; set; }

        public int Score => (this.playarea.Snake.Body.Count * 100) - (10 * this.moveCount);
        public bool HighscoresAreVisible => !string.IsNullOrEmpty(this.NavigationService.SessionToken);
        public bool ErrorIsVisible => !this.HighscoresAreVisible;
        public int NumberOfRows => this.playarea.Size.NumberOfRows;
        public int NumberOfColumns => this.playarea.Size.NumberOfColumns;

        public bool GameIsOver => this.playarea.CurrentGameState.Equals(Game.Over);
        public bool GameHasStarted => this.playarea.CurrentGameState.Equals(Game.Start);
        public ObservableCollection<SolidColorBrush> ItemsOnPlayarea { get; private set; }
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
                case Key.R: this.Restart(); break;
                default: return;
            }
        }

        private void InizializePlayarea()
        {
            this.ItemsOnPlayarea.Clear();
            List<SolidColorBrush> playareaColors = new List<SolidColorBrush>();
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                for (int j = 0; j < this.NumberOfColumns; j++)
                {
                    playareaColors.Add(this.PlayareaColor);
                }
            }

            this.ItemsOnPlayarea = new ObservableCollection<SolidColorBrush>(playareaColors);
        }

        private void ClearPlayarea()
        {
            for (int i = 0; i < this.ItemsOnPlayarea.Where(iop => iop != this.PlayareaColor).ToList().Count; i++)
            {
                this.ItemsOnPlayarea[i] = this.PlayareaColor;
            }
        }

        private void InizializeTimers()
        {
            this.state = new object();
            this.renderTimer = new Timer(this.OnRenderUpdate, this.state, 0, this.renderSpeedInMilliSeconds);
            this.movementTimer = new Timer(this.OnGameUpdate, this.state, 0, this.gameSpeedInMilliSeconds);
        }

        private void OnGameUpdate(object state)
        {
            this.CheckForGameState();
            this.playarea.UpdateSnakeDirection(this.Direction);
            this.playarea.MoveSnakeWhenAllowed();

            if (this.SlowMotionIsActive)
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
            int indexFromPosition = p.Row * this.NumberOfColumns + p.Column;
            return indexFromPosition;
        }

        private void OnRenderUpdate(object state)
        {
            Application.Current?.Dispatcher?.Invoke(delegate
            {
                this.ClearPlayarea();
                this.ItemsOnPlayarea[GetIndexFromPosition(this.playarea.Snake.Head.Position)] = this.SnakeColor;
                this.ItemsOnPlayarea[GetIndexFromPosition(this.playarea.Food.Position)] = this.FoodColor;

                for (int i = 0; i < this.playarea.Snake.Body.Count; i++)
                {
                    int index = this.GetIndexFromPosition(this.playarea.Snake.Body[i].Position);
                    this.ItemsOnPlayarea[index] = this.SnakeColor;
                }
            });
            this.OnPropertyChanged(nameof(this.Score));
        }


        private void CheckForGameState()
        {
            this.OnPropertyChanged(nameof(this.GameHasStarted));
            this.OnPropertyChanged(nameof(this.GameIsOver));

            if (this.GameIsOver && this.HighscoresAreVisible)
            {
                if (isAllowedToWriteHighscore)
                {
                    Highscore newHighscore = new Highscore(this.NavigationService.Username, this.Score);
                    this.database.WriteHighscore(newHighscore);
                    this.highscoreIsWritten = true; 
                }
            }
        }

        private int CalculateGameSpeed()
        {
            return 80000 / Math.Max(this.playarea.Size.NumberOfColumns, this.playarea.Size.NumberOfRows) / ((this.difficultyLevel + (this.playarea.Snake.Body.Count / 3)) * 3);
        }

        private void ActivateSlowMotion()
        {
            if (this.SlowMotionIsActive == false && this.playarea.CurrentGameState.Equals(Game.Running))
            {
                int newGameSpeed = (int)(this.CalculateGameSpeed() * 2.5);
                this.movementTimer?.Change(newGameSpeed, newGameSpeed);
                this.slowMotionTicks = this.difficultyLevel / 2;
                this.moveCount += 20; 
            }
        }

        private void NavigateToMenuView()
        {
            if (string.IsNullOrEmpty(this.NavigationService.SessionToken))
            {
                this.NavigationService.NavigateTo(new HomeMenuPageViewModel(this.NavigationService));
            }
            else
            {
                this.NavigationService.NavigateTo(new LoginSuccessfulPageMenuViewModel(this.NavigationService));
            }
        }

        private void Restart()
        {
            this.playarea.RestartGame();
            this.InizializePlayarea();
            this.InizializeTimers();
            this.slowMotionTicks = 0;
            this.moveCount = 0;
            this.highscoreIsWritten = false;
        }
    }
}