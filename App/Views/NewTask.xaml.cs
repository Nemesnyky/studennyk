using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.ViewModels;

namespace App.Views;

public partial class NewTask : ContentView
{
    public DateTime newTaskDate;
    public TimeSpan newTaskTime;
    public NewTask()
    {
        InitializeComponent();
        BindingContext = new NewTaskViewModel();

    }
    void DescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        string oldTextDescription = e.OldTextValue;
        string newTextDescription = e.NewTextValue;
        string textDescription = entryDescription.Text;
    }
    void TitleTextChanged(object sender, TextChangedEventArgs e)
    {
        string oldTextTitle = e.OldTextValue;
        string newTextTitle = e.NewTextValue;
        string textTitle = entryTitle.Text;
    }
    private void NewTaskDatePicker(object sender, DateChangedEventArgs e)
    {
        newTaskDate = newTaskDatePicker.Date;
    }
    void NewTaskTimePicker(object sender, PropertyChangedEventArgs args)
    {
        newTaskTime = newTaskTimePicker.Time;
    }
    void RewriteNewTask()
    {

    }
}