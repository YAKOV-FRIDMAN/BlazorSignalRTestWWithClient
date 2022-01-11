using AgentClient.Commands;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgentClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region prop

        private bool isConnecting;

        public bool IsConnecting
        {
            get { return isConnecting; }
            set { isConnecting = value; OnPropertyChanged(); }
        }
        private string userNmae;

        public string UserName
        {
            get { return userNmae; }
            set { userNmae = value; OnPropertyChanged(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        #endregion



        HubConnection hubConnection;

        public RelayCommandAsync LoginCommand { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommandAsync(Login);

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44335/chathub") // change
                .Build();


            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                
            });

            hubConnection.On<string>("Conenct", ( message) =>
            {
                MessageBox.Show(message);
            });

            //hubConnection.On<string>("OnConnectedAsync", (d) =>
            //{

            //});
            // await hubConnection.InvokeAsync<string>("Login", userInput);
            hubConnection.StartAsync();
        }
        private async Task Login()
        {
            IsConnecting = true;
            await hubConnection.SendAsync("Login", userNmae);

        }
    }
}
