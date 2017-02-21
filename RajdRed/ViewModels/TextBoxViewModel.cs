using RajdRed.Models;
using RajdRed.Repositories;
using RajdRed.Views;
using System.Windows;

namespace RajdRed.ViewModels
{
    public class TextBoxViewModel
    {
        public TextBoxModel TextBoxModel { get; set; }
        public TextBoxRepository TextBoxRepository { get; set; }
        public TextBoxView TextBoxView { get; set; }
        public TextBoxViewModel(Point p, TextBoxRepository tbr)
        {
            TextBoxModel = new TextBoxModel(p, this)
            {
                Text = "New text *"
            };
            TextBoxRepository = tbr;
        }

        public void SetView(TextBoxView t)
        {
            TextBoxView = t;
        }

        public void Select()
        {
            if (!IsSelected())
            {
                TextBoxModel.IsSelected = true;
                TextBoxRepository.IncreaseSelected();
            }
        }

        public void Deselect()
        {
            if (IsSelected())
            {
                TextBoxModel.IsSelected = false;
                TextBoxRepository.DecreaseSelected();
                StopEdit();
            }
        }

        public void Delete()
        {
            Deselect();
            TextBoxRepository.Remove(this);
        }

        public bool IsSelected()
        {
            return (TextBoxModel.IsSelected ? true : false);
        }

        public void Edit()
        {
            if (IsSelected())
            {
                TextBoxModel.Editable = true;
                TextBoxView.TextBoxField.SelectAll();
            }
        }

        public void StopEdit()
        {
            TextBoxModel.Editable = false;
        }
    }
}
