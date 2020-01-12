using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using Schnacc.Domain.Snake;
using Schnacc.UserInterface.Infrastructure.Commands;

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


        public PlayareaViewModel(INavigationService navigationService, Playarea playarea)
        {
            //this.UpdateSnakeDirection = new RelayCommand<KeyEventArgs>(this.UpdateSnakeDirectionTo);
            this.navigationService = navigationService;
            this.playarea = playarea;
            this.InizializeTimer();
        }

        public INavigationService navigationService { get; set; }

        public int Score { get; set; }
        public int NumberOfRows => this.playarea.Size.NumberOfRows;
        public int NumberOfColumns => this.playarea.Size.NumberOfColumns;
        public Position HeadPosition => this.playarea.Snake.Head.Position;
        public ObservableCollection<Position> BodyPositions => new ObservableCollection<Position>(this.playarea.Snake.Body.Select(bp => bp.Position));
        public Position FoodPosition => this.playarea.Food.Position;
        public AsyncRelayCommand<KeyEventArgs> UpdateSnakeDirection { get; }

        public ObservableCollection<SolidColorBrush> items
        {
            get => fillPlayarea();
        }

        private ObservableCollection<SolidColorBrush> fillPlayarea()
        {
            ObservableCollection<SolidColorBrush> colors = new ObservableCollection<SolidColorBrush>();
            for (int i = 0; i < this.NumberOfRows; i++)
            {
                for (int j = 0; j < this.NumberOfColumns; j++)
                {
                    Position p = new Position(i, j);

                    colors.Add(this.positionToBrush(p));
                }
            }

            return colors;
        }

        private SolidColorBrush positionToBrush(Position position)
        {
            if (this.playarea.Snake.Head.Position.Equals(position) || this.playarea.Snake.Body.Select(bp => bp.Position).Any(p => p.Equals(position)))
            {
                return Brushes.Green;
            }else if (this.playarea.Food.Position.Equals(position))
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.White;
            }
        }

        private void InizializeTimer()
        {
            this.State = new object();
            this.renderTimer = new Timer(this.Render, this.State, 0, this.renderSpeedInMilliSeconds);
            this.movementTimer = new Timer(this.Move, this.State, 0, this.gameSpeedInMilliSeconds);
        }

        private void Move(object? state)
        {
            this.playarea.MoveSnakeWhenAllowed();

        }

        private async void Render(object? state)
        {
            this.OnPropertyChanged(nameof(this.HeadPosition));
            this.OnPropertyChanged(nameof(this.BodyPositions));
            this.OnPropertyChanged(nameof(this.FoodPosition));
            this.CheckForGameOver();
            fillPlayarea();
        }

        private void CheckForGameOver()
        {
            if (this.playarea.CurrentGameState == Game.Over && this.navigationService.EmailIsVerified)
            {
                int s = 0;
            }
        }

        public void UpdateSnakeDirectionTo(KeyEventArgs args)
        {
            Direction newDirection;
            switch (args.Key)
            {
                case Key.Right: newDirection = Direction.Right; break;
                case Key.Left: newDirection = Direction.Left; break;
                case Key.Up: newDirection = Direction.Up; break;
                case Key.Down: newDirection = Direction.Down; break;
                default: return;
            }

            this.playarea.UpdateSnakeDirection(newDirection);
        }

        private void writeHighscore()
        {
        }

        public void UpdateSnakeDirectionTo(object sender, KeyEventArgs args)
        {
            Direction newDirection;
            switch (args.Key)
            {
                case Key.Right: newDirection = Direction.Right; break;
                case Key.Left: newDirection = Direction.Left; break;
                case Key.Up: newDirection = Direction.Up; break;
                case Key.Down: newDirection = Direction.Down; break;
                default: return;
            }

            this.playarea.UpdateSnakeDirection(newDirection);
        }
    }
}