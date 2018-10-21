using ConwaysGameOfLife.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ConwaysGameOfLife.Model
{
    /// <summary>
    /// The GameOfLife class represents an implementation of Conway's Game Of Life.
    /// It is made up of a square grid of cells.
    /// At any point in time, a cell will have one of two states: alive or dead.
    /// </summary>
    public class GameOfLife : NotificationBase
    {
        #region Fields

        private ObservableCollection<Cell> _gridCells;
        private int _stepIntervalMilliSeconds = Constants.DefaultStepIntervalMilliSeconds;
        private DispatcherTimer _stepTimer;
        private bool _isGameRunning = false;
        private static object _updateLock = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameOfLife()
        {
            try
            {
                BuildGrid();
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife(): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets the width of the grid (in cells).
        /// </summary>
        public int GridWidthCells
        {
            get
            {
                return Constants.GridWidth;
            }
        }

        /// <summary>
        /// Gets the height of the grid (in cells).
        /// </summary>
        public int GridHeightCells
        {
            get
            {
                return Constants.GridHeight;
            }
        }

        /// <summary>
        /// Gets the collection of grid cells.
        /// </summary>
        public ObservableCollection<Cell> GridCells
        {
            get
            {
                if (_gridCells == null)
                {
                    _gridCells = new ObservableCollection<Cell>();
                }

                return _gridCells;
            }
            private set
            {
                if (value != null)
                {
                    _gridCells = new ObservableCollection<Cell>(value);
                    RaisePropertyChanged("GridCells");
                }
            }
        }

        /// <summary>
        /// Gets or sets the step interval is milli-seconds.
        /// </summary>
        public int StepIntervalMilliSeconds
        {
            get
            {
                return _stepIntervalMilliSeconds;
            }
            set
            {
                if (value >= Constants.MinimumStepIntervalMilliSeconds && value <= Constants.MaximumStepIntervalMilliSeconds)
                {
                    _stepIntervalMilliSeconds = value;
                    RaisePropertyChanged("StepIntervalMilliSeconds");

                    if (_stepTimer != null && _stepTimer.IsEnabled)
                    {
                        // Update the step timer interval.
                        _stepTimer.Interval = TimeSpan.FromMilliseconds(_stepIntervalMilliSeconds);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the game running boolean flag.
        /// </summary>
        public bool IsGameRunning
        {
            get
            {
                return _isGameRunning;
            }
            private set
            {
                _isGameRunning = value;
                RaisePropertyChanged("IsGameRunning");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The BuildGrid method is called to build (initialise) the grid.
        /// </summary>
        private void BuildGrid()
        {
            try
            {
                if (!IsGameRunning)
                {
                    _gridCells = new ObservableCollection<Cell>();
                    while (_gridCells.Count != Constants.GridWidth * Constants.GridHeight)
                    {
                        _gridCells.Add(new Cell(CellState.Dead));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.BuildGrid(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The BuildGrid method is called to reset the grid.
        /// All grid cells are set to dead.
        /// </summary>
        private void ResetGrid()
        {
            try
            {
                if (!IsGameRunning)
                {
                    foreach (Cell gridCell in _gridCells)
                    {
                        gridCell.CellState = CellState.Dead;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.ResetGrid(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The StartGame method is called to start the game.
        /// </summary>
        public void StartGame()
        {
            try
            {
                if (!IsGameRunning)
                {
                    // Start the step timer.
                    _stepTimer = new DispatcherTimer();
                    _stepTimer.Interval = TimeSpan.FromMilliseconds(_stepIntervalMilliSeconds);
                    _stepTimer.Tick += new EventHandler(StepTimerEventHandler);
                    _stepTimer.Start();

                    IsGameRunning = true; // Set the simulation running flag.
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.StartGame(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The StopGame method is called to stop the game.
        /// </summary>
        public void StopGame()
        {
            try
            {
                // Stop the step timer.
                if (_stepTimer != null && _stepTimer.IsEnabled)
                {
                    _stepTimer.Stop();
                }

                // Clear the simulation running flag.
                if (IsGameRunning)
                {
                    IsGameRunning = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.StopGame(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The StepGame method is called to step through one iteration of the game.
        /// </summary>
        public async Task StepGame()
        {
            try
            {
                // Clear the simulation running flag.
                if (!IsGameRunning)
                {
                    await UpdateGrid();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.StepGame(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The ResetGame method is called to reset the game.
        /// </summary>
        public void ResetGame()
        {
            try
            {
                StopGame();     // Stop the game (if it is running).  
                ResetGrid();    // Reset the grid.
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.ResetGame(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The ToggleCellState method is called to toggle a cell's state.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="gridWidthPixels"></param>
        /// <param name="gridHeightPixels"></param>
        public void ToggleCellState(Point point, double gridWidthPixels, double gridHeightPixels)
        {
            try
            {
                if (!IsGameRunning && point != null && gridWidthPixels > 0 && gridHeightPixels > 0)
                {
                    // Retrieve the x/y coordinates in pixels.
                    double xPositionPixels = point.X;
                    double yPositionPixels = point.Y;

                    // Convert the x/y coordinates from pixels to cells.
                    int xPosition = (int)((xPositionPixels / gridWidthPixels) * Constants.GridWidth);
                    int yPosition = (int)((yPositionPixels / gridHeightPixels) * Constants.GridHeight);

                    // Determine the cell index from the x/y coordinates.
                    int cellIndex = xPosition + (yPosition * Constants.GridWidth);

                    // Retrieve the grid cell and set it to burning.
                    if (IsCellIndexValid(cellIndex))
                    {
                        Cell cell = _gridCells.ElementAt(cellIndex);
                        if (cell.CellState == CellState.Dead)
                        {
                            cell.CellState = CellState.Alive;
                        }
                        else if (cell.CellState == CellState.Alive)
                        {
                            cell.CellState = CellState.Dead;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.ToggleCellState(Point point, double gridWidthPixels, double gridHeightPixels): " + ex.ToString());
            }
        }

        /// <summary>
        /// The StepTimerEventHandler method is called when the step timer expires.
        /// It updates the current state of the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StepTimerEventHandler(object sender, EventArgs e)
        {
            try
            {
                if (IsGameRunning)
                {
                    await UpdateGrid();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.StepTimerEventHandler(object sender, EventArgs e): " + ex.ToString());
            }
        }

        /// <summary>
        /// The UpdateGrid method is called to update the grid.
        /// Each cell is updated as per the rules for Conway's game Of Life.
        /// For more information: https://rosettacode.org/wiki/Conway%27s_Game_of_Life
        /// </summary>
        private async Task UpdateGrid()
        {
            try
            {
                await Task.Run(() =>
                {
                    lock (_updateLock)
                    {

                        ObservableCollection<Cell> updatedGrid = new ObservableCollection<Cell>();    // The updated grid.

                        // Update the grid cells, one cell at a time.
                        foreach (Cell gridCell in _gridCells)
                        {
                            int cellIndex = _gridCells.IndexOf(gridCell);   // Retrieve the index of the cell.
                            
                            // Determine the indexes for all of the neighbours.
                            int topNeighbourIndex = cellIndex - Constants.GridWidth;
                            int topRightNeighbourIndex = cellIndex - Constants.GridWidth + 1;
                            int rightNeighbourIndex = cellIndex + 1;
                            int bottomRightNeighbourIndex = cellIndex + Constants.GridWidth + 1;
                            int bottomNeighbourIndex = cellIndex + Constants.GridWidth;
                            int bottomLeftNeighbourIndex = cellIndex + Constants.GridWidth - 1;
                            int leftNeighbourIndex = cellIndex - 1;
                            int topLeftNeighbourIndex = cellIndex - Constants.GridWidth - 1;

                            // Determine if the cell is on the top/right/bottom/left edge of the grid - certain neighbours must be ignored if a cell is on an edge.
                            bool topEdge = cellIndex < Constants.GridWidth ? true : false;
                            bool rightEdge = ((cellIndex + 1) % Constants.GridWidth) == 0 ? true : false;
                            bool leftEdge = (cellIndex % Constants.GridWidth) == 0 ? true : false;
                            bool bottomEdge = (cellIndex + Constants.GridWidth) >= (Constants.GridWidth * Constants.GridHeight) ? true : false;

                            // Check each neighbour - determine the number of living neighbours.
                            int numberOfLivingNeighbours = 0;
                            if (!topEdge && IsCellAlive(topNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!rightEdge && IsCellAlive(topRightNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!rightEdge && IsCellAlive(rightNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!rightEdge && IsCellAlive(bottomRightNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!bottomEdge && IsCellAlive(bottomNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!leftEdge && IsCellAlive(bottomLeftNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!leftEdge && IsCellAlive(leftNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }
                            if (!leftEdge && IsCellAlive(topLeftNeighbourIndex))
                            {
                                numberOfLivingNeighbours++;
                            }

                            Cell updatedCell = new Cell(gridCell);  // Make a copy of the current grid cell.

                            // Update the cell based on its current status and the number of living neighbours.
                            if (gridCell.CellState == CellState.Alive)
                            {
                                // Cell is currently alive.
                                switch (numberOfLivingNeighbours)
                                {
                                    case 0:
                                    case 1:
                                        updatedCell.CellState = CellState.Dead; // Lonely.
                                        break;
                                    case 2:
                                    case 3:
                                        updatedCell.CellState = CellState.Alive; // Lives.
                                        break;
                                    case 4:
                                    case 5:
                                    case 6:
                                    case 7:
                                    case 8:
                                        updatedCell.CellState = CellState.Dead; // Overcrowded.
                                        break;
                                }
                            }
                            else if (gridCell.CellState == CellState.Dead)
                            {
                                // Cell is currently dead.
                                switch (numberOfLivingNeighbours)
                                {
                                    case 3:
                                        updatedCell.CellState = CellState.Alive; // Takes 3 to give birth.
                                        break;
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 4:
                                    case 5:
                                    case 6:
                                    case 7:
                                    case 8:
                                        updatedCell.CellState = CellState.Dead; // Barren.
                                        break;
                                }
                            }

                            updatedGrid.Add(updatedCell);   // Add the updated grid cell to the updated grid.
                        }

                        // Update the grid.
                        if (updatedGrid.Count == _gridCells.Count)
                        {
                            foreach (Cell gridCell in _gridCells)
                            {
                                CellState updatedCellState = updatedGrid.ElementAt(_gridCells.IndexOf(gridCell)).CellState;
                                if (updatedCellState != gridCell.CellState)
                                {
                                    gridCell.CellState = updatedCellState;
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("GameOfLife.UpdateGrid(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The IsCellIndexValid method is called to determine if a provided cell index is within range of the grid cell collection.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        private bool IsCellIndexValid(int cellIndex)
        {
            return cellIndex >= 0 && cellIndex < _gridCells.Count;
        }

        /// <summary>
        /// The IsCellAlive method is called to determine if a provided cell is alive.
        /// The cell is checked via its index.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        private bool IsCellAlive(int cellIndex)
        {
            return IsCellIndexValid(cellIndex) && _gridCells.ElementAt(cellIndex).CellState == CellState.Alive;
        }

        #endregion
    }
}
