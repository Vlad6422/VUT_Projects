using Time2Plan.App.ViewModels;

namespace Time2Plan.App
{
    public partial class MainPage
    {
        
        public MainPage(UserListViewModel userListViewModel) : base(userListViewModel) 
        {
            InitializeComponent();
        }
    }
}