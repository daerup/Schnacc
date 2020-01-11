using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Input;

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

        public int NumberOfRows => this.playarea.Size.NumberOfRows;
        public int NumberOfColumns => this.playarea.Size.NumberOfColumns;
        public Position HeadPosition => this.playarea.Snake.Head.Position;
        public ObservableCollection<Position> BodyPositions => new ObservableCollection<Position>(this.playarea.Snake.Body.Select(bp => bp.Position));
        public Position FoodPosition => this.playarea.Food.Position;
        public AsyncRelayCommand<KeyEventArgs> UpdateSnakeDirection { get; }

        private void InizializeTimer()
        {
            this.State = new object();
            this.renderTimer = new Timer(this.Render, this.State, 0, this.renderSpeedInMilliSeconds);
            this.movementTimer = new Timer(this.Move, this.State, 0, this.gameSpeedInMilliSeconds);
        }

        private async void Move(object? state)
        {
            this.playarea.MoveSnakeWhenAllowed();

        }

        private async void Render(object? state)
        {
            this.OnPropertyChanged(nameof(this.HeadPosition));
            this.OnPropertyChanged(nameof(this.BodyPositions));
            this.OnPropertyChanged(nameof(this.FoodPosition));
        }

        public async void UpdateSnakeDirectionTo(object sener, KeyEventArgs args)
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