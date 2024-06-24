﻿using DataAccess.Redis;
using LibraryApplication.Helper;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace LibraryApplication.View
{
    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();

            ConnectionHelper.Connection.On<bool>("LoginResult", isSuccess =>
            {
                if (isSuccess)
                {

                    this.Dispatcher.Invoke(() =>
                    {
                        ConnectionHelper.SetLoggedUser(new Data.Entity.User() { UserName = UserNameTextBox.Text, Password = PasswordBox.Password });
                        Library library = new Library();
                        library.Show();
                        this.Close();
                    });
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı girildi");
                }
            });
        }
        /// <summary>
        /// kullanıcı giriş yapar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Loggin_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var redis = new RedisRepository())
            {
                redis.SetAsync(UserNameTextBox.Text, TimeSpan.FromMinutes(1));
                Console.Write(redis.Get("name"));
                
                  
            }
                await ConnectionHelper.Connection.InvokeAsync("Login", UserNameTextBox.Text, PasswordBox.Password);
        }
    }
}
