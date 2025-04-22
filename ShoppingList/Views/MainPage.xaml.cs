using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Views;

public partial class MainPage : ContentPage
{
    private LoginPage LP = new LoginPage();
    public MainPage()
    {
        InitializeComponent();
        Title = "Shopping List Pro";
        this.Loaded += MainPage_Loaded;
        LP.Unloaded += LP_UnLoaded;
    }

    private void LP_UnLoaded(object sender, EventArgs e)
    {
        OnAppearing1();
    }

    private void MainPage_Loaded(object sender, EventArgs e)
    {
        OnAppearing1();
    }

    public void OnAppearing1()
    {
        if (string.IsNullOrEmpty(App.SessionKey))
        {
            Navigation.PushModalAsync(new NavigationPage(LP));
        }
    }

    private void Logout_OnClicked(object sender, EventArgs e)
    {
        App.SessionKey = "";
        OnAppearing1();
    }
}