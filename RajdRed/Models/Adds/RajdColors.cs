using System.Windows.Media;

namespace RajdRed.Models.Adds
{
	public class RajdColors : Base.RajdElement
	{
		private Brush _klassHeaderBg;
		public Brush KlassHeaderBg
		{
			get { return _klassHeaderBg; }
			set {
				_klassHeaderBg = value;
				OnPropertyChanged("KlassHeaderBg");
			}
		}

		private Brush _klassAttributesBg;
		public Brush KlassAttributesBg
		{
			get { return _klassAttributesBg; }
			set
			{
				_klassAttributesBg = value;
				OnPropertyChanged("KlassAttributesBg");
			}
		}

		private Brush _klassMethodsBg;
		public Brush KlassMethodsBg
		{
			get { return _klassMethodsBg; }
			set
			{
				_klassMethodsBg = value;
				OnPropertyChanged("KlassMethodsBg");
			}
		}

		public Brush KlassNameBg;
        public Brush TheCanvasBg;
        public Brush MenuBotBg;
        public Brush MenuButtonBg;
		public Brush TitleText;

		public RajdColors() {
 			
		}

		public RajdColors(RajdColors r)
		{
			KlassHeaderBg = r.KlassNameBg;
			KlassNameBg = r.KlassNameBg;
			KlassAttributesBg = r.KlassAttributesBg;
			KlassMethodsBg = r.KlassMethodsBg;
			TheCanvasBg = r.TheCanvasBg;
			MenuBotBg = r.MenuBotBg;
			MenuButtonBg = r.MenuButtonBg;
			TitleText = r.TitleText;
		}
	}
}
