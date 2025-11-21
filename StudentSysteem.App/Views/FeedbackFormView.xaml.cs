using Microsoft.Maui.Controls;

namespace StudentSysteem.App.Views
{
    public partial class FeedbackFormView : ContentPage
    {
        int count = 0;

        public FeedbackFormView()
        {
            InitializeComponent();

        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
