using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        Title = "Login";
    }

    async void Login_OnClicked(object sender, EventArgs e)
    {
        // User Info
        // u:Jon11
        // p: Jon11
        // e: Jon11@aaa.com
        
        //Validate that something exists for the textboxes (to prevent crashes with calling Trim())
        if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
        {
            await DisplayAlert("Error", "Sorry invalid username or password!", "OK");
            return;
        }
        
        var data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text.Trim(), txtPassword.Text));
        var client = new HttpClient();
        
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"),
            new StringContent(data, Encoding.UTF8, "application/json"));

        var SKey = response.Content.ReadAsStringAsync().Result;

        if (!string.IsNullOrEmpty(SKey) && SKey.Length < 50)
        {
            // Able to get a valid session key with our login info
            App.SessionKey = SKey;
            Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Sorry invalid username or password!", "OK");
            return;
        }
    }

    private void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NewAccountPage());
    }
}