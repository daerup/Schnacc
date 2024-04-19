using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Schnacc.Database;
using Schnacc.Domain.Playarea;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.HighScoreView;
using Schnacc.UserInterface.HomeMenuView;
using Schnacc.UserInterface.Infrastructure.Commands;
using Schnacc.UserInterface.Infrastructure.Extensions;
using Schnacc.UserInterface.Infrastructure.Navigation;
using Schnacc.UserInterface.Infrastructure.ViewModels;
using Schnacc.UserInterface.LoginView;

namespace Schnacc.UserInterface.PlayAreaView
{
    public class PlayAreaViewModel : ViewModelBase, INavigableViewModel
    {
        private readonly Playarea playArea;
        private readonly Database.Database database;
        private Timer renderTimer;
        private Timer movementTimer;
        private const int RenderSpeedInMilliSeconds = 50;
        private readonly int gameSpeedInMilliSeconds;
        private readonly List<Direction> directionsBuffer = new List<Direction>();
        private readonly object directionLock = new object();
        private int moveCount;
        private Direction lastDirection = Direction.None;
        private DateTime lastDirectionChange;
        private readonly int difficultyLevel;
        private int slowMotionTicks;
        private bool highScoreIsWritten;

        private bool IsAllowedToWriteHighScore => !this.highScoreIsWritten && this.NavigationService.EmailIsVerified;
        private bool SlowMotionIsActive =>  !this.slowMotionTicks.Equals(0);
        private string SnakeColor => !this.SlowMotionIsActive ? "#3d9e31" : "#6cf85b";
        private string FoodColor => !this.SlowMotionIsActive ? "#f93910" :"#ff33cc";
        private string PlayAreaColor => "#646464";

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

        public PlayAreaViewModel(INavigationService navigationService, Playarea playArea, int difficultyLevel)
        {
            this.NavigationService = navigationService;
            this.database = new Database.Database(this.NavigationService.SessionToken);
            this.HighscoreViewModel = new HighscoreViewModel(navigationService, this.database);
            this.GoToLoginView = new RelayCommand(this.NavigateToLoginView);
            this.GoToMenuView = new RelayCommand(this.NavigateToMenuView);
            this.playArea = playArea;
            this.difficultyLevel = difficultyLevel;
            this.gameSpeedInMilliSeconds = this.CalculateGameSpeed();
            this.InitializePlayarea();
            this.InitializeTimers();
        }

        private void NavigateToLoginView()
        {
            this.NavigationService.NavigateTo(new LoginPageViewModel(this.NavigationService));
        }

        public INavigationService NavigationService { get; set; }

        public int Score => this.playArea.Snake.Body.Count * 100 - 10 * this.moveCount;
        public bool HighscoresAreVisible => !string.IsNullOrEmpty(this.NavigationService.SessionToken);
        public bool ErrorIsVisible => !this.HighscoresAreVisible;
        public int NumberOfRows => this.playArea.Size.NumberOfRows;
        public int NumberOfColumns => this.playArea.Size.NumberOfColumns;

        public bool GameIsOver => this.playArea.CurrentGameState.Equals(Game.Over);
        public bool GameHasStarted => this.playArea.CurrentGameState.Equals(Game.Start);
        public SuppressableObservableCollection<string> ItemsOnPlayArea { get; private set; }
        public HighscoreViewModel HighscoreViewModel { get; }
        public RelayCommand GoToLoginView { get; }
        public RelayCommand GoToMenuView { get; }

        public void UpdateSnakeDirectionTo(object sender, KeyEventArgs args)
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (args.Key)
            {
                case Key.Right: this.SetDirectionTo(Direction.Right); break;
                case Key.Left: this.SetDirectionTo(Direction.Left); break;
                case Key.Up: this.SetDirectionTo(Direction.Up); break;
                case Key.Down: this.SetDirectionTo(Direction.Down); break;
                case Key.Space: this.ActivateSlowMotion(); break;
                case Key.R: this.Restart(); break;
            }
        }

        private void SetDirectionTo(Direction newDirection)
        {
            if (!this.GameIsOver)
            {
                this.Direction = newDirection; 
            }
        }

        private void InitializePlayarea()
        {
            var playAreaColors = new List<string>();
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                for (int j = 0; j < this.NumberOfColumns; j++)
                {
                    playAreaColors.Add(this.PlayAreaColor);
                }
            }

            this.ItemsOnPlayArea = new SuppressableObservableCollection<string>(playAreaColors);
        }

        private void ClearPlayArea()
        {
            for (int i = 0; i < this.ItemsOnPlayArea.Count; i++)
            {
                this.ItemsOnPlayArea[i] = this.PlayAreaColor;
            }
        }

        private void InitializeTimers()
        {
            this.renderTimer = new Timer(this.OnRenderUpdate, null, 0, PlayAreaViewModel.RenderSpeedInMilliSeconds);
            this.movementTimer = new Timer(this.OnGameUpdate, null, 0, this.gameSpeedInMilliSeconds);
        }

        private void OnGameUpdate(object newGameState)
        {
            this.playArea.UpdateSnakeDirection(this.Direction);
            this.playArea.MoveSnakeWhenAllowed();

            if (this.SlowMotionIsActive)
            {
                this.slowMotionTicks -= 1;
            }
            else
            {
                this.movementTimer?.Change(this.CalculateGameSpeed(), this.CalculateGameSpeed());
            }
            this.CheckForGameState().GetAwaiter().GetResult();
        }

        private int GetIndexFromPosition(Position p)
        {
            int indexFromPosition = p.Row * this.NumberOfColumns + p.Column;
            return indexFromPosition;
        }

        private void OnRenderUpdate(object newGameState)
        {
            this.OnPropertyChanged(nameof(this.GameHasStarted));
            this.OnPropertyChanged(nameof(this.GameIsOver));
            this.OnPropertyChanged(nameof(this.Score));
            Application.Current?.Dispatcher?.Invoke(delegate
            {
                this.ItemsOnPlayArea.SuppressNotification = true;
                this.ClearPlayArea();
                this.ItemsOnPlayArea[this.GetIndexFromPosition(this.playArea.Snake.Head.Position)] = this.SnakeColor;
                this.ItemsOnPlayArea[this.GetIndexFromPosition(this.playArea.Food.Position)] = this.FoodColor;

                foreach (int index in this.playArea.Snake.Body.Select(t => this.GetIndexFromPosition(t.Position)))
                {
                    this.ItemsOnPlayArea[index] = this.SnakeColor;
                }
                
                this.ItemsOnPlayArea.SuppressNotification = false;
            });
        }

        private async Task CheckForGameState()
        {
            if (this.GameIsOver && this.HighscoresAreVisible)
            {
                if (this.IsAllowedToWriteHighScore)
                {
                    var newHighScore = new Highscore(this.NavigationService.Username, this.Score);
                    await this.database.WriteHighScore(newHighScore);
                    this.highScoreIsWritten = true; 
                }

                await this.movementTimer.DisposeAsync();
                await this.renderTimer.DisposeAsync();
            }
        }

        private int CalculateGameSpeed() => 80000 / Math.Max(this.playArea.Size.NumberOfColumns, this.playArea.Size.NumberOfRows) / ((this.difficultyLevel + this.playArea.Snake.Body.Count / 3) * 3);

        private void ActivateSlowMotion()
        {
            if (this.SlowMotionIsActive || !this.playArea.CurrentGameState.Equals(Game.Running))
            {
                return;
            }

            int newGameSpeed = (int)(this.CalculateGameSpeed() * 2.5);
            this.movementTimer?.Change(newGameSpeed, newGameSpeed);
            this.slowMotionTicks = this.difficultyLevel / 2;
            this.moveCount += 20;
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
            this.movementTimer.Dispose();
            this.renderTimer.Dispose();
            this.Direction = Direction.None;
            this.directionsBuffer.Clear();
            this.ItemsOnPlayArea.Clear();
            this.playArea.RestartGame();
            this.InitializePlayarea();
            this.InitializeTimers();
            this.slowMotionTicks = 0;
            this.moveCount = 0;
            this.highScoreIsWritten = false;
        }
    }
}