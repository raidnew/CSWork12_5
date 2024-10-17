using CSWork12_5.Bank.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using CSWork12_5;
using CSWork12_5.Bank.ViewModels;
using CSWork12_5.Bank.Views;
using CSWork12_5.Bank.Models;
using System.Collections.ObjectModel;
using CSWork12_5.Bank.Interfaces;
using System.Windows.Data;

namespace CSWork12_5
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private BankClients _bankClients;

        public App()
        {
            _bankClients = new BankClients();
            _bankClients.Init();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CreateWindowsBankClientsList();
        }

        private void CreateWindowsBankClientsList()
        {
            WindowBanksClients window = new WindowBanksClients();
            WindowBanksClientsViewModel viewModel = new WindowBanksClientsViewModel();
            viewModel.OnClickAddBankClient += CreateWindowAddClient;
            viewModel.OnClickEditBankClient += CreateWindowEditClient;
            viewModel.OnClickTransfer += CreateWindowTransfer;
            viewModel.BankClients = _bankClients.DataCollection;
            window.DataContext = viewModel;
            ShowWindow(window, viewModel);
        }

        private void CreateWindowTransfer(IBankClient client)
        {
            WindowEditBankAccountViewModel viewModel = new WindowEditBankAccountViewModel();
            WindowEditBankAccount window = new WindowEditBankAccount();
            viewModel.CurrentBanksClient = client;
            viewModel.BanksClients = _bankClients.DataCollection;
            viewModel.OnTransferMoney += OnTransferMoney;
            viewModel.OnExit += () => CloseWindow(window);
            window.DataContext = viewModel;
            ShowWindow(window, viewModel);
        }
        private void CreateWindowAddClient()
        {
            WindowEditClientViewModel viewModel = new WindowEditClientViewModel();
            WindowEditClient window = new WindowEditClient();
            viewModel.OnSaveBankClient += (IBankClient client) => OnAddPerson(client);
            viewModel.OnExit += () => CloseWindow(window);
            window.DataContext = viewModel;
            ShowWindow(window, viewModel);
        }

        private void CreateWindowEditClient(IBankClient client)
        {
            WindowEditClientViewModel viewModel = new WindowEditClientViewModel(client);
            WindowEditClient window = new WindowEditClient();
            window.DataContext = viewModel;
            viewModel.OnSaveBankClient += (IBankClient client) => OnSavePerson(client);
            viewModel.OnExit += () => CloseWindow(window);
            ShowWindow(window, viewModel);
        }

        private void OnTransferMoney(IBankClient destnationClient, IBankAccount destinationAccount, IBankClient sourceClient, IBankAccount sourceAccount, float amount)
        {
            _bankClients.TransferMoney(destnationClient, destinationAccount, sourceClient, sourceAccount, amount);
            _bankClients.Save();
        }

        private void OnAddPerson(IBankClient client)
        {
            _bankClients.AddItem(client);
            _bankClients.Save();
        }

        private void OnSavePerson(IBankClient client)
        {
            _bankClients.Save();
        }

        private void ShowWindow(Window window, object viewModel)
        {
            window.Show();
            window.DataContext = viewModel;
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
        
    }
}