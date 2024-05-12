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
        private readonly Playarea _playArea;
        private readonly FirebaseDatabase _firebaseDatabase;
        private Timer _renderTimer;
        private Timer _movementTimer;
        private const int _renderSpeedInMilliSeconds = 50;
        private readonly int _gameSpeedInMilliSeconds;
        private readonly List<Direction> _directionsBuffer = new List<Direction>();
        private readonly object _directionLock = new object();
        private int _moveCount;
        private Direction _lastDirection = Direction.None;
        private DateTime _lastDirectionChange;
        private readonly int _difficultyLevel;
        private int _slowMotionTicks;
        private bool _highScoreIsWritten;

        private bool IsAllowedToWriteHighScore => !this._highScoreIsWritten && this.NavigationService.EmailIsVerified;
        private bool SlowMotionIsActive =>  !this._slowMotionTicks.Equals(0);
        private string SnakeColor => !this.SlowMotionIsActive ? "#3d9e31" : "#6cf85b";
        private string FoodColor => !this.SlowMotionIsActive ? "#f93910" :"#ff33cc";
        private string PlayAreaColor => "#646464";

        private Direction Direction
        {
            get
            {
                lock (this._directionLock)
                {
                    if (this._directionsBuffer.Count <= 0)
                    {
                        return Direction.None;
                    }
                    Direction o = this._directionsBuffer.First();
                    this._directionsBuffer.Remove(o);
                    this._moveCount++;
                    return o;
                }
            }
            set
            {
                if (value == this._lastDirection)
                {
                    return;
                }

                lock (this._directionLock)
                {
                    if (DateTime.Now.Subtract(this._lastDirectionChange) > TimeSpan.FromMilliseconds(500))
                    {
                        this._directionsBuffer.Clear();
                    }

                    this._lastDirectionChange = DateTime.Now;
                    this._directionsBuffer.Add(value);
                    this._lastDirection = value;
                }
            }
        }

        public PlayAreaViewModel(INavigationService navigationService, Playarea playArea, int difficultyLevel)
        {
            this.NavigationService = navigationService;
            this._firebaseDatabase = new FirebaseDatabase(new FirebaseDatabaseConfig { SessionKey = this.NavigationService.SessionToken });
            this.HighscoreViewModel = new HighscoreViewModel(navigationService, this._firebaseDatabase);
            this.GoToLoginView = new RelayCommand(this.NavigateToLoginView);
            this.GoToMenuView = new RelayCommand(this.NavigateToMenuView);
            this._playArea = playArea;
            this._difficultyLevel = difficultyLevel;
            this._gameSpeedInMilliSeconds = this.CalculateGameSpeed();
            this.InitializePlayarea();
            this.InitializeTimers();
        }

        private void NavigateToLoginView()
        {
            this.NavigationService.NavigateTo(new LoginPageViewModel(this.NavigationService));
        }

        public INavigationService NavigationService { get; set; }

        public int Score => this._playArea.Snake.Body.Count * 100 - 10 * this._moveCount;
        public bool HighscoresAreVisible => !string.IsNullOrEmpty(this.NavigationService.SessionToken);
        public bool ErrorIsVisible => !this.HighscoresAreVisible;
        public int NumberOfRows => this._playArea.Size.NumberOfRows;
        public int NumberOfColumns => this._playArea.Size.NumberOfColumns;

        public bool GameIsOver => this._playArea.CurrentGameState.Equals(Game.Over);
        public bool GameHasStarted => this._playArea.CurrentGameState.Equals(Game.Start);
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
            this._renderTimer = new Timer(this.OnRenderUpdate, null, 0, PlayAreaViewModel._renderSpeedInMilliSeconds);
            this._movementTimer = new Timer(this.OnGameUpdate, null, 0, this._gameSpeedInMilliSeconds);
        }

        private void OnGameUpdate(object newGameState)
        {
            this._playArea.UpdateSnakeDirection(this.Direction);
            this._playArea.MoveSnakeWhenAllowed();

            if (this.SlowMotionIsActive)
            {
                this._slowMotionTicks -= 1;
            }
            else
            {
                this._movementTimer?.Change(this.CalculateGameSpeed(), this.CalculateGameSpeed());
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
                this.ItemsOnPlayArea[this.GetIndexFromPosition(this._playArea.Snake.Head.Position)] = this.SnakeColor;
                this.ItemsOnPlayArea[this.GetIndexFromPosition(this._playArea.Food.Position)] = this.FoodColor;

                foreach (int index in this._playArea.Snake.Body.Select(t => this.GetIndexFromPosition(t.Position)))
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
                    await this._firebaseDatabase.WriteHighScore(newHighScore);
                    this._highScoreIsWritten = true; 
                }

                await this._movementTimer.DisposeAsync();
                await this._renderTimer.DisposeAsync();
            }
        }

        private int CalculateGameSpeed() => 80000 / Math.Max(this._playArea.Size.NumberOfColumns, this._playArea.Size.NumberOfRows) / ((this._difficultyLevel + this._playArea.Snake.Body.Count / 3) * 3);

        private void ActivateSlowMotion()
        {
            if (this.SlowMotionIsActive || !this._playArea.CurrentGameState.Equals(Game.Running))
            {
                return;
            }

            int newGameSpeed = (int)(this.CalculateGameSpeed() * 2.5);
            this._movementTimer?.Change(newGameSpeed, newGameSpeed);
            this._slowMotionTicks = this._difficultyLevel / 2;
            this._moveCount += 20;
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
            this._movementTimer.Dispose();
            this._renderTimer.Dispose();
            this.Direction = Direction.None;
            this._directionsBuffer.Clear();
            this.ItemsOnPlayArea.Clear();
            this._playArea.RestartGame();
            this.InitializePlayarea();
            this.InitializeTimers();
            this._slowMotionTicks = 0;
            this._moveCount = 0;
            this._highScoreIsWritten = false;
        }
    }
}