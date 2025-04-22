using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
        Title = "Create New Account";
    }

    private void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        App.SessionKey = "aaa";
        Navigation.PopModalAsync();
    }
}