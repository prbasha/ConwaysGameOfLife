using ConwaysGameOfLife.Common;
using ConwaysGameOfLife.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConwaysGameOfLife.ViewModel
{
    /// <summary>
    /// The ConwaysGameOfLifeViewModel class represents the View Model for Conway's Game Of Life.
    /// </summary>
    public class ConwaysGameOfLifeViewModel
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConwaysGameOfLifeViewModel()
        {
            try
            {
                GameOfLife = new GameOfLife();  // Initialise the model class.
                GameOfLife.PropertyChanged += OnGameOfLifePropertyChanged;

                // Initialise commands.
                StartGameCommand = new DelegateCommand(OnStartGame, CanStartGame);
                StopGameCommand = new DelegateCommand(OnStopGame, CanStopGame);
                StepGameCommand = new DelegateCommand(OnStepGame, CanStepGame);
                ResetGameCommand = new DelegateCommand(OnResetGame);
                ToggleCellStateCommand = new DelegateCommand(OnToggleCellState, CanToggleCellState);
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel(): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets the GameOfLife model class.
        /// </summary>
        public GameOfLife GameOfLife { get; }

        /// <summary>
        /// Gets or sets the start game command.
        /// </summary>
        public DelegateCommand StartGameCommand { get; private set; }

        /// <summary>
        /// Gets or sets the stop game command.
        /// </summary>
        public DelegateCommand StopGameCommand { get; private set; }

        /// <summary>
        /// Gets or sets the step game command.
        /// </summary>
        public DelegateCommand StepGameCommand { get; private set; }

        /// <summary>
        /// Gets or sets the reset game command.
        /// </summary>
        public DelegateCommand ResetGameCommand { get; private set; }

        /// <summary>
        /// Gets or sets the toggle cell state command.
        /// </summary>
        public DelegateCommand ToggleCellStateCommand { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// The OnStartGame method is called to start the game.
        /// </summary>
        /// <param name="arg"></param>
        public void OnStartGame(object arg)
        {
            try
            {
                if (GameOfLife != null)
                {
                    GameOfLife.StartGame();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel.OnStartGame(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanStartGame method is callled to determine if the game can start.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanStartGame(object arg)
        {
            return GameOfLife != null && !GameOfLife.IsGameRunning;
        }

        /// <summary>
        /// The OnStopGame method is called to stop the game.
        /// </summary>
        /// <param name="arg"></param>
        public void OnStopGame(object arg)
        {
            try
            {
                if (GameOfLife != null)
                {
                    GameOfLife.StopGame();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel.OnStopGame(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanStopGame method is callled to determine if the game can stop.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanStopGame(object arg)
        {
            return GameOfLife != null && GameOfLife.IsGameRunning;
        }

        /// <summary>
        /// The OnStepGame method is called to step the game.
        /// </summary>
        /// <param name="arg"></param>
        public async void OnStepGame(object arg)
        {
            try
            {
                if (GameOfLife != null)
                {
                    await GameOfLife.StepGame();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel.OnStepGame(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanStepGame method is callled to determine if the game can step.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanStepGame(object arg)
        {
            return GameOfLife != null && !GameOfLife.IsGameRunning;
        }

        /// <summary>
        /// The OnResetGame method is called to reset the game.
        /// </summary>
        /// <param name="arg"></param>
        public void OnResetGame(object arg)
        {
            try
            {
                if (GameOfLife != null)
                {
                    GameOfLife.ResetGame();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel.OnResetGame(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The OnToggleCellState method is toggle the state of a cell at the left mouse button click position.
        /// </summary>
        /// <param name="arg"></param>
        public void OnToggleCellState(object arg)
        {
            try
            {
                if (GameOfLife != null)
                {
                    Point mousePoint = Mouse.GetPosition((IInputElement)arg);   // Get the mouse position (in pixels).
                    double gridWidthPixels = ((ItemsControl)arg).ActualWidth;   // Get the grid width (in pixels).
                    double gridHeightPixels = ((ItemsControl)arg).ActualHeight; // Get the grid height (in pixels).

                    GameOfLife.ToggleCellState(mousePoint, gridWidthPixels, gridHeightPixels);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel.OnToggleCellState(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanToggleCellState method is callled to determine if a fire can be started.
        /// </summary>
        public bool CanToggleCellState(object arg)
        {
            return GameOfLife != null && !GameOfLife.IsGameRunning;
        }

        /// <summary>
        /// The OnGameOfLifePropertyChanged method is called when a property in the GameOfLife model class changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameOfLifePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                StartGameCommand.RaiseCanExecuteChanged();
                StopGameCommand.RaiseCanExecuteChanged();
                StepGameCommand.RaiseCanExecuteChanged();
                ResetGameCommand.RaiseCanExecuteChanged();
                ToggleCellStateCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                throw new Exception("ConwaysGameOfLifeViewModel.OnGameOfLifePropertyChanged(object sender, PropertyChangedEventArgs e): " + ex.ToString());
            }
        }

        #endregion
    }
}
