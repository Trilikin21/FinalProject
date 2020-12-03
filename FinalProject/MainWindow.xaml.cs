﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBHandler DBHandler = DBHandler.Instance;

        private List<Person> userList = new List<Person>();

        public MainWindow()
        {
            InitializeComponent();
            readAll();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.ShowDialog();
            readAll();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            //find a way to get the ID so that when the window pop ups, the values are already there.
            if (lvDataBinding.SelectedItem != null)
            {
                Person selectedPerson = (Person)lvDataBinding.SelectedItem;

                UpdateWindow update = new UpdateWindow(selectedPerson.FirstName, selectedPerson.LastName, selectedPerson.Age, selectedPerson.Email, selectedPerson.PhoneNumber, selectedPerson.Id);
                update.ShowDialog();
                readAll();
            }
            else
                MessageBox.Show("No contact selected.", "ERROR", MessageBoxButton.OK);
        }

        private void View(object sender, RoutedEventArgs e)
        {
            if (lvDataBinding.SelectedItem != null)
            {
                Person selectedPerson = (Person)lvDataBinding.SelectedItem;

                DetailsWindow view = new DetailsWindow(selectedPerson.FirstName, selectedPerson.LastName, selectedPerson.Age, selectedPerson.Email, selectedPerson.PhoneNumber);
                view.ShowDialog();
            }
            else
                MessageBox.Show("No contact selected.", "ERROR", MessageBoxButton.OK);
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            Person person = (Person)lvDataBinding.SelectedItem;

            DBHandler.DeleteRecord(person.Id);

            lvDataBinding.ItemsSource = DBHandler.ReadAllPersons();
        }

        private void readAll()
        {
            lvDataBinding.ItemsSource = DBHandler.ReadAllPersons();
        }

        private void lvDataBinding_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvDataBinding.SelectedItem != null)
            {
                Person selectedPerson = (Person)lvDataBinding.SelectedItem;

                DetailsWindow view = new DetailsWindow(selectedPerson.FirstName, selectedPerson.LastName, selectedPerson.Age, selectedPerson.Email, selectedPerson.PhoneNumber);
                view.ShowDialog();
            }
            else
                MessageBox.Show("No contact selected.", "ERROR", MessageBoxButton.OK);
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
                lvDataBinding.ItemsSource = File.ReadAllText(openFileDialog.FileName);
        }

        private void Export(object sender, RoutedEventArgs e)
        {
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
        }
    }
}