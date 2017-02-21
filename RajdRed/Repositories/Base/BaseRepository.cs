using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RajdRed.Repositories.Base
{
    public abstract class BaseRepository<T> : ObservableCollection<T>
    {
        //Använd till vad du vill...
        public Point TempPosition { get; set; }

        private int _numberOfSelected = 0;
        private MainRepository _mainRepository;

        public int NumberOfSelected { get { return _numberOfSelected; } }
        public MainRepository MainRepository { get { return _mainRepository; } }

        /// <summary>
        /// Constructor. Repository is always bind to MainRepository
        /// </summary>
        /// <param name="m"></param>
        public BaseRepository(MainRepository m)
        {
            _mainRepository = m;
        }

        /// <summary>
        /// If repository has any selected
        /// </summary>
        /// <returns>True</returns>
        public bool HasSelected()
        {
            return (_numberOfSelected != 0 ? true : false);
        }

        /// <summary>
        /// Increase number of selected elements in repository
        /// </summary>
        public void IncreaseSelected()
        {
            if (_numberOfSelected != this.Count)
            {
                _numberOfSelected++;
            }
        }

        /// <summary>
        /// Decrease number of selected elements in repository
        /// </summary>
        public void DecreaseSelected()
        {
            if (_numberOfSelected != 0)
            {
                _numberOfSelected--;
            }
        }

        /// <summary>
        /// Selects element in repository and increases number of selected elements
        /// </summary>
        /// <param name="t"></param>
        public virtual void Select(T t) { }

        /// <summary>
        /// Deselects element in repository and increases number of selected elements
        /// </summary>
        /// <param name="t"></param>
        public virtual void Deselect(T t) { }

        /// <summary>
        /// Selects all elements in repository
        /// </summary>
        public virtual void SelectAll() { }

        /// <summary>
        /// Deselects all elements in repository
        /// </summary>
        public virtual void DeselectAll() { }

        /// <summary>
        /// Deletes all selected elements in repository
        /// </summary>
        public virtual void DeleteSelected() { }
    }
}
