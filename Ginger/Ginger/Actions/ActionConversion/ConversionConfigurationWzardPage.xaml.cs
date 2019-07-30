#region License
/*
Copyright © 2014-2019 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using Amdocs.Ginger.Common;
using Amdocs.Ginger.CoreNET;
using Ginger.Actions._Common.ActUIElementLib;
using Ginger.UserControls;
using GingerWPF.WizardLib;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ginger.Actions.ActionConversion
{
    /// <summary>
    /// Interaction logic for ConversionConfigurationWzardPage.xaml
    /// </summary>
    public partial class ConversionConfigurationWzardPage : Page, IWizardPage
    {
        ActionsConversionWizard mWizard;
        ObservableList<string> TargetAppList;
        POMElementGridSelectionPage mPOMControl;

        /// <summary>
        /// Constructor for configuration page
        /// </summary>
        public ConversionConfigurationWzardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Wizard events
        /// </summary>
        /// <param name="WizardEventArgs"></param>
        public void WizardEvent(WizardEventArgs WizardEventArgs)
        {
            switch (WizardEventArgs.EventType)
            {
                case EventType.Init:
                    mWizard = (ActionsConversionWizard)WizardEventArgs.Wizard;
                    break;
                case EventType.Active:
                    Init(WizardEventArgs);
                    break;
            }
        }

        /// <summary>
        /// This methos will set the selected POMs to the wizard object 
        /// </summary>
        private void POMSelectedEventHandler(object sender, string guid)
        {
            if (mWizard.SelectedPOMs != null)
            {
                mWizard.SelectedPOMs.Add(guid);
            }
        }

        /// <summary>
        /// This method is used to init the configuration settings page
        /// </summary>
        /// <param name="WizardEventArgs"></param>
        private void Init(WizardEventArgs WizardEventArgs)
        {
            DataContext = mWizard;
            SetTargetApplicationGridView();

            mPOMControl = new POMElementGridSelectionPage(true, 220, 555);
            mPOMControl.POMSelectionEvent += POMSelectedEventHandler;
            xPOMUserControl.Content = mPOMControl;
        }

        /// <summary>
        /// This method is used to set the columns TargetApplciation GridView
        /// </summary>
        private void SetTargetApplicationGridView()
        {
            //Set the Data Grid columns
            GridViewDef view = new GridViewDef(GridViewDef.DefaultViewName);
            view.GridColsView = new ObservableList<GridColView>();
            TargetAppList = new ObservableList<string>();

            mWizard.ConvertableTargetApplications = GetTargetApplication();

            view.GridColsView.Add(new GridColView() { Field = nameof(ConvertableTargetApplicationDetails.SourceTargetApplicationName), WidthWeight = 15, ReadOnly = true, Header = "Source - Taret Application" });
            view.GridColsView.Add(new GridColView()
            {
                Field = nameof(ConvertableTargetApplicationDetails.TargetTargetApplicationName),
                BindingMode = BindingMode.TwoWay,
                StyleType = GridColView.eGridColStyleType.ComboBox,
                CellValuesList = TargetAppList,
                WidthWeight = 15,
                Header = "Map to - Target Application"
            });
            xTargetApplication.SetAllColumnsDefaultView(view);
            xTargetApplication.InitViewItems();
            xTargetApplication.DataSourceList = mWizard.ConvertableTargetApplications;
            xTargetApplication.ShowTitle = Visibility.Collapsed;
        }
        
        /// <summary>
        /// This event is used to expand the Target Application grid which helps to map the target application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlsViewsExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(((System.Windows.FrameworkElement)sender).Name) == TargetApplicationExpander.Name)
            {
                ControlsViewRow.Height = new GridLength(230);
                ControlsViewRow.MaxHeight = Double.PositiveInfinity;
            }
            else
            {
                POMControlsViewRow.Height = new GridLength(270);
                POMControlsViewRow.MaxHeight = Double.PositiveInfinity;
            }
        }

        /// <summary>
        /// This event is used to collapsed the Target Application grid which helps to map the target application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlsViewsExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(((System.Windows.FrameworkElement)sender).Name) == TargetApplicationExpander.Name)
            {
                ControlsViewRow.Height = new GridLength(35);
                ControlsViewRow.MaxHeight = 35;
            }
            else
            {
                POMControlsViewRow.Height = new GridLength(35);
                POMControlsViewRow.MaxHeight = 35;
            }
        }
        
        /// <summary>
        /// This method is used to get the Target Application
        /// </summary>
        /// <returns></returns>
        private ObservableList<ConvertableTargetApplicationDetails> GetTargetApplication()
        {
            ObservableList<ConvertableTargetApplicationDetails> lstTA = new ObservableList<ConvertableTargetApplicationDetails>();
            // fetching list of selected convertible activities from the first grid
            if (mWizard.ConversionType == ActionsConversionWizard.eActionConversionType.SingleBusinessFlow)
            {
                foreach (var targetBase in mWizard.Context.BusinessFlow.TargetApplications)
                {
                    if (!TargetAppList.Contains(targetBase.ItemName))
                    {
                        lstTA.Add(new ConvertableTargetApplicationDetails() { SourceTargetApplicationName = targetBase.ItemName, TargetTargetApplicationName = targetBase.ItemName });
                        TargetAppList.Add(targetBase.ItemName);
                    }
                }
            }
            else
            {
                foreach (var targetBase in mWizard.ListOfBusinessFlow.Where(x => x.SelectedForConversion).SelectMany(y => y.TargetApplications))
                {
                    if (!TargetAppList.Contains(targetBase.ItemName))
                    {
                        lstTA.Add(new ConvertableTargetApplicationDetails() { SourceTargetApplicationName = targetBase.ItemName, TargetTargetApplicationName = targetBase.ItemName });
                        TargetAppList.Add(targetBase.ItemName);
                    }
                }
            }

            return lstTA;
        }
    }
}
