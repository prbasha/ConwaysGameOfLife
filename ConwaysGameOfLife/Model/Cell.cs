using ConwaysGameOfLife.Common;
using System;

namespace ConwaysGameOfLife.Model
{
    /// <summary>
    /// The Cell class represents a single cell.
    /// </summary>
    public class Cell : NotificationBase
    {
        #region Fields

        private CellState _cellState;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Cell()
        {
            _cellState = CellState.Dead;
        }

        /// <summary>
        /// Constructor.
        /// Creates a cell with the provided state.
        /// </summary>
        public Cell(CellState cellState)
        {
            _cellState = cellState;
        }

        /// <summary>
        /// Create a cell from an existing cell.
        /// </summary>
        /// <param name="cell"></param>
        public Cell(Cell cell)
        {
            try
            {
                if (cell == null)
                {
                    throw new Exception("cell can not be null");
                }

                _cellState = cell.CellState;
            }
            catch (Exception ex)
            {
                throw new Exception("Cell(Cell cell): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cell state.
        /// </summary>
        public CellState CellState
        {
            get
            {
                return _cellState;
            }
            set
            {
                _cellState = value;
                RaisePropertyChanged("CellState");
            }
        }

        #endregion

        #region Methods
        #endregion
    }
}
