using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
        Title = "Create New Account";
    }

    // Validates that an email meets email requirements
    // and ensures that a "." comes after the @ part (a "." is within the domain of the address)
    static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        email = email.Trim();
        
        try
        {
            var address = new MailAddress(email);
            return address.Host.Contains(".");
        }
        catch
        {
            return false;
        }
    }
    
    async void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        
        //user input validation stuff
        //username: must start with AND contain a letter, otherwise it can be any amount of letters/numbers/underscores
        if (string.IsNullOrEmpty(txtUser.Text) || !Regex.IsMatch(txtUser.Text.Trim(), @"^[A-Za-z][A-Za-z0-9_]*$"))
        {
            await DisplayAlert(
                "Username Invalid", 
                "\n1. Username must start with a letter\n\n2. Username must contain only letters, numbers, and underscores", 
                "OK");
            return;
        }
        
        //password: must have at least 1 or more characters that are a letter, number, or special character (excluding spaces)
        if (string.IsNullOrEmpty(txtPassword1.Text)
            || !Regex.IsMatch(txtPassword1.Text, @"^[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':\""|,.<>\/?]+$"))
        {
            await DisplayAlert(
                "Password Invalid", 
                "Password must contain 1 or more letters, numbers, or special characters (backslashes and spaces not allowed)", 
                "OK");
            return;
        }
        
        //password must be retyped and match
        if (string.IsNullOrEmpty(txtPassword2.Text)
            || !(txtPassword1.Text == txtPassword2.Text))
        {
            await DisplayAlert(
                "Passwords don't match", 
                "Your password and the retyped password do not match.", 
                "OK");
            return;
        }
        
        //email must follow the rules of a valid email address
        if (string.IsNullOrEmpty(txtEmail.Text) || !IsValidEmail(txtEmail.Text.Trim()))
        {
            await DisplayAlert(
                "Email address Invalid", 
                "Please ensure your email address is valid.", 
                "OK");
            return;
        }
        
        
        //api stuff
        var data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text.Trim(), txtPassword1.Text, txtEmail.Text.Trim()));

        var client = new HttpClient();
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"),
            new StringContent(data, Encoding.UTF8, "application/json"));

        var AccountStatus = response.Content.ReadAsStringAsync().Result;
        
        //checking if the account is valid
        if (AccountStatus == "user exists")
        {
            await DisplayAlert("Error", "Sorry this username has been taken!", "OK");
            return;
        }
        
        if (AccountStatus == "email exists")
        {
            await DisplayAlert("Error", "Sorry this email has been taken!", "OK");
            return;
        }
        
        //what to do if new user created
        if (AccountStatus == "complete")
        {
            response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"),
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
                await DisplayAlert("Error", "Sorry there was an issue logging you in!", "OK");
                return;
            }
        }
        //if error occurs on api end it will pass exception info as account status, this code handles those errors
        else
        {
            await DisplayAlert("Error", "Sorry there was an error creating your account!", "OK");
            return;
        }
        
    }
}